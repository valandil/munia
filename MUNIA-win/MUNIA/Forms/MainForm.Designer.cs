﻿using OpenTK;

namespace MUNIA.Forms {
    partial class MainForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.tsmiControllers = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiOptions = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiSetWindowSize = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiMuniaSettings = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiFirmware = new System.Windows.Forms.ToolStripMenuItem();
			this.setCaptureLagCompensationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mapArduinoDevicesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiHelp = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiCheckUpdates = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiAbout = new System.Windows.Forms.ToolStripMenuItem();
			this.glControl = new OpenTK.GLControl();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.lblSkins = new System.Windows.Forms.ToolStripStatusLabel();
			this.lblFill = new System.Windows.Forms.ToolStripStatusLabel();
			this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
			this.pbProgress = new System.Windows.Forms.ToolStripProgressBar();
			this.menuStrip1.SuspendLayout();
			this.statusStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiControllers,
            this.tsmiOptions,
            this.tsmiHelp});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(577, 24);
			this.menuStrip1.TabIndex = 1;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// tsmiControllers
			// 
			this.tsmiControllers.Name = "tsmiControllers";
			this.tsmiControllers.Size = new System.Drawing.Size(104, 20);
			this.tsmiControllers.Text = "&Select controller";
			// 
			// tsmiOptions
			// 
			this.tsmiOptions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiSetWindowSize,
            this.tsmiMuniaSettings,
            this.tsmiFirmware,
            this.setCaptureLagCompensationToolStripMenuItem,
            this.mapArduinoDevicesToolStripMenuItem});
			this.tsmiOptions.Name = "tsmiOptions";
			this.tsmiOptions.Size = new System.Drawing.Size(61, 20);
			this.tsmiOptions.Text = "&Options";
			// 
			// tsmiSetWindowSize
			// 
			this.tsmiSetWindowSize.Name = "tsmiSetWindowSize";
			this.tsmiSetWindowSize.Size = new System.Drawing.Size(188, 22);
			this.tsmiSetWindowSize.Text = "Set &window size";
			this.tsmiSetWindowSize.Click += new System.EventHandler(this.tsmiSetWindowSize_Click);
			// 
			// tsmiMuniaSettings
			// 
			this.tsmiMuniaSettings.Name = "tsmiMuniaSettings";
			this.tsmiMuniaSettings.Size = new System.Drawing.Size(188, 22);
			this.tsmiMuniaSettings.Text = "&MUNIA &config";
			this.tsmiMuniaSettings.Click += new System.EventHandler(this.tsmiMuniaSettings_Click);
			// 
			// tsmiFirmware
			// 
			this.tsmiFirmware.Name = "tsmiFirmware";
			this.tsmiFirmware.Size = new System.Drawing.Size(188, 22);
			this.tsmiFirmware.Text = "MUNIA &firmware";
			this.tsmiFirmware.Click += new System.EventHandler(this.tsmiFirmware_Click);
			// 
			// setCaptureLagCompensationToolStripMenuItem
			// 
			this.setCaptureLagCompensationToolStripMenuItem.Name = "setCaptureLagCompensationToolStripMenuItem";
			this.setCaptureLagCompensationToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
			this.setCaptureLagCompensationToolStripMenuItem.Text = "Set &lag compensation";
			this.setCaptureLagCompensationToolStripMenuItem.Click += new System.EventHandler(this.tsmiSetLagCompensation);
			// 
			// mapArduinoDevicesToolStripMenuItem
			// 
			this.mapArduinoDevicesToolStripMenuItem.Name = "mapArduinoDevicesToolStripMenuItem";
			this.mapArduinoDevicesToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
			this.mapArduinoDevicesToolStripMenuItem.Text = "Map arduino devices";
			this.mapArduinoDevicesToolStripMenuItem.Click += new System.EventHandler(this.tsmiMapArduinoDevicesClick);
			// 
			// tsmiHelp
			// 
			this.tsmiHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiCheckUpdates,
            this.tsmiAbout});
			this.tsmiHelp.Name = "tsmiHelp";
			this.tsmiHelp.Size = new System.Drawing.Size(44, 20);
			this.tsmiHelp.Text = "&Help";
			// 
			// tsmiCheckUpdates
			// 
			this.tsmiCheckUpdates.Name = "tsmiCheckUpdates";
			this.tsmiCheckUpdates.Size = new System.Drawing.Size(170, 22);
			this.tsmiCheckUpdates.Text = "&Check for updates";
			this.tsmiCheckUpdates.Click += new System.EventHandler(this.tsmiCheckUpdates_Click);
			// 
			// tsmiAbout
			// 
			this.tsmiAbout.Name = "tsmiAbout";
			this.tsmiAbout.Size = new System.Drawing.Size(170, 22);
			this.tsmiAbout.Text = "&About";
			this.tsmiAbout.Click += new System.EventHandler(this.tsmiAbout_Click);
			// 
			// glControl
			// 
			this.glControl.BackColor = System.Drawing.Color.Black;
			this.glControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.glControl.Location = new System.Drawing.Point(0, 24);
			this.glControl.Name = "glControl";
			this.glControl.Size = new System.Drawing.Size(577, 336);
			this.glControl.TabIndex = 0;
			this.glControl.VSync = false;
			this.glControl.Load += new System.EventHandler(this.glControl_Load);
			this.glControl.MouseClick += new System.Windows.Forms.MouseEventHandler(this.glControl_MouseClick);
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblSkins,
            this.lblFill,
            this.lblStatus,
            this.pbProgress});
			this.statusStrip1.Location = new System.Drawing.Point(0, 360);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(577, 22);
			this.statusStrip1.TabIndex = 2;
			this.statusStrip1.Text = "statusStrip";
			// 
			// lblSkins
			// 
			this.lblSkins.Name = "lblSkins";
			this.lblSkins.Size = new System.Drawing.Size(0, 17);
			// 
			// lblFill
			// 
			this.lblFill.Name = "lblFill";
			this.lblFill.Size = new System.Drawing.Size(418, 17);
			this.lblFill.Spring = true;
			// 
			// lblStatus
			// 
			this.lblStatus.Name = "lblStatus";
			this.lblStatus.Size = new System.Drawing.Size(42, 17);
			this.lblStatus.Text = "Status:";
			// 
			// pbProgress
			// 
			this.pbProgress.Name = "pbProgress";
			this.pbProgress.Size = new System.Drawing.Size(100, 16);
			this.pbProgress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(577, 382);
			this.Controls.Add(this.glControl);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.menuStrip1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "MainForm";
			this.Text = "MUNIA";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
			this.Shown += new System.EventHandler(this.MainForm_Shown);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tsmiControllers;
        private GLControl glControl;
		private System.Windows.Forms.ToolStripMenuItem tsmiOptions;
		private System.Windows.Forms.ToolStripMenuItem tsmiSetWindowSize;
		private System.Windows.Forms.ToolStripMenuItem tsmiHelp;
		private System.Windows.Forms.ToolStripMenuItem tsmiCheckUpdates;
		private System.Windows.Forms.ToolStripMenuItem tsmiAbout;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripStatusLabel lblStatus;
		private System.Windows.Forms.ToolStripStatusLabel lblFill;
		private System.Windows.Forms.ToolStripProgressBar pbProgress;
		private System.Windows.Forms.ToolStripStatusLabel lblSkins;
		private System.Windows.Forms.ToolStripMenuItem tsmiMuniaSettings;
		private System.Windows.Forms.ToolStripMenuItem tsmiFirmware;
		private System.Windows.Forms.ToolStripMenuItem setCaptureLagCompensationToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem mapArduinoDevicesToolStripMenuItem;
	}
}

