using System;
using System.Collections.Generic;
using System.Text;

namespace WindowsApplication1.DesignPatterns.Creational.Factory
{
    public abstract class Factory_Builder
    {
        public abstract Product_Home ProvideHome();
    }

    public class ConcreteFactory_BuilderHighStyle : Factory_Builder
    {
        public override Product_Home ProvideHome()
        {
            return new ProductHome_HighStyle();
        }
    }

    public class ConcreteFactory_BuilderMediumStyle : Factory_Builder
    {
        public override Product_Home ProvideHome()
        {
            return new ProductHome_MediumStyle();
        }
    }

    public class ConcreteFactory_BuilderLowStyle : Factory_Builder
    {
        public override Product_Home ProvideHome()
        {
            return new ProductHome_LowStyle();
        }
    }    
}
