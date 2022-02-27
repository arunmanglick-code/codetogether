using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Windows.Forms;
using System.Threading;
using System.ServiceModel.Activation;

namespace Arun.Manglick.SL.Services
{
    [ServiceContract(Namespace = "http://www.ArunManglick.Silverlight3.com/PerSessionCounter", SessionMode = SessionMode.Required)]
    public interface ICounterServiceSessionSvc
    {
        [OperationContract]
        int IncrementCounter();
    }

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class CounterServiceSessionSvc : ICounterServiceSessionSvc
    {
        private int m_counter;

        #region ICounterServiceSession Members

        int ICounterServiceSessionSvc.IncrementCounter()
        {
            m_counter++;

            MessageBox.Show(String.Format("Incrementing Session counter to {0}.\r\nSession Id: {1}", m_counter, OperationContext.Current.SessionId));
            return m_counter;
        }

        #endregion

    }
}
