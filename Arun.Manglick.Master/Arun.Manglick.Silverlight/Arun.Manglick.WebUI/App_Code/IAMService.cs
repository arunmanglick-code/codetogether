using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

// NOTE: If you change the interface name "IAMService" here, you must also update the reference to "IAMService" in Web.config.
[ServiceContract]
public interface IAMService
{
    [OperationContract]
    List<Customer> GetCustomerByContactName(string lastName);
    
}
