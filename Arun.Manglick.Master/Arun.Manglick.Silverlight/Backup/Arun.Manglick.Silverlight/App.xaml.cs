﻿using System;
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

namespace Arun.Manglick.Silverlight
{
    public partial class App : Application
    {
        private Grid myRoot = new Grid();
        public App()
        {
            this.Startup += this.Application_Startup;
            this.Exit += this.Application_Exit;
            this.UnhandledException += this.Application_UnhandledException;

            InitializeComponent();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //this.RootVisual = new Page();
            //this.RootVisual = new CanvasPage();
            //this.RootVisual = new DragAndDrop();
            //this.RootVisual = new GridPage();
            //this.RootVisual = new StackPanel();
            //this.RootVisual = new StackPanelHorizontal();
            //this.RootVisual = new Library();
            //this.RootVisual = new LibraryStyle();
            //this.RootVisual = new BallPlay();
            //this.RootVisual = new DotPlay();
            //this.RootVisual = new DotStarPlay();
            //this.RootVisual = new DragFlowerPlay();

            this.RootVisual = myRoot;
            myRoot.Children.Add(new MenuPage());
        }
        private void Application_Exit(object sender, EventArgs e)
        {

        }
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            // If the app is running outside of the debugger then report the exception using
            // the browser's exception mechanism. On IE this will display it a yellow alert 
            // icon in the status bar and Firefox will display a script error.
            if (!System.Diagnostics.Debugger.IsAttached)
            {
                // NOTE: This will allow the application to continue running after an exception has been thrown but not handled. 
                // For production applications this error handling should be replaced with something that will 
                // report the error to the website and stop the application.
                e.Handled = true;
                Deployment.Current.Dispatcher.BeginInvoke(delegate { ReportErrorToDOM(e); });
            }
        }
        private void ReportErrorToDOM(ApplicationUnhandledExceptionEventArgs e)
        {
            try
            {
                string errorMsg = e.ExceptionObject.Message + e.ExceptionObject.StackTrace;
                errorMsg = errorMsg.Replace('"', '\'').Replace("\r\n", @"\n");

                System.Windows.Browser.HtmlPage.Window.Eval("throw new Error(\"Unhandled Error in Silverlight 2 Application " + errorMsg + "\");");
            }
            catch (Exception)
            {
            }
        }

        public static void Navigate(UserControl newPage)
        {
            App currentApp = Application.Current as App;
            currentApp.myRoot.Children.Clear();
            currentApp.myRoot.Children.Add(newPage);
        }
    }
}