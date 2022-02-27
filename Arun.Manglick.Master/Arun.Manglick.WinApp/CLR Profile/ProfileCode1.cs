using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace WindowsApplication1.CLR_Profile
{
    class ProfileCode1
    {
        public static void ProfileStringCode()
        {
            int start = Environment.TickCount;
            for (int i = 0; i < 1000; i++)
            {
                StringBuilder sb = new StringBuilder();
                for (int j = 0; j < 100; j++)
                {
                    sb.Append("Outer index = ");
                    sb.Append(i);
                    sb.Append(" Inner index = ");
                    sb.Append(j);
                    sb.Append(" ");
                }
                string s = sb.ToString();

            }

            double totalSeconds = 0.001 * (Environment.TickCount - start);
            MessageBox.Show("Program ran for {0} seconds: " + totalSeconds.ToString());
        }
    }
}
