using System;
using System.Collections.Generic;
using System.Text;

namespace WindowsApplication1.DesignPatterns.Creational.Singleton
{
    class Singleton_Lazy
    {
        private static Singleton_Lazy _instance = null;
        public int i = 5;
        protected Singleton_Lazy() { }

        public static Singleton_Lazy GetInstance()
        {
            if (_instance == null)
            {
                _instance = new Singleton_Lazy();
            }

            return _instance;
        }
    }


    class Singleton_Lock
    {
        private static Singleton_Lock _instance = null;
        public int i = 6;
        protected Singleton_Lock() { }

        public static Singleton_Lock GetInstance()
        {
            if (_instance == null)
            {
                lock (typeof(Singleton_Lock))
                {
                    if (_instance == null)
                    {
                        _instance = new Singleton_Lock();
                    }
                }
            }
            return _instance;
        }
    }

    class Singleton_Volatile
    {
        private static volatile Singleton_Volatile _instance = null;
        public int i = 7;
        protected Singleton_Volatile() { }

        public static Singleton_Volatile GetInstance()
        {
            if (_instance == null)
            {
                lock (typeof(Singleton_Volatile))
                {
                    if (_instance == null)
                    {
                        _instance = new Singleton_Volatile();
                    }
                }
            }
            return _instance;
        }
    }


    class SingleTon_ThreadSafe
    {
        public int i = 8;
        public static SingleTon_ThreadSafe Instance = new SingleTon_ThreadSafe();

        // Note: Here the non-static constructor will take precedence over static constructor. 
        // So you can comment the ‘Static’ constructor, unless you are required to initialize any static variable before any object is created.

        private SingleTon_ThreadSafe()
        {
            i = 9;
        }

        //static SingleTon_ThreadSafe()
        //{ }
        
        public static SingleTon_ThreadSafe GetInstance()
        {
            return Instance;
        }
    }

}
