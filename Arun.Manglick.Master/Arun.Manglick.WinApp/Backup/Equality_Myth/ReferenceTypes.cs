using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace WindowsApplication1.Equality_Myth
{
    class ReferenceTypes
    {

        private WindowsApplication1.Equality_Myth.Class1 obj1;
        private WindowsApplication1.Equality_Myth.Class1 obj2;
        bool res;

        public ReferenceTypes()
        {
            obj1 = new Class1(1,"AA");
            obj2 = new Class1(1, "AA");
            obj2 = null;
        }

        public void MakeClone()
        {
            obj1=obj2;
        }
        public void RemoveClone()
        {
            obj1 = null;
            obj2 = null;
            obj1 = new Class1(1, "AA");
            obj2 = new Class1(1, "AA");
        }
        public void SetNull()
        {
            obj1 = null;
            obj2 = null;
            obj1 = new Class1(1, "AA");           
        }

        public void FindEquality()
        {
            res = obj1 == obj2;   				// True

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
            res = Object.ReferenceEquals(obj1, obj2); 		    // False            

            if (res)
            {
                MessageBox.Show("Equal");
            }
            else
            {
                MessageBox.Show("Un Equal");
            }
        }
        public void FindVirtualEqualsEquality()
        {
            res = obj1.Equals(obj2);		    // True            

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
            res = Object.ReferenceEquals(obj1, obj2);  		    // True            

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
