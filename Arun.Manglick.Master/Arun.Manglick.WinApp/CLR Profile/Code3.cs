using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace WindowsApplication1.CLR_Profile
{
    class Code3
    {
        public static void ProfileStreamCode()
        {
            int start = Environment.TickCount;

            StreamReader r = new StreamReader(@"D:\Arun.Manglick.ORG\PSPL\Project Apps Trial\RndDWindows\RndDWindows\RndWindowsSimple\WindowsApplication1\WindowsApplication1\CLR Profile\Demo.dat");
            string line;
            int lineCount = 0;
            int itemCount = 0;
            while ((line = r.ReadLine()) != null)
            {
                lineCount++;
                string[] items = line.Split();
                for (int i = 0; i < items.Length; i++)
                {
                    itemCount++;
                }
            }
            r.Close();


            double totalSeconds = 0.001 * (Environment.TickCount - start);
            MessageBox.Show("Program ran for {0} seconds: " + totalSeconds.ToString());
        }
    }
}
