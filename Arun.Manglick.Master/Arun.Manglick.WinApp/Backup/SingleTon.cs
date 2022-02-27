using System;
using System.Collections.Generic;
using System.Text;

namespace WindowsApplication1
{
    class SingleTon
    {
        public int i=6;
        public static SingleTon obj = new SingleTon();  // Note: Here the non-static constructor will take precedence over static constructor. 
                                                        // So comment that normal constructor, otherwise it will no longer be Singleton class

        static SingleTon()
        {
            
        }

        
        //public SingleTon()
        //{
        //    i = 7;
        //}

        public static SingleTon GetInstance()
        {
            return obj;
        }
    }
}
