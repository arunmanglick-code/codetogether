
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace HelloIndigo.Lab2.MC
{
    [DataContract(Namespace = "http://schemas.thatindigogirl.com/samples/2006/06")]
    public class PhotoLink: LinkItem
    {
        public PhotoLink()
        {
            this.Category = LinkItemCategories.Photo;
        }

    }
}
