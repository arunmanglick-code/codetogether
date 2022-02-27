using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Arun.Manglick.Silverlight.Web
{
    // NOTE: If you change the class name "Service1" here, you must also update the reference to "Service1" in Web.config.
    public class Service1 : IService1
    {
        public List<Employee> GetEmployeeByLastName(string lastName)
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            var matchingEmployees = from employee in db.Employees
                                    where employee.LastName.StartsWith(lastName)
                                    select employee;
            return matchingEmployees.ToList();
        }
    }
}
