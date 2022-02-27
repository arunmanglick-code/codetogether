using System;
using System.Collections.Generic;
using System.Text;

namespace WindowsApplication1.DesignPatterns.Creational.Abstract_Factory
{
    public abstract class AbstractProduct_Home
    {
        public abstract string ShowHomeDetails();
    }

    public class ProductHome_HighStyle : AbstractProduct_Home
    {
        public override string ShowHomeDetails()
        {
            return "This is High Style Home";
        }
    }

    public class ProductHome_MediumStyle : AbstractProduct_Home
    {
        public override string ShowHomeDetails()
        {
            return "This is Medium Style Home";
        }
    }

    public class ProductHome_LowStyle : AbstractProduct_Home
    {
        public override string ShowHomeDetails()
        {
            return "This is Low Style Home";
        }
    }

}
