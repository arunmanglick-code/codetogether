using System;
using System.Collections.Generic;
using System.Text;

namespace WindowsApplication1.DesignPatterns.Structural.Bridge
{
    public abstract class Product_Home
    {
        protected Implementor_Builder implementor;

        public Implementor_Builder Implementor
        {
            set { implementor = value; }
        }

        public virtual string ShowHomeDetails()
        {
            return implementor.ProvideHome();
        }
    }

    class Refined_Product_Home : Product_Home
    {
        public override string ShowHomeDetails()
        {
            return implementor.ProvideHome();
        }
    }
}
