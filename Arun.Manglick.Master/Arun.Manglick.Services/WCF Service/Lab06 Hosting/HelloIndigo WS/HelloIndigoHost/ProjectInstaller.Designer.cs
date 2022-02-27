using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration.Install;
using System.ComponentModel;
using System.ServiceProcess;

namespace HelloIndigoHost
{
    partial class ProjectInstaller
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.HelloIndigoServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.HelloIndigoServiceInstaller = new System.ServiceProcess.ServiceInstaller();

            this.HelloIndigoServiceProcessInstaller.Account = ServiceAccount.LocalSystem;
            this.HelloIndigoServiceProcessInstaller.Password = null;

            this.HelloIndigoServiceInstaller.DisplayName = "HelloIndigo WCF Host";
            this.HelloIndigoServiceInstaller.Description = "WCF service host for the Messaging.MessagingService.";
            this.HelloIndigoServiceInstaller.ServiceName = "HelloIndigo WCFHost WindowsService";
            this.HelloIndigoServiceInstaller.StartType = ServiceStartMode.Automatic;

            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
                                                                                    this.HelloIndigoServiceProcessInstaller,
                                                                                    this.HelloIndigoServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller HelloIndigoServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller HelloIndigoServiceInstaller;
    }
}