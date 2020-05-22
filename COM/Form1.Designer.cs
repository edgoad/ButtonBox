namespace COM
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbPort = new System.Windows.Forms.ComboBox();
            this.cmdOpen = new System.Windows.Forms.Button();
            this.cmdClose = new System.Windows.Forms.Button();
            this.txtSerial = new System.Windows.Forms.TextBox();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.cmdReceive = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Port";
            // 
            // cmbPort
            // 
            this.cmbPort.FormattingEnabled = true;
            this.cmbPort.Location = new System.Drawing.Point(62, 28);
            this.cmbPort.Name = "cmbPort";
            this.cmbPort.Size = new System.Drawing.Size(121, 21);
            this.cmbPort.TabIndex = 1;
            // 
            // cmdOpen
            // 
            this.cmdOpen.Location = new System.Drawing.Point(205, 28);
            this.cmdOpen.Name = "cmdOpen";
            this.cmdOpen.Size = new System.Drawing.Size(75, 23);
            this.cmdOpen.TabIndex = 2;
            this.cmdOpen.Text = "Open";
            this.cmdOpen.UseVisualStyleBackColor = true;
            this.cmdOpen.Click += new System.EventHandler(this.cmdOpen_Click);
            // 
            // cmdClose
            // 
            this.cmdClose.Location = new System.Drawing.Point(303, 28);
            this.cmdClose.Name = "cmdClose";
            this.cmdClose.Size = new System.Drawing.Size(75, 23);
            this.cmdClose.TabIndex = 3;
            this.cmdClose.Text = "Close";
            this.cmdClose.UseVisualStyleBackColor = true;
            this.cmdClose.Click += new System.EventHandler(this.cmdClose_Click);
            // 
            // txtSerial
            // 
            this.txtSerial.Location = new System.Drawing.Point(62, 72);
            this.txtSerial.Multiline = true;
            this.txtSerial.Name = "txtSerial";
            this.txtSerial.Size = new System.Drawing.Size(316, 168);
            this.txtSerial.TabIndex = 4;
            // 
            // cmdReceive
            // 
            this.cmdReceive.Location = new System.Drawing.Point(205, 247);
            this.cmdReceive.Name = "cmdReceive";
            this.cmdReceive.Size = new System.Drawing.Size(75, 23);
            this.cmdReceive.TabIndex = 5;
            this.cmdReceive.Text = "Receive";
            this.cmdReceive.UseVisualStyleBackColor = true;
            this.cmdReceive.Click += new System.EventHandler(this.cmdReceive_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(424, 297);
            this.Controls.Add(this.cmdReceive);
            this.Controls.Add(this.txtSerial);
            this.Controls.Add(this.cmdClose);
            this.Controls.Add(this.cmdOpen);
            this.Controls.Add(this.cmbPort);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbPort;
        private System.Windows.Forms.Button cmdOpen;
        private System.Windows.Forms.Button cmdClose;
        private System.Windows.Forms.TextBox txtSerial;
        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.Button cmdReceive;
    }
}

