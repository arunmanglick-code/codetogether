using System;
using System.Collections.Generic;
using System.Text;

namespace WindowsApplication1.DesignPatterns.Structural.Bridge
{
    public abstract class Implementor_Builder
    {
        public abstract string ProvideHome();
    }

    public class ConcreteImplementor_BuilderHighStyle : Implementor_Builder
    {
        public override string ProvideHome()
        {
            return "This is High Style Home Implementor";
        }
    }

    public class ConcreteImplementor_BuilderMediumStyle : Implementor_Builder
    {
        public override string ProvideHome()
        {
            return "This is Medium Style Home Implementor";
        }
    }

    public class ConcreteImplementor_BuilderLowStyle : Implementor_Builder
    {
        public override string ProvideHome()
        {
            return "This is Low Style Home Implementor";
        }
    }    
}
