using System;
using System.Collections.Generic;
using System.Text;


namespace WindowsApplication1.CopyConstructor_Myth
{
    class Employee
    {
        public int age;
        public string city;

        // Instance constructor
        public Employee(int age, string city)
        {
            this.age = age;
            this.city = city;
        }

        // Copy constructor
        public Employee(Employee obj)
        {
            age = obj.age;
            city = obj.city;
        }

        public string Details 
        {
            get
            {
                return " Age is: " + age.ToString() + " City is: " + city;
            }
        }
    }
   
}
