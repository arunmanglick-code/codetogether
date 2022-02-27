namespace HelloSecureClient
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.cmdAdminOp = new System.Windows.Forms.Button();
            this.cmdUserOp = new System.Windows.Forms.Button();
            this.cmdGuestOp = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.mnuLogin = new System.Windows.Forms.ToolStripMenuItem();
            this.lblLoggedInAs = new System.Windows.Forms.Label();
            this.lblLoggedInUser = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblWindowsIdentity = new System.Windows.Forms.Label();
            this.lblUsernamePassword = new System.Windows.Forms.Label();
            this.lblIdentity = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radSoap12 = new System.Windows.Forms.RadioButton();
            this.radSoap11 = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.cmdNormalOp = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdAdminOp
            // 
            this.cmdAdminOp.Location = new System.Drawing.Point(272, 34);
            this.cmdAdminOp.Name = "cmdAdminOp";
            this.cmdAdminOp.Size = new System.Drawing.Size(149, 31);
            this.cmdAdminOp.TabIndex = 0;
            this.cmdAdminOp.Text = "Admin Operation";
            this.cmdAdminOp.UseVisualStyleBackColor = true;
            this.cmdAdminOp.Click += new System.EventHandler(this.cmdAdminOp_Click);
            // 
            // cmdUserOp
            // 
            this.cmdUserOp.Location = new System.Drawing.Point(272, 76);
            this.cmdUserOp.Name = "cmdUserOp";
            this.cmdUserOp.Size = new System.Drawing.Size(149, 31);
            this.cmdUserOp.TabIndex = 1;
            this.cmdUserOp.Text = "User Operation";
            this.cmdUserOp.UseVisualStyleBackColor = true;
            this.cmdUserOp.Click += new System.EventHandler(this.cmdUserOp_Click);
            // 
            // cmdGuestOp
            // 
            this.cmdGuestOp.Location = new System.Drawing.Point(272, 118);
            this.cmdGuestOp.Name = "cmdGuestOp";
            this.cmdGuestOp.Size = new System.Drawing.Size(149, 31);
            this.cmdGuestOp.TabIndex = 2;
            this.cmdGuestOp.Text = "Guest Operation";
            this.cmdGuestOp.UseVisualStyleBackColor = true;
            this.cmdGuestOp.Click += new System.EventHandler(this.cmdGuestOp_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuLogin});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(499, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // mnuLogin
            // 
            this.mnuLogin.Name = "mnuLogin";
            this.mnuLogin.Size = new System.Drawing.Size(48, 20);
            this.mnuLogin.Text = "Login!";
            this.mnuLogin.Click += new System.EventHandler(this.mnuLogin_Click);
            // 
            // lblLoggedInAs
            // 
            this.lblLoggedInAs.AutoSize = true;
            this.lblLoggedInAs.ForeColor = System.Drawing.Color.Coral;
            this.lblLoggedInAs.Location = new System.Drawing.Point(70, 78);
            this.lblLoggedInAs.Name = "lblLoggedInAs";
            this.lblLoggedInAs.Size = new System.Drawing.Size(90, 13);
            this.lblLoggedInAs.TabIndex = 4;
            this.lblLoggedInAs.Text = "Security principal:";
            // 
            // lblLoggedInUser
            // 
            this.lblLoggedInUser.AutoSize = true;
            this.lblLoggedInUser.Location = new System.Drawing.Point(70, 97);
            this.lblLoggedInUser.Name = "lblLoggedInUser";
            this.lblLoggedInUser.Size = new System.Drawing.Size(94, 13);
            this.lblLoggedInUser.TabIndex = 5;
            this.lblLoggedInUser.Text = "[Security Principal]";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblWindowsIdentity);
            this.groupBox1.Controls.Add(this.lblUsernamePassword);
            this.groupBox1.Controls.Add(this.lblLoggedInUser);
            this.groupBox1.Controls.Add(this.lblIdentity);
            this.groupBox1.Controls.Add(this.lblLoggedInAs);
            this.groupBox1.Controls.Add(this.pictureBox1);
            this.groupBox1.Location = new System.Drawing.Point(16, 33);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(250, 154);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Identities";
            // 
            // lblWindowsIdentity
            // 
            this.lblWindowsIdentity.AutoSize = true;
            this.lblWindowsIdentity.Location = new System.Drawing.Point(70, 34);
            this.lblWindowsIdentity.Name = "lblWindowsIdentity";
            this.lblWindowsIdentity.Size = new System.Drawing.Size(94, 13);
            this.lblWindowsIdentity.TabIndex = 21;
            this.lblWindowsIdentity.Text = "[Windows Identity]";
            // 
            // lblUsernamePassword
            // 
            this.lblUsernamePassword.AutoSize = true;
            this.lblUsernamePassword.Location = new System.Drawing.Point(253, 16);
            this.lblUsernamePassword.Name = "lblUsernamePassword";
            this.lblUsernamePassword.Size = new System.Drawing.Size(0, 13);
            this.lblUsernamePassword.TabIndex = 20;
            // 
            // lblIdentity
            // 
            this.lblIdentity.AutoSize = true;
            this.lblIdentity.ForeColor = System.Drawing.Color.Coral;
            this.lblIdentity.Location = new System.Drawing.Point(70, 15);
            this.lblIdentity.Name = "lblIdentity";
            this.lblIdentity.Size = new System.Drawing.Size(90, 13);
            this.lblIdentity.TabIndex = 18;
            this.lblIdentity.Text = "Windows identity:";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(19, 21);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(29, 36);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 14;
            this.pictureBox1.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radSoap12);
            this.groupBox2.Controls.Add(this.radSoap11);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(16, 224);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(405, 71);
            this.groupBox2.TabIndex = 22;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Protocol";
            // 
            // radSoap12
            // 
            this.radSoap12.AutoSize = true;
            this.radSoap12.Location = new System.Drawing.Point(237, 31);
            this.radSoap12.Name = "radSoap12";
            this.radSoap12.Size = new System.Drawing.Size(136, 17);
            this.radSoap12.TabIndex = 22;
            this.radSoap12.Text = "SOAP 1.2/WS-Security";
            this.radSoap12.UseVisualStyleBackColor = true;
            // 
            // radSoap11
            // 
            this.radSoap11.AutoSize = true;
            this.radSoap11.Checked = true;
            this.radSoap11.Location = new System.Drawing.Point(50, 31);
            this.radSoap11.Name = "radSoap11";
            this.radSoap11.Size = new System.Drawing.Size(97, 17);
            this.radSoap11.TabIndex = 21;
            this.radSoap11.TabStop = true;
            this.radSoap11.Text = "SOAP 1.1/SSL";
            this.radSoap11.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(253, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 13);
            this.label2.TabIndex = 20;
            // 
            // cmdNormalOp
            // 
            this.cmdNormalOp.Location = new System.Drawing.Point(272, 155);
            this.cmdNormalOp.Name = "cmdNormalOp";
            this.cmdNormalOp.Size = new System.Drawing.Size(149, 31);
            this.cmdNormalOp.TabIndex = 23;
            this.cmdNormalOp.Text = "Normal Operation";
            this.cmdNormalOp.UseVisualStyleBackColor = true;
            this.cmdNormalOp.Click += new System.EventHandler(this.cmdNormalOp_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(499, 367);
            this.Controls.Add(this.cmdNormalOp);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cmdGuestOp);
            this.Controls.Add(this.cmdUserOp);
            this.Controls.Add(this.cmdAdminOp);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "WinClient";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdAdminOp;
        private System.Windows.Forms.Button cmdUserOp;
        private System.Windows.Forms.Button cmdGuestOp;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnuLogin;
        private System.Windows.Forms.Label lblLoggedInAs;
        private System.Windows.Forms.Label lblLoggedInUser;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblWindowsIdentity;
        private System.Windows.Forms.Label lblUsernamePassword;
        private System.Windows.Forms.Label lblIdentity;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton radSoap12;
        private System.Windows.Forms.RadioButton radSoap11;
        private System.Windows.Forms.Button cmdNormalOp;
    }
}

