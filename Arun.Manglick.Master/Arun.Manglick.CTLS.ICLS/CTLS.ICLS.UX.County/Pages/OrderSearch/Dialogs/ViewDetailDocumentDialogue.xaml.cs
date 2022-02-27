using System.Windows;
using CT.SLABB.UX.Controls;
using CTLS.ICLS.UX.Controls;

namespace CTLS.ICLS.UX.CountySearch
{
    public partial class ViewDetailDocumentDialogue : ICLSDialog
    {
        #region Variables
        private string __imagePath;
        #endregion

        #region .ctor
        /// <summary>
        /// Constructor
        /// </summary>
        public ViewDetailDocumentDialogue()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(ViewDetailDocument_Loaded);
        }
        #endregion       

        #region Properties

        /// <summary>
        /// Get OR Set the image path
        /// </summary>
        public string ImagePath
        {
            get;
            set;
        }
        #endregion

        #region Event Handlers

        /// <summary>
        /// Click event of Close button
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">MenuItemClickedEventArgs</param>
        /// <returns>void</returns>
        private void btnClose_Click(object sender, MenuItemClickedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Loaded event of ViewDetailDocumentDialogue
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">RoutedEventArgs</param>
        /// <returns>void</returns>
        private void ViewDetailDocument_Loaded(object sender, RoutedEventArgs e)
        {
            txbPathTest.Text = this.ImagePath;
        }

        #endregion
    }
}
