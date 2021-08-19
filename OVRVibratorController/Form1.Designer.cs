
using System.Drawing;

namespace VibratorController {
    partial class Form1 {
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
            this.setHold = new System.Windows.Forms.Button();
            this.setLock = new System.Windows.Forms.Button();
            this.add = new System.Windows.Forms.Button();
            this.setHoldText = new System.Windows.Forms.TextBox();
            this.setLockText = new System.Windows.Forms.TextBox();
            this.addToyText = new System.Windows.Forms.TextBox();
            this.serverStatus = new System.Windows.Forms.Label();
            this.ovrStatus = new System.Windows.Forms.Label();
            this.dropdown1 = new System.Windows.Forms.ComboBox();
            this.slider1 = new System.Windows.Forms.TrackBar();
            this.name1 = new System.Windows.Forms.Label();
            this.controllerStatus = new System.Windows.Forms.Label();
            this.rotateButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.slider1)).BeginInit();
            this.SuspendLayout();
            // 
            // setHold
            // 
            this.setHold.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.setHold.Location = new System.Drawing.Point(12, 12);
            this.setHold.Name = "setHold";
            this.setHold.Size = new System.Drawing.Size(92, 23);
            this.setHold.TabIndex = 0;
            this.setHold.Text = "Set Hold Button";
            this.setHold.UseVisualStyleBackColor = true;
            this.setHold.Click += new System.EventHandler(this.setHold_Click);
            // 
            // setLock
            // 
            this.setLock.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.setLock.Location = new System.Drawing.Point(110, 12);
            this.setLock.Name = "setLock";
            this.setLock.Size = new System.Drawing.Size(92, 23);
            this.setLock.TabIndex = 1;
            this.setLock.Text = "Set Lock Button";
            this.setLock.UseVisualStyleBackColor = true;
            this.setLock.Click += new System.EventHandler(this.setLock_Click);
            // 
            // add
            // 
            this.add.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.add.Location = new System.Drawing.Point(208, 12);
            this.add.Name = "add";
            this.add.Size = new System.Drawing.Size(92, 23);
            this.add.TabIndex = 2;
            this.add.Text = "Add Toy";
            this.add.UseVisualStyleBackColor = true;
            this.add.Click += new System.EventHandler(this.add_Click);
            // 
            // setHoldText
            // 
            this.setHoldText.Enabled = false;
            this.setHoldText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(146)))), ((int)(((byte)(151)))));
            this.setHoldText.Location = new System.Drawing.Point(12, 41);
            this.setHoldText.Name = "setHoldText";
            this.setHoldText.Size = new System.Drawing.Size(92, 20);
            this.setHoldText.TabIndex = 3;
            this.setHoldText.Text = "None";
            this.setHoldText.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // setLockText
            // 
            this.setLockText.Enabled = false;
            this.setLockText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(146)))), ((int)(((byte)(151)))));
            this.setLockText.Location = new System.Drawing.Point(110, 41);
            this.setLockText.Name = "setLockText";
            this.setLockText.Size = new System.Drawing.Size(92, 20);
            this.setLockText.TabIndex = 4;
            this.setLockText.Text = "None";
            this.setLockText.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // addToyText
            // 
            this.addToyText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(146)))), ((int)(((byte)(151)))));
            this.addToyText.Location = new System.Drawing.Point(208, 41);
            this.addToyText.Name = "addToyText";
            this.addToyText.Size = new System.Drawing.Size(92, 20);
            this.addToyText.TabIndex = 5;
            this.addToyText.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // serverStatus
            // 
            this.serverStatus.AutoSize = true;
            this.serverStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.serverStatus.Location = new System.Drawing.Point(306, 12);
            this.serverStatus.Name = "serverStatus";
            this.serverStatus.Size = new System.Drawing.Size(134, 13);
            this.serverStatus.TabIndex = 8;
            this.serverStatus.Text = "Disconnected from server..";
            this.serverStatus.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // ovrStatus
            // 
            this.ovrStatus.AutoSize = true;
            this.ovrStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.ovrStatus.Location = new System.Drawing.Point(306, 25);
            this.ovrStatus.Name = "ovrStatus";
            this.ovrStatus.Size = new System.Drawing.Size(108, 13);
            this.ovrStatus.TabIndex = 9;
            this.ovrStatus.Text = "OVR not connected..";
            // 
            // dropdown1
            // 
            this.dropdown1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dropdown1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(146)))), ((int)(((byte)(151)))));
            this.dropdown1.FormattingEnabled = true;
            this.dropdown1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.dropdown1.Items.AddRange(new object[] {
            "Slider Only",
            "Left",
            "Right",
            "Both"});
            this.dropdown1.Location = new System.Drawing.Point(9, 414);
            this.dropdown1.Name = "dropdown1";
            this.dropdown1.Size = new System.Drawing.Size(95, 21);
            this.dropdown1.TabIndex = 5;
            this.dropdown1.Tag = "";
            this.dropdown1.Visible = false;
            // 
            // slider1
            // 
            this.slider1.AllowDrop = true;
            this.slider1.LargeChange = 1;
            this.slider1.Location = new System.Drawing.Point(110, 388);
            this.slider1.Maximum = 20;
            this.slider1.Name = "slider1";
            this.slider1.Size = new System.Drawing.Size(504, 45);
            this.slider1.TabIndex = 7;
            this.slider1.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.slider1.Visible = false;
            // 
            // name1
            // 
            this.name1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.name1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(146)))), ((int)(((byte)(151)))));
            this.name1.Location = new System.Drawing.Point(9, 390);
            this.name1.Name = "name1";
            this.name1.Size = new System.Drawing.Size(95, 21);
            this.name1.TabIndex = 13;
            this.name1.Text = "Hush";
            this.name1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.name1.Visible = false;
            // 
            // controllerStatus
            // 
            this.controllerStatus.AutoSize = true;
            this.controllerStatus.ForeColor = System.Drawing.Color.Khaki;
            this.controllerStatus.Location = new System.Drawing.Point(306, 38);
            this.controllerStatus.Name = "controllerStatus";
            this.controllerStatus.Size = new System.Drawing.Size(169, 13);
            this.controllerStatus.TabIndex = 14;
            this.controllerStatus.Text = "Can\'t find left and right controllers..";
            // 
            // rotateButton
            // 
            this.rotateButton.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.rotateButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rotateButton.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.rotateButton.Location = new System.Drawing.Point(110, 388);
            this.rotateButton.Name = "rotateButton";
            this.rotateButton.Size = new System.Drawing.Size(504, 45);
            this.rotateButton.TabIndex = 15;
            this.rotateButton.Text = "Rotate";
            this.rotateButton.UseVisualStyleBackColor = false;
            this.rotateButton.Visible = false;
            // 
            // Form1
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(57)))), ((int)(((byte)(63)))));
            this.ClientSize = new System.Drawing.Size(626, 445);
            this.Controls.Add(this.rotateButton);
            this.Controls.Add(this.controllerStatus);
            this.Controls.Add(this.name1);
            this.Controls.Add(this.dropdown1);
            this.Controls.Add(this.slider1);
            this.Controls.Add(this.ovrStatus);
            this.Controls.Add(this.serverStatus);
            this.Controls.Add(this.addToyText);
            this.Controls.Add(this.setLockText);
            this.Controls.Add(this.setHoldText);
            this.Controls.Add(this.add);
            this.Controls.Add(this.setLock);
            this.Controls.Add(this.setHold);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "Form1";
            this.Text = "OVR Vibrator Controller";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.slider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button setHold;
        private System.Windows.Forms.Button setLock;
        private System.Windows.Forms.Button add;
        private System.Windows.Forms.TextBox setHoldText;
        private System.Windows.Forms.TextBox setLockText;
        private System.Windows.Forms.TextBox addToyText;
        private System.Windows.Forms.Label serverStatus;
        private System.Windows.Forms.Label ovrStatus;
        private System.Windows.Forms.ComboBox dropdown1;
        private System.Windows.Forms.TrackBar slider1;
        private System.Windows.Forms.Label name1;
        private System.Windows.Forms.Label controllerStatus;
        private System.Windows.Forms.Button rotateButton;
    }
}

