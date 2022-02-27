namespace Arun.Manglick.EventLogger
{
    partial class EventLogger
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Timers.Timer timer1;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.timer1 = new System.Timers.Timer();
            this.timer1.Enabled = true;
            this.timer1.Interval = 30000;
            this.timer1.Elapsed += new System.Timers.ElapsedEventHandler(timer1_Elapsed);
            this.ServiceName = "EventLoggerService";
        }

        #endregion
    }
}
