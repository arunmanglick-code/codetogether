using System;
using System.Collections.Generic;
using System.Text;

namespace WindowsApplication1.DesignPatterns.Creational.Abstract_Factory
{
    public abstract class AbstractProduct_Car
    {
        public abstract string ShowCarDetails();
    }

    public class ProductCar_HighStyle : AbstractProduct_Car
    {
        public override string ShowCarDetails()
        {
            return "This is High Style Car";
        }
    }

    public class ProductCar_MediumStyle : AbstractProduct_Car
    {
        public override string ShowCarDetails()
        {
            return "This is Medium Style Car";
        }
    }

    public class ProductCar_LowStyle : AbstractProduct_Car
    {
        public override string ShowCarDetails()
        {
            return "This is Low Style Car";
        }
    }
}
