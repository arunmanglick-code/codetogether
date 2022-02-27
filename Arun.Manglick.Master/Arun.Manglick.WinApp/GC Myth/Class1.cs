using System;
using System.Collections.Generic;
using System.Text;

namespace WindowsApplication1.GC_Myth
{
    class Class1
    {
        private int Id1;

        public int Class1Id
        {
            get { return Id1; }
            set { Id1 = value; }
        }
        private Class2 objClass2;

        internal Class2 ObjClass2
        {
            get { return objClass2; }
            set { objClass2 = value; }
        }
    }
}
