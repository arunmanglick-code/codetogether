using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using HelloIndigoClient.HIServiceReferenceKT;


namespace HelloIndigoClient
{
    public partial class ClientForm : Form
    {
        HelloIndigoServiceClient proxy = new HelloIndigoServiceClient();

        /// <summary>
        /// 
        /// </summary>
        public ClientForm()
        {
            InitializeComponent();
        }        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdSave_Click(object sender, EventArgs e)
        {
            GigInfo item = new GigInfo();

            item.Id = int.Parse(this.txtId.Text);
            item.Title = this.txtTitle.Text;
            item.Description = this.txtDescription.Text;
            item.DateStart = this.dtpStart.Value;
            item.DateEnd = this.dtpEnd.Value;
            item.Url = this.txtUrl.Text;

            proxy.SaveGig(item);
            ClearFields();
            GetGiG();           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdGet_Click(object sender, EventArgs e)
        {
            GigInfo item = proxy.GetGig() as GigInfo;
            if (item != null)
            {
                this.txtId.Text = item.Id.ToString();
                this.txtTitle.Text = item.Title;
                this.txtDescription.Text = item.Description;

                if (item.DateStart != DateTime.MinValue)
                    this.dtpStart.Value = item.DateStart;
                if (item.DateEnd != DateTime.MinValue)
                    this.dtpEnd.Value = item.DateEnd;

                this.txtUrl.Text = item.Url;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        private void ClearFields()
        {
            txtId.Text = String.Empty;
            txtTitle.Text = String.Empty;
            txtDescription.Text = String.Empty;
            this.dtpStart.Value = DateTime.Now;
            this.dtpEnd.Value = DateTime.Now;
            txtUrl.Text = String.Empty;
            
        }

        /// <summary>
        /// 
        /// </summary>
        private void GetGiG()
        {
            GigInfo item = proxy.GetGig() as GigInfo;
            if (item != null)
            {
                this.txtId.Text = item.Id.ToString();
                this.txtTitle.Text = item.Title;
                this.txtDescription.Text = item.Description;

                if (item.DateStart != DateTime.MinValue)
                    this.dtpStart.Value = item.DateStart;
                if (item.DateEnd != DateTime.MinValue)
                    this.dtpEnd.Value = item.DateEnd;

                this.txtUrl.Text = item.Url;
            }
        }
    }
}

