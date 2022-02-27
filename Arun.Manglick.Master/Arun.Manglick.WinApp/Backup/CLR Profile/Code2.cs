using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsApplication1.CLR_Profile
{
    class Code2
    {
        public static void ProfileBrushCode()
        {
            int start = Environment.TickCount;
            for (int i = 0; i < 100 * 1000; i++)
            {
                Brush b = new SolidBrush(Color.Black); // Brush has a finalizer
                string s = new string(' ', i % 37);
            }

            double totalSeconds = 0.001 * (Environment.TickCount - start);
            MessageBox.Show("Program ran for {0} seconds: " + totalSeconds.ToString());
        }
    }
}
