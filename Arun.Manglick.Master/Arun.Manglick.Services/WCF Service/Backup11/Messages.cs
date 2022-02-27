using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;

namespace HelloIndigo.Lab2.MC
{
    #region Get Contract

    //[MessageContract]
    //[MessageContract(IsWrapped=true, WrapperName="GetGig")]
    [MessageContract(IsWrapped = false)]
    public class GetGigRequest
    {
        private string m_licenseKey;

        [MessageHeader]
        public string LicenseKey
        {
            get { return m_licenseKey; }
            set { m_licenseKey = value; }
        }

    }

    //[MessageContract]
    [MessageContract(IsWrapped = false)]
    public class GetGigResponse
    {
        private LinkItem m_linkItem;

        public GetGigResponse()
        {
        }
        public GetGigResponse(LinkItem item)
        {
            this.m_linkItem = item;
        }

        [MessageBodyMember]
        public LinkItem Item
        {
            get { return m_linkItem; }
            set { m_linkItem = value; }
        }
    }

    #endregion

    #region Save Contract

    //[MessageContract]
    //[MessageContract(IsWrapped=true, WrapperName="SaveGig")]
    [MessageContract(IsWrapped = false)]
    public class SaveGigRequest
    {
        private LinkItem m_linkItem;

        [MessageBodyMember]
        public LinkItem Item
        {
            get { return m_linkItem; }
            set { m_linkItem = value; }
        }

    }

    //[MessageContract]
    [MessageContract(IsWrapped = false)]
    public class SaveGigResponse
    {
    }

    #endregion
}
