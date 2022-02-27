using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathsHelper;

namespace Arun.Manglick.NUnit
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter two numbers\n");
            int number1;
            int number2;
            number1 = int.Parse(Console.ReadLine());
            number2 = int.Parse(Console.ReadLine());

            Maths helper = new Maths();
            int x = helper.Add(number1, number2, true);
            Console.WriteLine("\nThe sum of " + number1 +
                " and " + number2 + " is " + x);
            Console.ReadKey();
            int y = helper.Subtract(number1, number2);
            Console.WriteLine("\nThe difference between " +
                  number1 + " and" + number2 + "  is " + y);
            Console.ReadKey();
        }
    }
}
