using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Reflection;
using Arun.Manglick.Silverlight.BO;

namespace Arun.Manglick.Silverlight
{
    public partial class App : Application
    {
        State objState= new State();
        

        public State State
        {
            get { return objState; }
            set { objState = value; }
        }

        public App()
        {
            this.Startup += this.Application_Startup;
            this.UnhandledException += this.Application_UnhandledException;
            objState.Name = "Startup";

            InitializeComponent();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //<param name="initParams" value="startPage=Page1,viewMode=Customer" />

            UserControl startPage = null;
            if (e.InitParams.ContainsKey("startPage"))
            {
                string startPageName = e.InitParams["startPage"];
                try
                {
                    // Create an instance of the page.
                    Type type = this.GetType();
                    Assembly assembly = type.Assembly;
                    startPage = (UserControl)assembly.CreateInstance(type.Namespace + "." + startPageName);
                }
                catch
                {
                    startPage = null;
                }
            }
            if (startPage == null) startPage = new MainPage();
                      

            this.RootVisual = startPage;
        }

        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            // If the app is running outside of the debugger then report the exception using a ChildWindow control.
            if (!System.Diagnostics.Debugger.IsAttached)
            {
                // NOTE: This will allow the application to continue running after an exception has been thrown but not handled. 
                e.Handled = true;
                ChildWindow errorWin = new ErrorWindow(e.ExceptionObject);
                errorWin.Show();
            }
        }
    }
}