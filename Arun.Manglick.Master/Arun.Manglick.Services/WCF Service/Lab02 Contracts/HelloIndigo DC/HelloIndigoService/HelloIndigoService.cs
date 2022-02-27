using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace HelloIndigo.Lab2.DC
{
    [ServiceContract(Namespace = "http://www.thatindigogirl.com/samples/2006/06")]
    public interface IHelloIndigoService
    {
        [OperationContract]
        void SaveGig(LinkItem item);

        [OperationContract]
        LinkItem GetGig();
    }

    public class HelloIndigoService : IHelloIndigoService
    {
        private LinkItem m_linkItem;

        #region IHelloIndigoService Members

        public void SaveGig(LinkItem item)
        {
            m_linkItem = item;
        }

        public LinkItem GetGig()
        {
            return m_linkItem;
        }

        #endregion
    }
}
