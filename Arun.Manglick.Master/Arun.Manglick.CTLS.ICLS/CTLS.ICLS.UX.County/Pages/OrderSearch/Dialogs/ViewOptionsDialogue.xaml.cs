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

namespace CTLS.ICLS.UX.CountySearch
{
    public partial class ViewOptionsDialogue : ChildWindow
    {
        #region .ctor

        /// <summary>
        /// Constructor
        /// </summary>
        public ViewOptionsDialogue()
        {
            InitializeComponent();
        }
        #endregion

        #region Event Handlers

        /// <summary>
        /// click event of OK button
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">MenuItemClickedEventArgs</param>
        /// <returns>void</returns>
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        /// <summary>
        /// click event of Cancel button
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">MenuItemClickedEventArgs</param>
        /// <returns>void</returns>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        /// <summary>
        /// Loaded event of countySearchVMC CountySearchViewModelConnector
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">RoutedEventArgs</param>
        /// <returns>void</returns>
        private void countySearchVMC_Loaded(object sender, RoutedEventArgs e)
        {
            pnlViewOptions.DataContext = null;
            pnlViewOptions.DataContext = this.countySearchVMC.ViewState.ViewOptionsBindableModel;
        }

        #endregion
    }
}

