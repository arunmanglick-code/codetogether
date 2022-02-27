using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace WindowsApplication1.Equality_Myth
{
    class StringTypes
    {
        private string i = "Str1", j = "Str1";
        bool res;

        public void FindEquality()
        {
            res = i == j;   				// True

            if (res)
            {
                MessageBox.Show("Equal");
            }
            else
            {
                MessageBox.Show("Un Equal");
            }
        }
        public void FindReferenceEquality()
        {
            res = Object.ReferenceEquals(i, j); 		    // False            

            if (res)
            {
                MessageBox.Show("Equal"  + "\n\n" + "Different behavoiur than ValueTypes");
            }
            else
            {
                MessageBox.Show("Un Equal");
            }
        }
        public void FindVirtualEqualsEquality()
        {
            res = i.Equals(j);		    // True            

            if (res)
            {
                MessageBox.Show("Equal");
            }
            else
            {
                MessageBox.Show("Un Equal");
            }
        }
        public void FindStaticEqualsEquality()
        {
            res = Object.ReferenceEquals(i, j); 		    // True            

            if (res)
            {
                MessageBox.Show("Equal");
            }
            else
            {
                MessageBox.Show("Un Equal");
            }
        }
    }
}
