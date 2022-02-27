using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace WindowsApplication1.Swap_Myth
{
    class Swap
    {
        public static void SwapMe(Employee a, Employee b)
        {
            Employee temp;
            temp = a;
            a = b;
            b = temp;
        }

        public static void SwapMeByRef(ref Employee a,ref Employee b)
        {
            Employee temp;
            temp = a;
            a = b;
            b = temp;
        }

        public static void SwapMe(ref string a, ref string b)
        {
            string temp;
            temp = a;
            a = b;
            b = temp;
        }
        public static void ChangeMe(Employee a, Employee b)
        {
            a.i = 55;
            b.i = 66;
        }

        public static void AddMore(ArrayList arr)
        {
            arr.Add(new WindowsApplication1.Swap_Myth.Employee(9));
        }

    }
}
