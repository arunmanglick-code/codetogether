using System;
using System.Collections.Generic;
using System.Text;

namespace WindowsApplication1.DesignPatterns.Behavioral.Strategy
{
    public abstract class Strategy_Builder
    {
        public abstract string ProvideHome();
    }

    public class ConcreteStrategy_BuilderHighStyle : Strategy_Builder
    {
        public override string ProvideHome()
        {
            return "This is High Style Home Strategy";
        }
    }

    public class ConcreteStrategy_BuilderMediumStyle : Strategy_Builder
    {
        public override string ProvideHome()
        {
            return "This is Medium Style Home Strategy";
        }
    }

    public class ConcreteStrategy_BuilderLowStyle : Strategy_Builder
    {
        public override string ProvideHome()
        {
            return "This is Low Style Home Strategy";
        }
    }    
}
