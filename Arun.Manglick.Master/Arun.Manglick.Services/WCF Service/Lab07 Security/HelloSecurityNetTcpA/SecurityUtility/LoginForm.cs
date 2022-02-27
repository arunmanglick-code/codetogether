// © 2007 Michele Leroux Bustamante. All rights reserved 
// Book: Learning WCF, O'Reilly
// Book Blog: www.thatindigogirl.com
// Michele's Blog: www.dasblonde.net
// IDesign: www.idesign.net

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Security;


namespace SecurityUtility
{
	public partial class LoginForm : Form
	{
		public LoginForm()
		{
			InitializeComponent();
		}

        private Boolean m_showDomain = true;

        public Boolean ShowDomain
        {
            get { return m_showDomain; }
            set { m_showDomain = value; }
        }

        private string m_domain;

        public string Domain
        {
            get { return m_domain; }
            set { m_domain = value; }
        }

		private string m_username;

		public string Username
		{
			get { return m_username; }
			set { m_username = value; }
		}
		private string m_password;

		public string Password
		{
			get { return m_password; }
			set { m_password = value; }
		}

		private void cmdOk_Click(object sender, EventArgs e)
		{

			this.Username = this.txtUsername.Text;
			this.Password = this.txtPassword.Text;

			this.DialogResult = DialogResult.OK;
            
			this.Hide();

		}

    	private void cmdCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Hide();
		}

        private void LoginForm_Load(object sender, EventArgs e)
        {
            if (!this.m_showDomain)
            {
                this.lblDomain.Hide();
                this.txtDomain.Hide();
            }

        }
	}
}