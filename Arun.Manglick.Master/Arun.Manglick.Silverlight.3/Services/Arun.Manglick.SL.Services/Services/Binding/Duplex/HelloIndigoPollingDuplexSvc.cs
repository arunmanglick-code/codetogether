using System;
using System.ServiceModel;
using System.Threading;
using System.ServiceModel.Activation;

namespace Arun.Manglick.SL.Services.Binding.PollingDuplex
{

    [ServiceContract(Name = "HelloIndigoContract", Namespace = "http://www.arunmanglick.com/", CallbackContract = typeof(IHelloIndigoPollingDuplexSvcCallback))]
    public interface IHelloIndigoPollingDuplexSvc
    {
        [OperationContract(IsOneWay=true)]
        void HelloIndigo(string message);
    }

    public interface IHelloIndigoPollingDuplexSvcCallback
    {
        [OperationContract(IsOneWay=true)]
        string HelloIndigoCallback(string message);
    }

    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class HelloIndigoPollingDuplexSvc : IHelloIndigoPollingDuplexSvc
    {
        #region IHelloIndigoService Members

        public void HelloIndigo(string callBackMessage)
        {
            Console.WriteLine("Hello Indigo from HelloIndigo.Lab5.Duplex Service");

            IHelloIndigoPollingDuplexSvcCallback callback = OperationContext.Current.GetCallbackChannel<IHelloIndigoPollingDuplexSvcCallback>();
            callback.HelloIndigoCallback(callBackMessage);
        }

        #endregion
    }

}
