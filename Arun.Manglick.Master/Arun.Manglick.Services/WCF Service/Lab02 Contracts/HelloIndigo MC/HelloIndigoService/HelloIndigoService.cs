using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace HelloIndigo.Lab2.MC
{
    [ServiceContract(Namespace = "http://www.thatindigogirl.com/samples/2006/06")]
    public interface IHelloIndigoService
    {
        [OperationContract]
        SaveGigResponse SaveGig(SaveGigRequest requestMessage);

        [OperationContract]
        GetGigResponse GetGig(GetGigRequest requestMessage);
    }

    public class HelloIndigoService : IHelloIndigoService
    {
        private LinkItem m_linkItem;

        #region IGigManager Members

        public SaveGigResponse SaveGig(SaveGigRequest requestMessage)
        {
            m_linkItem = requestMessage.Item;
            return new SaveGigResponse();
        }

        public GetGigResponse GetGig(GetGigRequest requestMessage)
        {
            if (requestMessage.LicenseKey != "XXX")
                throw new FaultException("Invalid license key.");

            return new GetGigResponse(m_linkItem);
        }

        #endregion
    }
}
