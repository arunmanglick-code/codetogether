#pragma checksum "D:\Arun.Manglick.ORG\PSPL\Project Apps Trial\Arun.Manglick.Silverlight\Arun.Manglick.Silverlight\BooksCollection\MyBooksCollection.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "8D2F33EB233ACB628D5C7B4A6F7FF7DC"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.3082
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace Arun.Manglick.Silverlight {
    
    
    public partial class MyBooksCollection : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.ListBox MyBooks;
        
        internal System.Windows.Controls.StackPanel BookDetails;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/Arun.Manglick.Silverlight;component/BooksCollection/MyBooksCollection.xaml", System.UriKind.Relative));
            this.MyBooks = ((System.Windows.Controls.ListBox)(this.FindName("MyBooks")));
            this.BookDetails = ((System.Windows.Controls.StackPanel)(this.FindName("BookDetails")));
        }
    }
}