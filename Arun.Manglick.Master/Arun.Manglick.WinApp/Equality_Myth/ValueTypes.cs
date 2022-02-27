using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace WindowsApplication1.Equality_Myth
{
    class ValueTypes
    {
        private int i=1, j=1;
        bool res;

        public void FindEquality()
        {
            res= i == j;   				// True

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
            res = Object.ReferenceEquals(i,j); 		    // False            

            if (res)
            {
                MessageBox.Show("Equal");
            }
            else
            {
                MessageBox.Show("Un Equal" + "\n\n" + "Will always return False");
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
            res = Object.Equals(i, j); 		    // True            

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
