#pragma checksum "D:\Arun.Manglick.ORG\PSPL\Project Apps Trial\Arun.Manglick.Silverlight\Arun.Manglick.Silverlight\Tutorial 7\DataBindingNTemplate.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "D55B78B274386C4F646A923BD08B1D7E"
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


namespace Arun.Manglick.Silverlight.Tutorial7 {
    
    
    public partial class DataBindingNTemplate : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.TextBlock Title;
        
        internal System.Windows.Controls.ListBox LibraryList;
        
        internal System.Windows.Controls.Grid DetailsGrid;
        
        internal System.Windows.Controls.TextBlock NumAuthorsPrompt;
        
        internal System.Windows.Controls.TextBlock AuthorsPrompt;
        
        internal System.Windows.Controls.ListBox AuthorsListBox;
        
        internal System.Windows.Controls.TextBlock PublisherPrompt;
        
        internal System.Windows.Controls.TextBlock Publisher;
        
        internal System.Windows.Controls.TextBlock EditionPrompt;
        
        internal System.Windows.Controls.TextBlock Edition;
        
        internal System.Windows.Controls.TextBlock PrintingPrompt;
        
        internal System.Windows.Controls.TextBlock Printing;
        
        internal System.Windows.Controls.TextBlock YearPrompt;
        
        internal System.Windows.Controls.TextBlock Year;
        
        internal System.Windows.Controls.TextBlock RatingPrompt;
        
        internal System.Windows.Controls.Slider RatingSlider;
        
        internal System.Windows.Controls.TextBlock SliderValueDisplay;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Arun.Manglick.Silverlight;component/Tutorial%207/DataBindingNTemplate.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.Title = ((System.Windows.Controls.TextBlock)(this.FindName("Title")));
            this.LibraryList = ((System.Windows.Controls.ListBox)(this.FindName("LibraryList")));
            this.DetailsGrid = ((System.Windows.Controls.Grid)(this.FindName("DetailsGrid")));
            this.NumAuthorsPrompt = ((System.Windows.Controls.TextBlock)(this.FindName("NumAuthorsPrompt")));
            this.AuthorsPrompt = ((System.Windows.Controls.TextBlock)(this.FindName("AuthorsPrompt")));
            this.AuthorsListBox = ((System.Windows.Controls.ListBox)(this.FindName("AuthorsListBox")));
            this.PublisherPrompt = ((System.Windows.Controls.TextBlock)(this.FindName("PublisherPrompt")));
            this.Publisher = ((System.Windows.Controls.TextBlock)(this.FindName("Publisher")));
            this.EditionPrompt = ((System.Windows.Controls.TextBlock)(this.FindName("EditionPrompt")));
            this.Edition = ((System.Windows.Controls.TextBlock)(this.FindName("Edition")));
            this.PrintingPrompt = ((System.Windows.Controls.TextBlock)(this.FindName("PrintingPrompt")));
            this.Printing = ((System.Windows.Controls.TextBlock)(this.FindName("Printing")));
            this.YearPrompt = ((System.Windows.Controls.TextBlock)(this.FindName("YearPrompt")));
            this.Year = ((System.Windows.Controls.TextBlock)(this.FindName("Year")));
            this.RatingPrompt = ((System.Windows.Controls.TextBlock)(this.FindName("RatingPrompt")));
            this.RatingSlider = ((System.Windows.Controls.Slider)(this.FindName("RatingSlider")));
            this.SliderValueDisplay = ((System.Windows.Controls.TextBlock)(this.FindName("SliderValueDisplay")));
        }
    }
}
