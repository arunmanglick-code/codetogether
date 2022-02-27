using System;
using System.Collections.Generic;
using System.Text;

namespace WindowsApplication1.DesignPatterns.Creational.Factory
{
    public abstract class Product_Home
    {
        public abstract string ShowHomeDetails();
    }

    public class ProductHome_HighStyle : Product_Home
    {
        public override string ShowHomeDetails()
        {
            return "This is High Style Home";
        }
    }

    public class ProductHome_MediumStyle : Product_Home
    {
        public override string ShowHomeDetails()
        {
            return "This is Medium Style Home";
        }
    }

    public class ProductHome_LowStyle : Product_Home
    {
        public override string ShowHomeDetails()
        {
            return "This is Low Style Home";
        }
    }

}
