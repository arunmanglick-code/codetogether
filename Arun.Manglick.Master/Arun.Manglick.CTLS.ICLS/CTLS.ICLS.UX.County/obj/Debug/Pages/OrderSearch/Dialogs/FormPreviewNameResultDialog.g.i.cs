﻿#pragma checksum "C:\Arun.Manglick\Arun.Manglick.APRJ\Arun.Manglick.Master\Arun.Manglick.CTLS.ICLS\CTLS.ICLS.UX.County\Pages\OrderSearch\Dialogs\FormPreviewNameResultDialog.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "AE39C8CD342575F84536D57A6116823C"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using CT.SLABB.UX.Controls;
using CTLS.ICLS.UX.Controls;
using CTLS.ICLS.UX.CountySearch;
using CTLS.Shared.UX.Controls;
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


namespace CTLS.ICLS.UX.CountySearch {
    
    
    public partial class FormPreviewNameResultDialog : CTLS.ICLS.UX.Controls.ICLSDialog {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal CTLS.ICLS.UX.CountySearch.CountySearchViewModelConnector countySearchVMC;
        
        internal System.Windows.Controls.Grid grFormPreview;
        
        internal CTLS.ICLS.UX.CountySearch.EmptyStringToVisibility EmptyStringToVisibility;
        
        internal CT.SLABB.UX.Controls.MenuItem btnExport;
        
        internal CT.SLABB.UX.Controls.MenuItem btnPrint;
        
        internal CT.SLABB.UX.Controls.MenuItem btnClose;
        
        internal System.Windows.Controls.TextBlock txbNoRecords;
        
        internal System.Windows.Controls.ScrollViewer scrlNameResult;
        
        internal CTLS.Shared.UX.Controls.GridView grvNameSearchResults;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/CTLS.ICLS.UX.CountySearch;component/Pages/OrderSearch/Dialogs/FormPreviewNameRes" +
                        "ultDialog.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.countySearchVMC = ((CTLS.ICLS.UX.CountySearch.CountySearchViewModelConnector)(this.FindName("countySearchVMC")));
            this.grFormPreview = ((System.Windows.Controls.Grid)(this.FindName("grFormPreview")));
            this.EmptyStringToVisibility = ((CTLS.ICLS.UX.CountySearch.EmptyStringToVisibility)(this.FindName("EmptyStringToVisibility")));
            this.btnExport = ((CT.SLABB.UX.Controls.MenuItem)(this.FindName("btnExport")));
            this.btnPrint = ((CT.SLABB.UX.Controls.MenuItem)(this.FindName("btnPrint")));
            this.btnClose = ((CT.SLABB.UX.Controls.MenuItem)(this.FindName("btnClose")));
            this.txbNoRecords = ((System.Windows.Controls.TextBlock)(this.FindName("txbNoRecords")));
            this.scrlNameResult = ((System.Windows.Controls.ScrollViewer)(this.FindName("scrlNameResult")));
            this.grvNameSearchResults = ((CTLS.Shared.UX.Controls.GridView)(this.FindName("grvNameSearchResults")));
        }
    }
}
