
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace HelloIndigo.Lab2.MC
{
    [DataContract(Namespace = "http://schemas.thatindigogirl.com/samples/2006/06")]
    public class MP3Link: LinkItem
    {
        public MP3Link()
        {
            this.Category = LinkItemCategories.MP3;
        }

    }
}
