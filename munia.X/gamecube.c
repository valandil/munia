#include "gamecube.h"
#include "hardware.h"
#include "globals.h"
#include <usb/usb_device_hid.h>
#include "gamepad.h"
#include "fakeout.h"
#include "menu.h"
#include "uarts.h"

ngc_packet_t joydata_ngc_last;

void ngc_tasks() {
    if (packets.ngc_test) {
        if (config.snes_mode & 2) ngc_fakeout_test();
        else ngc_handle_packet();
        packets.ngc_test = false;
    }
    
    if (pollNeeded && (in_menu || config.ngc_mode == NGC_MODE_N64 || (config.ngc_mode == NGC_MODE_PC && USB_READY))) {
        USBDeviceTasks();
        di();
        ngc_poll();
        // waste some more instructions before sampling
        _delay(40);
        asm("lfsr 0, _sample_buff+25"); // setup FSR0
        ngc_sample();
        asm("movff FSR0L, _sample_w+0"); // update sample_w
        asm("movff FSR0H, _sample_w+1");
        ei();
        USBDeviceTasks();
    }    

    if (packets.ngc_avail) {
        // see if this packet is equal to the last transmitted one, and if so, discard it
        if (memcmp(&joydata_ngc, &joydata_ngc_last, sizeof(ngc_packet_t)) != 0) {
            // new, changed packet available; unpack if faking and send over usb
            if (config.ngc_mode == NGC_MODE_N64) {
                ngc_create_n64_fake();
                fake_unpack((uint8_t*)&joydata_n64_raw, sizeof(n64_packet_t));
            }
            if (!in_menu && USB_READY && !HIDTxHandleBusy(USBInHandleNGC)) {
                USBInHandleNGC = HIDTxPacket(HID_EP_NGC, (uint8_t*)&joydata_ngc, sizeof(ngc_packet_t));
                // save last packet
                memcpy(&joydata_ngc_last, &joydata_ngc, sizeof(ngc_packet_t));
            }   
        }
        packets.ngc_avail = false; // now consumed    
    }    
}

void ngc_poll() {
    portc_mask = 0b00000001;
    LATC &= ~portc_mask; // pull down - always call this before CLR() calls
    CLR(); // set data pin to output, making the pin low
    // send 01000000
    //      00000011    
    //      00000010
    LOW(); HIGH(); LOW(); LOW(); LOW(); LOW(); LOW(); LOW(); 
    LOW(); LOW(); LOW(); LOW(); LOW(); LOW(); HIGH(); HIGH(); 
    LOW(); LOW(); LOW(); LOW(); LOW(); LOW(); LOW(); LOW(); 
    
    // stop bit, 2 us
    CLR(); 
    _delay(22);
    SET();// back set to open collector input with pull up
    LATC |= portc_mask; // reset pull up
}

void ngc_handle_packet() {
    // translate samples in sample_buff to joydata_ngc
	uint8_t idx = sample_w - sample_buff;
    // dbgs("ngc packet len: "); dbgsval(idx); dbgs("\n");
    
    // 89 = 24 bits request, 1 stopbit, + 64 bits of data
	if (idx == 89 && !HIDTxHandleBusy(USBInHandleNGC)) {        
		uint8_t* w = (uint8_t*)&joydata_ngc;
        
		// bit 0 is not sampled, then 0-23 are request, 24 is stop bit, 25-88 is data
		int8_t* r = (int8_t*)(sample_buff + 25);
		for (uint8_t i = 0; i < sizeof(ngc_packet_t); i++) {
            *w++ = pack_byte(r);
            r += 8;
		}
        
        // these are inverted in HID reports
        joydata_ngc_raw = joydata_ngc;
        joydata_ngc.joy_y = -joydata_ngc.joy_y;
        joydata_ngc.c_y = -joydata_ngc.c_y;
        joydata_ngc.hat = hat_lookup_ngc[joydata_ngc.hat];
        
        packets.ngc_avail = true;
	}
}