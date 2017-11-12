#ifndef REPORT_DESCRIPTORS_H
#define	REPORT_DESCRIPTORS_H


//Class specific descriptor - HID
const uint8_t hid_rpt_ngc[] = {
    0x05, 0x01,                    // USAGE_PAGE (Generic Desktop)
    0x09, 0x04,                    // USAGE (Joystick)
    0xa1, 0x01,                    // COLLECTION (Application)

    // a b x y start
    0x05, 0x09,                    //   USAGE_PAGE (Button)
    0x19, 0x01,                    //   USAGE_MINIMUM (Button 1)
    0x29, 0x05,                    //   USAGE_MAXIMUM (Button 5)
    0x15, 0x00,                    //   LOGICAL MINIMUM (0)
    0x25, 0x01,                    //   LOGICAL_MAXIMUM (1)
    0x95, 0x05,                    //   REPORT_COUNT (5)
    0x75, 0x01,                    //   REPORT_SIZE (1)
    0x81, 0x02,                    //   INPUT (Data,Var,Abs)
    
    //  zero zero zero
    0x09, 0x00,		               //   USAGE (Undefined)
    0x95, 0x01,                    //   REPORT_COUNT (1)
    0x75, 0x03,                    //   REPORT_SIZE (3)
    0x81, 0x03,                    //   INPUT (Cnst,Var,Abs)
    
    // hat
    0x05, 0x01,                    //   USAGE_PAGE(Generic Desktop)
    0x09, 0x39,                    //   USAGE(Hat Switch)
    0x15, 0x00,                    //   LOGICAL MINIMUM (0)
    0x25, 0x07,                    //   LOGICAL MAXIMUM (7)
    0x46, 0x3b, 0b01,              //   PHYSICAL MAXIMUM (315)
    0x65, 0x14,                    //   UNIT (English,Rot,Ang.Pos))
    0x75, 0x04,                    //   REPORT_SIZE(4)
    0x95, 0x01,                    //   REPORT_COUNT(1)
    0x81, 0x42,                    //   INPUT(Data,Var,Abs,Null)

    0x05, 0x09,                    //   USAGE_PAGE (Button)
    0x19, 0x06,                    //   USAGE_MINIMUM (Button 6)
    0x29, 0x08,                    //   USAGE_MAXIMUM (Button 8)
    0x15, 0x00,                    //   LOGICAL_MINIMUM (0)
    0x25, 0x01,                    //   LOGICAL_MAXIMUM (1)
    0x95, 0x03,                    //   REPORT_COUNT (3)
    0x75, 0x01,                    //   REPORT_SIZE (1)
    0x81, 0x02,                    //   INPUT (Data,Var,Abs)
            
    0x09, 0x00,		               //   USAGE (Undefined)
    0x15, 0x00,                    //   LOGICAL MINIMUM (0)
    0x25, 0x01,                    //   LOGICAL_MAXIMUM (1)
    0x95, 0x01,                    //   REPORT_COUNT (1)
    0x75, 0x01,                    //   REPORT_SIZE (1)
    0x81, 0x03,                    //   INPUT (Cnst,Var,Abs)
    
    // x,y is left stick, rx,ry is c-stick
    0x05, 0x01,                    //   USAGE_PAGE (Generic Desktop)
    0x09, 0x30,                    //   USAGE (X)
    0x09, 0x31,                    //   USAGE (Y)
    0x09, 0x33,                    //   USAGE (Rx)
    0x09, 0x34,                    //   USAGE (Ry)
    0x15, 0x00,                    //   LOGICAL MINIMUM (0)
    0x26, 0xff, 0x00,              //   LOGICAL_MAXIMUM (255)
    0x75, 0x08,                    //   REPORT_SIZE (8)
    0x95, 0x04,                    //   REPORT_COUNT (4)
    0x81, 0x02,                    //   INPUT (Data,Var,Abs)
    
    // z = left trigger, rz = right trigger
    0x09, 0x32,                    //   USAGE (X)
    0x09, 0x35,                    //   USAGE (Rz)
    0x15, 0x00,                    //   LOGICAL MINIMUM (0)
    0x26, 0xff, 0x00,              //   LOGICAL_MAXIMUM (255)
    0x75, 0x08,                    //   REPORT_SIZE (8)
    0x95, 0x02,                    //   REPORT_COUNT (2)
    0x81, 0x02,                    //   INPUT (Data,Var,Abs)

    0xc0,                          // END_COLLECTION
};


#define CFG_CMD_READ  0x47        // if return length
#define CFG_CMD_WRITE 0x44
#define CFG_CMD_WRITE_LEGACY 0x46 // used with old menu structure
#define CFG_CMD_ENTER_BL  0x48
#define CFG_CMD_REPORT_SIZE 0x08

const uint8_t hid_rpt_cfg[] = {
    0x06, 0x00, 0xFF,              // Usage Page = 0xFF00 (Vendor Defined Page 1)
    0x09, 0x01,                    // USAGE (Vendor Usage 1)
    0xa1, 0x01,                    // COLLECTION (Application)
    0x05, 0x01,                    //   USAGE_PAGE (Generic Desktop)
    0x15, 0x00,                    //     LOGICAL_MINIMUM (0)
    0x26, 0xff, 0x00,              //     LOGICAL_MAXIMUM (255)

    // write config command
    0x09, 0x00,                    //     USAGE (Undefined)
    0x95, CFG_CMD_REPORT_SIZE,     //     REPORT_COUNT
    0x29, CFG_CMD_REPORT_SIZE,     //     USAGE_MAXIMUM
    0x19, 0x01,                    //     USAGE_MINIMUM
    0x75, 0x08,                    //     REPORT_SIZE (8)
    0x91, 0x00,                    //     OUTPUT (Data,Ary,Abs)
    0x29, CFG_CMD_REPORT_SIZE,     //     USAGE_MAXIMUM
    0x19, 0x01,                    //     USAGE_MINIMUM
    0x81, 0x00,                    //     INPUT (Data,Ary,Abs)

    0xc0                           // END_COLLECTION    
};

#define HID_RPT_NGC_SIZE          sizeof(hid_rpt_ngc)
#define HID_RPT_CFG_SIZE          sizeof(hid_rpt_cfg)

#define USB_ARRAYLEN(X) (sizeof(X)/sizeof(*X))
#define STATIC_SIZE_CHECK_EQUAL(X,Y) typedef char USB_CONCAT(STATIC_SIZE_CHECK_LINE_,__LINE__) [(X==Y)?1:-1]
#define USB_CONCAT(X,Y)  USB_CONCAT_HIDDEN(X,Y)
#define USB_CONCAT_HIDDEN(X,Y) X ## Y

STATIC_SIZE_CHECK_EQUAL(sizeof(hid_rpt_ngc), HID_RPT_NGC_SIZE);
STATIC_SIZE_CHECK_EQUAL(sizeof(hid_rpt_cfg), HID_RPT_CFG_SIZE);

#endif	/* REPORT_DESCRIPTORS_H */
