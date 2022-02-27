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
using System.ServiceModel;
using System.Net;
using System.Security.Principal;
using System.Threading;
using System.Security;
using HelloSecureClient.HSServiceReferenceInternetA;

namespace HelloSecureClient
{
    public partial class Form1 : Form
    {
        #region Variables & Constructor'

        string m_username;
        string m_password;
        SecureServiceContractClient proxy1;
        SecureServiceContractClient proxy2;

        public Form1()
        {
            Thread.GetDomain().SetPrincipalPolicy(PrincipalPolicy.UnauthenticatedPrincipal);

            InitializeComponent();
            InitializeProxy();
            UpdateIdentities();
        }

        #endregion
        
        #region Methods

        private void InitializeProxy()
        {
            proxy1 = new SecureServiceContractClient("BasicHttpBinding_SecureServiceContract");
            proxy1.ClientCredentials.UserName.UserName = "arun";
            proxy1.ClientCredentials.UserName.Password = "arun123#";

            //proxy2 = new SecureServiceContractClient("WSHttpBinding_SecureServiceContract");
            //proxy2.ClientCredentials.UserName.UserName = "admin";
            //proxy2.ClientCredentials.UserName.Password = "admin123#";

            if (!(String.IsNullOrEmpty(m_username) || String.IsNullOrEmpty(this.m_password)))
            {
                proxy1.ClientCredentials.UserName.UserName = this.m_username;
                proxy1.ClientCredentials.UserName.Password = this.m_password;

                //proxy2.ClientCredentials.UserName.UserName = this.m_username;
                //proxy2.ClientCredentials.UserName.Password = this.m_password;
            }

        }
        private SecureServiceContractClient GetProxy()
        {
            if (radSoap11.Checked)
                return this.proxy1;
            else
                return this.proxy2;
        }
        private void UpdateIdentities()
        {
            IIdentity winIdentity = WindowsIdentity.GetCurrent();
            this.lblWindowsIdentity.Text = String.Format("Name: {0}\r\nIsAuthenticated: {1}\r\nAuthenticationType: {2}", winIdentity.Name, winIdentity.IsAuthenticated, winIdentity.AuthenticationType);

            IPrincipal secPrincipal = Thread.CurrentPrincipal;
            this.lblLoggedInUser.Text = String.Format("Name: {0}\r\nIsAuthenticated: {1}\r\nAuthenticationType: {2}", secPrincipal.Identity.Name, secPrincipal.Identity.IsAuthenticated, secPrincipal.Identity.AuthenticationType); ;

        }

        #endregion

        #region Event Handlers

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void cmdAdminOp_Click(object sender, EventArgs e)
        {
            try
            {
                Util.SetCertificatePolicy();
                MessageBox.Show(GetProxy().AdminOperation());
            }
            catch (FaultException faultEx)
            {
                MessageBox.Show(faultEx.Message);
                InitializeProxy();
            }
            catch (CommunicationException comEx)
            {
                MessageBox.Show(comEx.Message);
                InitializeProxy();
            }
        }
        private void cmdUserOp_Click(object sender, EventArgs e)
        {
            try
            {
                MessageBox.Show(GetProxy().UserOperation());
            }
            catch (CommunicationException comEx)
            {
                MessageBox.Show(comEx.Message);
                InitializeProxy();
            }

        }
        private void cmdGuestOp_Click(object sender, EventArgs e)
        {

            try
            {
                MessageBox.Show(GetProxy().GuestOperation());
            }
            catch (CommunicationException comEx)
            {

                MessageBox.Show(comEx.Message);
                InitializeProxy();

            }

        }
        private void cmdNormalOp_Click(object sender, EventArgs e)
        {
            try
            {
                //MessageBox.Show(GetProxy().NormalOperation());
            }
            catch (CommunicationException comEx)
            {
                MessageBox.Show(comEx.Message);
                InitializeProxy();
            }
        }
        private void mnuLogin_Click(object sender, EventArgs e)
        {

            //SecurityUtility.LoginForm f = new SecurityUtility.LoginForm();
            //f.ShowDomain = false;

            //DialogResult res = f.ShowDialog();
            //if (res == DialogResult.OK)
            //{

            //    try
            //    {
            //        this.m_username = f.Username;
            //        this.m_password = f.Password;

            //        InitializeProxy();
            //        UpdateIdentities();

            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show(ex.Message);
            //    }

            //}

        }

        #endregion

    }
}