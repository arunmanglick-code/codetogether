using System;
using System.Collections.Generic;
using System.Text;

namespace WindowsApplication1.MultiThread
{
    class WithoutThreadPool
    {
        public void LongTask1()
        {
            for (int i = 0; i <= 999; i++)
            {
                Console.WriteLine("Long Task 1 is being executed");
            }
        }

        public void LongTask2()
        {
            for (int i = 0; i <= 999; i++)
            {
                Console.WriteLine("Long Task 2 is being executed");
            }
        }       
   
    }
}
