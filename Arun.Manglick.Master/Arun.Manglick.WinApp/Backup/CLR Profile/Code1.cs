using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace WindowsApplication1.CLR_Profile
{
    class Code1
    {
        public static void ProfileStringCode()
        {
            int start = Environment.TickCount;
            for (int i = 0; i < 1000; i++)
            {
                string s = "";
                for (int j = 0; j < 100; j++)
                {
                    s += "Outer index = ";
                    s += i;
                    s += " Inner index = ";
                    s += j;
                    s += " ";
                }
            }

            double totalSeconds = 0.001 * (Environment.TickCount - start);
            MessageBox.Show("Program ran for {0} seconds: " + totalSeconds.ToString());
        }
    }
}
