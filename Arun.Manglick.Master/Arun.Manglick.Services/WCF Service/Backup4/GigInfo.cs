   
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace HelloIndigo.Lab2.KT
{
    [DataContract(Namespace = "http://schemas.thatindigogirl.com/samples/2006/06")]
    public class GigInfo: LinkItem
    {
        public GigInfo()
        {
            this.Category = LinkItemCategories.Gig;
        }
    }
}
