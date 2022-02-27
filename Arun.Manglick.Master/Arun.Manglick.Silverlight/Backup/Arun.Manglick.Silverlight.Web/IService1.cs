using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Arun.Manglick.Silverlight.Web
{
    // NOTE: If you change the interface name "IService1" here, you must also update the reference to "IService1" in Web.config.
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        List<Employee> GetEmployeeByLastName(string lastName);
    }
}
