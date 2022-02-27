using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ServiceModel;
using HelloIndigo.Lab6.WIN;

namespace HelloIndigoHost
{
    public partial class Form1 : Form
    {
        ServiceHost selfHost = null;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {           
            try
            {
                selfHost = new ServiceHost(typeof(HelloIndigo.Lab6.WIN.HelloIndigoService));
                selfHost.Open();
                textBox1.Text = "Service is Ready & Running";                            
            }
            catch (CommunicationException ex)
            {
                Console.WriteLine("An exception occurred: {0}", ex.Message);
                selfHost.Abort();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to close the service?", "Service Controller", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

            if (result == DialogResult.Yes)
            {
                if (selfHost != null)
                {
                    selfHost.Close();
                    selfHost = null;
                    textBox1.Text = "Service is Stopped"; 
                }
            }
            else
                e.Cancel = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to close the service?", "Service Controller", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

            if (result == DialogResult.Yes)
            {
                if (selfHost != null)
                {
                    selfHost.Close();
                    selfHost = null;
                    textBox1.Text = "Service is Stopped"; 
                }
            }           
        }

       
    }
}
