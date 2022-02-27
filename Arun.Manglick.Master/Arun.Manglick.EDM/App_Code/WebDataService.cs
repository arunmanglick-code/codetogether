using System;
using System.Data.Services;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Web;

public class WebDataService : DataService<northwndModel.northwndEntities>
{
    // This method is called only once to initialize service-wide policies.
    public static void InitializeService(IDataServiceConfiguration config)
    {
        // TODO: set rules to indicate which entity sets and service operations are visible, updatable, etc.
        // Examples:
         config.SetEntitySetAccessRule("*", EntitySetRights.AllRead);
        // config.SetServiceOperationAccessRule("MyServiceOperation", ServiceOperationRights.All);
    }
}
