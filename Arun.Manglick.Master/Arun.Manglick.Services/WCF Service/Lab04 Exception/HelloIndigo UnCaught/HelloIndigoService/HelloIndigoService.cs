using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace HelloIndigo.Lab4.UnCaught
{
    [ServiceContract(Namespace = "http://www.thatindigogirl.com/samples/2006/06")]
    public interface IHelloIndigoService
    {
        [OperationContract]
        void ThrowException();

        [OperationContract(IsOneWay = true)]
        void ThrowExceptionOneWay();

        [OperationContract]
        string GoodOperation();

    }

    [ServiceBehavior(IncludeExceptionDetailInFaults=false)]
    public class HelloIndigoService : IHelloIndigoService
    {
        #region IService Members

        public void ThrowException()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void ThrowExceptionOneWay()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public string GoodOperation()
        {
            return string.Format("GoodOperation() called at {0}", DateTime.Now);
        }

        #endregion
    }
}
