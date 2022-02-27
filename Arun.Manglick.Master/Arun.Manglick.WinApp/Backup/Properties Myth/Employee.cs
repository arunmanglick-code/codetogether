using System;
using System.Collections.Generic;
using System.Text;


namespace WindowsApplication1.Properties_Myth
{
    class Employee
    {
        public int age;
        public static int age1;
        public string city;

        public Employee()
        {
            
        }

        // Instance constructor
        public Employee(int age, string city)
        {
            this.age = age;
            this.city = city;
        }

        

        public int ReadOnlyAge 
        {
            get
            {
                return age;
            }
        }

        public int WriteOnlyAge
        {
            set
            {
                age = value;
            }
        }

        public string this[string strCty]
        {
            get
            {
                return city;
            }
            set 
            {
                city = value;
            }
        }

        public string MyCity
        {
            get
            {
                return city;
            }
            set
            {
                city = value;
            }
        }
    }

    


   
}
