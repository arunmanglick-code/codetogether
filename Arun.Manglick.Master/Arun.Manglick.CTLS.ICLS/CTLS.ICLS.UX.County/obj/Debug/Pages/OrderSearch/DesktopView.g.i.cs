﻿#pragma checksum "C:\Arun.Manglick\Arun.Manglick.APRJ\Arun.Manglick.Master\Arun.Manglick.CTLS.ICLS\CTLS.ICLS.UX.County\Pages\OrderSearch\DesktopView.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "7EB9C8DCEF29FA16C0DCFF7069A5A18F"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using CT.SLABB.Data;
using CTLS.ICLS.UX.CountySearch;
using CTLS.ICLS.UX.Shared.ViewStates;
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
    
    
    public partial class DesktopView : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal CT.SLABB.Data.DataSource dsOrderItemListData;
        
        internal CTLS.ICLS.UX.CountySearch.OrderSearchDataProvider dpOrderItemList;
        
        internal CTLS.ICLS.UX.CountySearch.CountySearchViewModelConnector countySearchVMC;
        
        internal CTLS.ICLS.UX.Shared.ViewStates.ApplicationContextViewStateConnector ApplicationContext;
        
        internal System.Windows.Controls.ValidationSummary vlsValidationSummary;
        
        internal DataForm dfCountySearch;
        
        internal CT.SLABB.Data.RefreshIndicator refreshIndicatorSearchInstructions;
        
        internal System.Windows.Controls.ListBox lstInstructions;
        
        internal CT.SLABB.Data.RefreshIndicator refreshIndicatorCopyCostInstrunction;
        
        internal System.Windows.Controls.ListBox lstCopyCost;
        
        internal CT.SLABB.Data.RefreshIndicator refreshIndicatorAlertList;
        
        internal System.Windows.Controls.ListBox lstAlerts;
        
        internal CT.SLABB.Data.RefreshManager prefetchRefreshManager;
        
        internal CTLS.ICLS.UX.CountySearch.CountySearchInstructionsList CountySearchInstructionsList;
        
        internal CTLS.ICLS.UX.CountySearch.CopyCostInstrunctionList CopyCostInstrunctionList;
        
        internal CTLS.ICLS.UX.CountySearch.AlertList AlertList;
        
        internal System.Windows.Controls.HyperlinkButton lnkRefresh;
        
        internal CT.SLABB.Data.RefreshIndicator refreshIndicator;
        
        internal CTLS.Shared.UX.Controls.ListPager pgrTop;
        
        internal CTLS.Shared.UX.Controls.GridView gvwAcceptedOrders;
        
        internal CTLS.Shared.UX.Controls.ListPager pgrBottom;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/CTLS.ICLS.UX.CountySearch;component/Pages/OrderSearch/DesktopView.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.dsOrderItemListData = ((CT.SLABB.Data.DataSource)(this.FindName("dsOrderItemListData")));
            this.dpOrderItemList = ((CTLS.ICLS.UX.CountySearch.OrderSearchDataProvider)(this.FindName("dpOrderItemList")));
            this.countySearchVMC = ((CTLS.ICLS.UX.CountySearch.CountySearchViewModelConnector)(this.FindName("countySearchVMC")));
            this.ApplicationContext = ((CTLS.ICLS.UX.Shared.ViewStates.ApplicationContextViewStateConnector)(this.FindName("ApplicationContext")));
            this.vlsValidationSummary = ((System.Windows.Controls.ValidationSummary)(this.FindName("vlsValidationSummary")));
            this.dfCountySearch = ((DataForm)(this.FindName("dfCountySearch")));
            this.refreshIndicatorSearchInstructions = ((CT.SLABB.Data.RefreshIndicator)(this.FindName("refreshIndicatorSearchInstructions")));
            this.lstInstructions = ((System.Windows.Controls.ListBox)(this.FindName("lstInstructions")));
            this.refreshIndicatorCopyCostInstrunction = ((CT.SLABB.Data.RefreshIndicator)(this.FindName("refreshIndicatorCopyCostInstrunction")));
            this.lstCopyCost = ((System.Windows.Controls.ListBox)(this.FindName("lstCopyCost")));
            this.refreshIndicatorAlertList = ((CT.SLABB.Data.RefreshIndicator)(this.FindName("refreshIndicatorAlertList")));
            this.lstAlerts = ((System.Windows.Controls.ListBox)(this.FindName("lstAlerts")));
            this.prefetchRefreshManager = ((CT.SLABB.Data.RefreshManager)(this.FindName("prefetchRefreshManager")));
            this.CountySearchInstructionsList = ((CTLS.ICLS.UX.CountySearch.CountySearchInstructionsList)(this.FindName("CountySearchInstructionsList")));
            this.CopyCostInstrunctionList = ((CTLS.ICLS.UX.CountySearch.CopyCostInstrunctionList)(this.FindName("CopyCostInstrunctionList")));
            this.AlertList = ((CTLS.ICLS.UX.CountySearch.AlertList)(this.FindName("AlertList")));
            this.lnkRefresh = ((System.Windows.Controls.HyperlinkButton)(this.FindName("lnkRefresh")));
            this.refreshIndicator = ((CT.SLABB.Data.RefreshIndicator)(this.FindName("refreshIndicator")));
            this.pgrTop = ((CTLS.Shared.UX.Controls.ListPager)(this.FindName("pgrTop")));
            this.gvwAcceptedOrders = ((CTLS.Shared.UX.Controls.GridView)(this.FindName("gvwAcceptedOrders")));
            this.pgrBottom = ((CTLS.Shared.UX.Controls.ListPager)(this.FindName("pgrBottom")));
        }
    }
}
