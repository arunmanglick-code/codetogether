using System;
using System.Collections.Generic;
using System.Text;

namespace WindowsApplication1.DesignPatterns.Behavioral.Strategy
{
    public class Product_Home
    {
        protected Strategy_Builder strategy;
                
        public Strategy_Builder Strategy
        {
            set { strategy = value; }
        }

        public string ShowHomeDetails()
        {
            return strategy.ProvideHome();
        }
    }
}
