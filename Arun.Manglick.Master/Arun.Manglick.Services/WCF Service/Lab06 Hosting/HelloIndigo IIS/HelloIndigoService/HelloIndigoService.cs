using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace HelloIndigo.Lab6.IIS
{
    [ServiceContract(Namespace = "http://www.thatindigogirl.com/samples/2006/06")]
    public interface IHelloIndigoService
    {
        [OperationContract]
        string HelloIndigo();
    }

    public class HelloIndigoService : IHelloIndigoService
    {
        #region IHelloIndigoService Members

        public string HelloIndigo()
        {
            return "Hello Indigo from HelloIndigo.Lab6.IIS";
        }

        #endregion
    }


}
