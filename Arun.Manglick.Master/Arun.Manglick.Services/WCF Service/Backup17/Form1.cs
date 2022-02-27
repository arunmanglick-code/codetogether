using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ServiceModel;

using HelloIndigoWinClient.HIServiceReferenceUnCaughtWin;

namespace HelloIndigoWinClient
{
    public partial class Form1 : Form
    {
        HelloIndigoServiceClient proxy = new HelloIndigoServiceClient();

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string res = proxy.GoodOperation();
            MessageBox.Show(res, "GoodOperation() Called", MessageBoxButtons.OK);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string res = string.Empty;
            try
            {
                proxy.ThrowException();
                res = "SUCCESS: proxy.ThrowException";
                MessageBox.Show(res, "ThrowException() Called", MessageBoxButtons.OK);

            }
            catch (CommunicationException communicationException)
            {
                res += communicationException.GetType().ToString();
                res += Environment.NewLine;
                res += "Communication ERROR: {0}" + communicationException.Message;
                MessageBox.Show(res, "ThrowException() Called", MessageBoxButtons.OK);
            }
            catch (Exception ex)
            {
                res += ex.GetType().ToString();
                res += "ERROR: {0}" + ex.Message;
                MessageBox.Show(res, "ThrowException() Called", MessageBoxButtons.OK);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string res = string.Empty;
            try
            {
                proxy.ThrowExceptionOneWay();
                res = "SUCCESS: proxy.ThrowExceptionOneWay";
                res += Environment.NewLine;
                res += "Here No Exception is caught - Reason One-Way Call";
                MessageBox.Show(res, "ThrowExceptionOneWay() Called", MessageBoxButtons.OK);
            }
            catch (CommunicationException communicationException)
            {
                res += communicationException.GetType().ToString();
                res += Environment.NewLine;
                res += "Communication ERROR: {0}" + communicationException.Message;
                MessageBox.Show(res, "ThrowExceptionOneWay() Called", MessageBoxButtons.OK);
            }
            catch (Exception ex)
            {
                res += ex.GetType().ToString();
                res += "ERROR: {0}" + ex.Message;
                MessageBox.Show(res, "ThrowExceptionOneWay() Called", MessageBoxButtons.OK);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
    }
}
