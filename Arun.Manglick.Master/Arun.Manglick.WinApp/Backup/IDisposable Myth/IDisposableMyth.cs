using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;


namespace WindowsApplication1.IDisposable_Myth
{   
    public class DisposeExample
    {   
        public static void Test()
        {
            // Insert code here to create
            // and use the MyResource object.   
        }
    }

    public class MyResource : IDisposable
    {
        private IntPtr handle;  // Pointer to an external Unmanaged resource.
        private Component component = new Component(); // Other Managed resource this class uses.            
        private bool disposed = false; // Track whether Dispose has been called.

        public MyResource(IntPtr handle)
        {
            this.handle = handle;
        }

        


        // Do not make this method virtual. A derived class should not be able to override this method.
        public void Dispose()  // IDisposable Implementation
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Dispose(bool disposing) executes in two distinct scenarios.
        // If disposing equals true, the method has been called directly or indirectly by a user's code. Managed and unmanaged resources can be disposed.
        // If disposing equals false, the method has been called by the runtime from inside the finalizer and you should not reference other objects. Only unmanaged resources can be disposed.
        private void Dispose(bool thruDispose)
        {
            // Check to see if Dispose has already been called.
            if (!this.disposed)
            {
                // If disposing equals true, dispose all managed and unmanaged resources.
                if (thruDispose)
                {
                    component.Dispose(); // Dispose Managed resources.
                }

                // Cleanup Unmanaged resources.
                // If disposing is false, only the following code is executed.
                CloseHandle(handle);
                handle = IntPtr.Zero;
            }
            disposed = true;
        }

        // Use interop to call the method necessary  to clean up the unmanaged resource.
        [System.Runtime.InteropServices.DllImport("Kernel32")]
        private extern static Boolean CloseHandle(IntPtr handle);

        // Use C# destructor syntax for finalization code.
        // This destructor will run only if the Dispose method does not get called.
        // It gives your base class the opportunity to finalize.
        // Do not provide destructors in types derived from this class.
        ~MyResource()
        {
            // Do not re-create Dispose clean-up code here.
            // Calling Dispose(false) is optimal in terms of readability and maintainability.
            try
            {
                Dispose(false);
            }
            catch (System.Exception ex)
            {
            }
        }
    }

}
