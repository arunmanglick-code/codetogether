using System;
using System.Collections.Generic;
using System.Text;

namespace WindowsApplication1.InnerException
{
    public class MyAppException : ApplicationException
    {
        public MyAppException(String message) : base(message)
        { }
        public MyAppException(String message, Exception inner) : base(message, inner) 
        { }
    }

    public class ExceptExample
    {
        private void ThrowInner()
        {
            throw new MyAppException("Inner exception");
        }

        public void CatchInner()
        {
            try
            {
                this.ThrowInner();
            }
            catch (Exception e)
            {
                throw new MyAppException("Immediate Exception", e);
            }
        }
    }
}
