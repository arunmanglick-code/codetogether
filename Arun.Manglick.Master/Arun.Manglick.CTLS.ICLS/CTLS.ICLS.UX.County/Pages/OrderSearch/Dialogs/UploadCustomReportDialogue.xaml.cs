using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using CT.SLABB.DX;
using CTLS.ICLS.DX;
using CTLS.ICLS.UX.Controls;
using CTLS.ICLS.UX.Shared.Utils; 

namespace CTLS.ICLS.UX.CountySearch
{
    public partial class UploadCustomReportDialogue : ICLSDialog
    {
        #region Variables

        private string ERROR_MESSAGE = "Error reading file.";
        private string LOADING_MESSAGE = "Loading your document. Please wait...";
        private string FILESIZE_MESSAGE = "Files must be less than 3 MB.";
        private bool __uploadResult;
        public event RoutedEventHandler UCRServiceCallCompleted;

        #endregion

        #region .ctor
        /// <summary>
        /// Constructor
        /// </summary>
        public UploadCustomReportDialogue()
        {
            InitializeComponent();
            UCRServiceCallCompleted += UploadCustomReportDialogue_UCRServiceCallCompleted;
        }        
        #endregion

        #region Properties
        /// <summary>
        /// Get OR set Tracking Number
        /// </summary>
        public long TrackingNo { get; set; }
        #endregion

        #region Event Handlers

        /// <summary>
        /// Click event of OK button
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">RoutedEventArgs</param>
        /// <returns>void</returns>
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        /// <summary>
        /// Click event of Cancel Button
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">RoutedEventArgs</param>
        /// <returns>void</returns>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        /// <summary>
        /// Click event of Cancel Browse
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">RoutedEventArgs</param>
        /// <returns>void</returns>
        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "Doc Files (*.doc)|*.doc";
            txbLoaderMsg.Text = String.Empty;
            txbLoaderMsg.Visibility = Visibility.Collapsed;

            if (openDialog.ShowDialog() == true)
            {
                try
                {
                    using (Stream stream = openDialog.File.OpenRead())
                    {
                        if (stream.Length > 3000000)
                        {
                            MessageBox.Show(FILESIZE_MESSAGE);
                        }
                        else
                        {
                            byte[] data = new byte[stream.Length];
                            stream.Read(data, 0, (int)stream.Length);

                            UploadCustomReportRequest uploadCustomReportRequest = new UploadCustomReportRequest();
                            uploadCustomReportRequest.TrackingNo = this.TrackingNo;
                            uploadCustomReportRequest.FileStream = data;
                            UploadCustomReportProxy uploadCustomReportProxy = new UploadCustomReportProxy();
                            uploadCustomReportProxy.Invoke(uploadCustomReportRequest, UploadCustomReportServiceCompleted);                                                       
                            
                            txbLoaderMsg.Text = LOADING_MESSAGE;
                            txbLoaderMsg.Visibility = Visibility.Visible;
                        }
                    }
                }
                catch
                {
                    txbLoaderMsg.Text = ERROR_MESSAGE;
                    txbLoaderMsg.Visibility = Visibility.Visible;
                }
            }    
        }

        /// <summary>
        /// Asynchronus UploadCustomReportSVC service call completed event.
        /// </summary>
        /// <param name="sender">DxProxy</param>
        /// <param name="args">DxCompleteEventArgs<UploadCustomReportResponse></param>
        /// <returns>void</returns>
        private void UploadCustomReportServiceCompleted(DxProxy sender, DxCompleteEventArgs<UploadCustomReportResponse> args)
        {
            if (args.Error != null)
            {
                ErrorHandler.ExceptionOccured(this.GetType().FullName, MethodInfo.GetCurrentMethod().Name, this.GetType(), args.Error);
                return;
            }

            if (args.Response == null)
            {
                throw new Exception(string.Format(MessageConstants.COMMON_EXCEPTION_ERROR_NULL_RESPONSE, "UploadCustomReportSVC"));
            }

            __uploadResult = args.Response.IsFileUploaded;
            if (UCRServiceCallCompleted != null)
                UCRServiceCallCompleted(this, new RoutedEventArgs());
            
        }

        /// <summary>
        /// UCRServiceCallCompleted event of UploadCustomReportDialogue
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">RoutedEventArgs</param>
        /// <returns>void</returns>
        private void UploadCustomReportDialogue_UCRServiceCallCompleted(object sender, RoutedEventArgs e)
        {
            if (__uploadResult) txbLoaderMsg.Text = "Loading Completed.";
            else txbLoaderMsg.Text = "Error reading file.";

            txbLoaderMsg.Visibility = Visibility.Visible;
        }

        #endregion
    }
}

