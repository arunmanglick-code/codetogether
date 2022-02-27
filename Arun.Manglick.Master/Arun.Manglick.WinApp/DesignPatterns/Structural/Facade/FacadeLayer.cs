using System;
using System.Collections.Generic;
using System.Text;

namespace WindowsApplication1.DesignPatterns.Structural.Facade
{
    class Facade_TowerHomes
    {
        public string ProvieTowerHome()
        {
            return "This is your Tower Home";
        }
    }

    class Facade_IndividualHomes
    {
        public string ProvideIndividualHome()
        {
            return "This is your Individual Home";
        }
    }
    
    class Facade_Bank
    {
        public string ProvideBank()
        {
            return "This is your Bank";
        }
    }

    class Facade_ITCompany
    {
        public string ProvideITCompany()
        {
            return "This is your ITCompany";
        }
    }

    class Facade_MultiTheater
    {
        public string ProvideMultiTheater()
        {
            return "This is your MultiTheater";
        }
    }

    class Facade_BasicSchool
    {
        public string ProvieBasicSchool()
        {
            return "This is your BasicSchool";
        }
    }

    class Facade_College
    {
        public string ProvideCollege()
        {
            return "This is your College";
        }
    }
}
