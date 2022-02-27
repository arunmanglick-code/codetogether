using System;
using System.ServiceModel;
using System.Threading;

namespace HelloIndigo.Lab5.Duplex
{

    [ServiceContract(Name = "HelloIndigoContract", Namespace = "http://www.thatindigogirl.com/samples/2006/06", CallbackContract = typeof(IHelloIndigoServiceCallback))]
    public interface IHelloIndigoService
    {
        [OperationContract]//(IsOneWay=true)]
        void HelloIndigo(string message);
    }

    public interface IHelloIndigoServiceCallback
    {
        [OperationContract]//(IsOneWay=true)]
        string HelloIndigoCallback(string message);
    }

    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public class HelloIndigoService : IHelloIndigoService
    {
        #region IHelloIndigoService Members

        public void HelloIndigo(string callBackMessage)
        {
            Console.WriteLine("Hello Indigo from HelloIndigo.Lab5.Duplex Service");

            IHelloIndigoServiceCallback callback = OperationContext.Current.GetCallbackChannel<IHelloIndigoServiceCallback>();
            callback.HelloIndigoCallback(callBackMessage);
        }

        #endregion
    }

}
