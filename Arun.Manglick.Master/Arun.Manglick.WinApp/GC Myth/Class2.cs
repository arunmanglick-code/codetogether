using System;
using System.Collections.Generic;
using System.Text;

namespace WindowsApplication1.GC_Myth
{
    class Class2
    {
        private int Id2;

        public int Class2Id
        {
            get { return Id2; }
            set { Id2 = value; }
        }
        private Class1 objClass1;

        internal Class1 ObjClass1
        {
            get { return objClass1; }
            set { objClass1 = value; }
        }
    }
}
