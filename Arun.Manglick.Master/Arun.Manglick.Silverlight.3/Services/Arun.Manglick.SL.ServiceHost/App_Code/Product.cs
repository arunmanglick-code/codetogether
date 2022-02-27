using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Collections.Generic;
using System.Text;

[ServiceContract(Namespace = "")]
[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
public class Product
{
    [OperationContract]
    public void DoWork()
    {
        // Add your operation implementation here
        return;
    }

    // Add more operations here and mark them with [OperationContract]
}
