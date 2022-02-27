using System;
using System.Collections.Generic;
using System.Text;

namespace WindowsApplication1.DesignPatterns.Creational.Abstract_Factory
{
    public abstract class AbstractFactory_Builder
    {
        public abstract AbstractProduct_Home ProvideHome();
        public abstract AbstractProduct_Car GiftCar();
    }

    public class ConcreteFactory_BuilderHighStyle : AbstractFactory_Builder
    {
        public override AbstractProduct_Home ProvideHome()
        {
            return new ProductHome_HighStyle();
        }

        public override AbstractProduct_Car GiftCar()
        {
            return new ProductCar_HighStyle();
        }
    }

    public class ConcreteFactory_BuilderMediumStyle : AbstractFactory_Builder
    {
        public override AbstractProduct_Home ProvideHome()
        {
            return new ProductHome_MediumStyle();
        }

        public override AbstractProduct_Car GiftCar()
        {
            return new ProductCar_MediumStyle();
        }
    }

    public class ConcreteFactory_BuilderLowStyle : AbstractFactory_Builder
    {
        public override AbstractProduct_Home ProvideHome()
        {
            return new ProductHome_LowStyle();
        }

        public override AbstractProduct_Car GiftCar()
        {
            return new ProductCar_LowStyle();
        }
    }    
}
