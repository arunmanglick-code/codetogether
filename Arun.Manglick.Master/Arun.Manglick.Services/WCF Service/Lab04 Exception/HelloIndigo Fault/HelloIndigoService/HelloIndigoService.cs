using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace HelloIndigo.Lab4.Fault
{
    [ServiceContract(Namespace = "http://www.thatindigogirl.com/samples/2006/06")]
    public interface IHelloIndigoService
    {
        [OperationContract]
        [FaultContract(typeof(InvalidOperationException))]
        void ThrowSimpleFault();

        [OperationContract()]
        [FaultContract(typeof(InvalidOperationException))]
        void ThrowFaultException();

        [OperationContract]
        [FaultContract(typeof(InvalidOperationException))]
        void ThrowMessageFault();

    }   

    [ServiceBehavior(IncludeExceptionDetailInFaults=false)]
    public class HelloIndigoService : IHelloIndigoService
    {
        #region IService Members

        /// <summary>
        /// 
        /// </summary>
        public void ThrowSimpleFault()
        {
            throw new FaultException(new FaultReason("Invalid operation."));
        }

        /// <summary>
        /// 
        /// </summary>
        public void ThrowFaultException()
        {
            FaultException<InvalidOperationException> fe = new FaultException<InvalidOperationException>(
                                                        new InvalidOperationException("An invalid operation has occured."),
                                                        new FaultReason("Invalid operation."), 
                                                        new FaultCode("Server", new FaultCode(String.Format("Server.{0}", typeof(NotImplementedException)))));
            throw fe;
        }

        /// <summary>
        /// 
        /// </summary>
        public void ThrowMessageFault()
        {
            InvalidOperationException error = new InvalidOperationException("An invalid operation has occurred.");
            MessageFault mfault = MessageFault.CreateFault(
                                                        new FaultCode("Server", new FaultCode(String.Format("Server.{0}", error.GetType().Name))), 
                                                        new FaultReason("Invalid operation."), 
                                                        error);

            FaultException fe = FaultException.CreateFault(mfault, typeof(InvalidOperationException));

            throw fe;
        }


        #endregion
    }
}
