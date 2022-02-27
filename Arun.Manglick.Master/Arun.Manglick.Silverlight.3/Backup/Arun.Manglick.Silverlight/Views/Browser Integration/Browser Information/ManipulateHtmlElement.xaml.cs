using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Browser;

namespace Arun.Manglick.Silverlight.Views.BrowserIntegration.BrowserInformation
{
    public partial class ManipulateHtmlElement : UserControl
    {
        public ManipulateHtmlElement()
        {
            InitializeComponent();
        }

        #region Event Handlers

        private void cmdCreate_Click(object sender, RoutedEventArgs e)
        {
            HtmlElement element = HtmlPage.Document.CreateElement("p");
            
            element.Id = "paragraph";
            element.SetProperty("innerHTML","This is a new element. Click to change its background color.");                        
            HtmlPage.Document.Body.AppendChild(element);
            //HtmlPage.Document.Body.AppendChild(element, HtmlPage.Document.Body.Children[0]);
            //element.CssClass = "highlightedParagraph";            

            element.SetStyleAttribute("border", "solid 1px black");
            element.SetStyleAttribute("margin", "10px");
            element.SetStyleAttribute("padding", "10px");
                       
            element.AttachEvent("onclick", paragraph_Click);
        }

        private void cmdChange_Click(object sender, RoutedEventArgs e)
        {
            HtmlElement element = HtmlPage.Document.GetElementById("paragraph");
            if (element == null) return;

            element.SetProperty("innerHTML","This HTML paragraph has been updated by Silverlight.");
            element.SetStyleAttribute("background", "white");
        }

        private void paragraph_Click(object sender, HtmlEventArgs e)
        {
            HtmlElement element = (HtmlElement)sender;            
            element.SetProperty("innerHTML", "You clicked this HTML element, and Silverlight handled it.");
            element.SetStyleAttribute("background", "#00ff00");
        }

        private void cmdRemove_Click(object sender, RoutedEventArgs e)
        {
            HtmlElement element = HtmlPage.Document.GetElementById("paragraph");
            if (element == null) return;
            
            element.Parent.RemoveChild(element);
        }
        
        private void cmdScript_Click(object sender, RoutedEventArgs e)
        {
            HtmlElement element = HtmlPage.Document.GetElementById("paragraph");
            if (element == null) return;

            ScriptObject script = (ScriptObject)HtmlPage.Window.GetProperty("changeParagraph");
            script.InvokeSelf("Changed through JavaScript.");
        }
        
        private void cmdPopupWindow_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (HtmlPage.IsPopupWindowAllowed)
                {
                    // Configure the popup window options.
                    HtmlPopupWindowOptions options = new HtmlPopupWindowOptions();
                    options.Resizeable = true;
                    options.Height = 300;
                    options.Width = 500;
                    

                    //string hostUri = System.Windows.Browser.HtmlPage;
                    //HtmlPage.Navigate(new Uri(HtmlPage.DocumentUri, "relativepath.aspx").ToString());
                    // You pass in an absolute URI, an optional target frame, and the HtmlPopupWindowOptions.
                    //HtmlPage.PopupWindow(new Uri("http://localhost/BigIP.html"),null, options);
                    HtmlPage.PopupWindow(new Uri("http://in-svr-458:9595/DocumentDownloads/658_ebd386842d1044a48fa0b88f91ae50af_Combined.pdf"), null, null);
                    //
                     //HtmlPage.Window.Navigate(new Uri("BigIP.html", UriKind.RelativeOrAbsolute), "_self");
                    string str = "hello";
                }
            }
            catch (Exception ex)
            {                
                throw;
            }
        }

        #endregion
    }
}
