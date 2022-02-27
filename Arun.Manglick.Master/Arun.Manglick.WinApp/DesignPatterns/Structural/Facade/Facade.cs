using System;
using System.Collections.Generic;
using System.Text;

namespace WindowsApplication1.DesignPatterns.Structural.Facade
{
    class Facade
    {
        Facade_TowerHomes towerHome;
        Facade_IndividualHomes individualHome;
        Facade_Bank bank;
        Facade_MultiTheater theater;
        Facade_BasicSchool school;
        Facade_College college;
        Facade_ITCompany company;

        public Facade()
        {
            towerHome = new Facade_TowerHomes();
            individualHome = new Facade_IndividualHomes();
            bank = new Facade_Bank();
            theater = new Facade_MultiTheater();
            school = new Facade_BasicSchool();
            college = new Facade_College();
            company = new Facade_ITCompany();
        }

        public string BuildMetroCity()
        {
            string str = towerHome.ProvieTowerHome() + "\n";
            str += bank.ProvideBank() + "\n";
            str += theater.ProvideMultiTheater() + "\n";
            str += college.ProvideCollege() + "\n";
            str += company.ProvideITCompany();
            
            return str;
        }

        public string BuildUrbanCity()
        {
            string str = individualHome.ProvideIndividualHome() + "\n";
            str += bank.ProvideBank() + "\n";
            str += school.ProvieBasicSchool();

            return str;
        }
    }
}
