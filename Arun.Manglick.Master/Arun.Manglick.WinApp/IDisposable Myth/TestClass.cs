using System;
using System.Collections.Generic;
using System.Text;

namespace WindowsApplication1.IDisposable_Myth
{
    class TestClass
    {
        public MyResource GetResource()
        {
            int x = 10;
            IntPtr ptr = new IntPtr(x);
            MyResource obj = new MyResource(ptr);
            return obj;
        }
    }
}
