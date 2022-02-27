using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace HelloIndigo.Lab2.KT
{
    [DataContract(Namespace = "http://schemas.thatindigogirl.com/samples/2006/06")]
    [KnownType(typeof(GigInfo))]
    [KnownType(typeof(PhotoLink))]
    [KnownType(typeof(MP3Link))]
    public class LinkItem
    {
        #region Private Variables

        [DataMember(Name = "Id", IsRequired = false, Order = 0)]
        private long m_id;

        [DataMember(Name = "Title", IsRequired = true, Order = 1)]
        private string m_title;

        [DataMember(Name = "Description", IsRequired = true, Order = 2)]
        private string m_description;

        [DataMember(Name = "DateStart", IsRequired = true, Order = 3)]
        private DateTime m_dateStart;

        [DataMember(Name = "DateEnd", IsRequired = false, Order = 4)]
        private DateTime m_dateEnd;

        [DataMember(Name = "Url", IsRequired = false, Order = 5)]
        private string m_url;

        [DataMember(Name = "Category", IsRequired = false, Order = 6)]
        private LinkItemCategories m_category;
        
        #endregion

        #region Public Properties

        public DateTime DateStart
        {
            get { return m_dateStart; }
            set { m_dateStart = value; }
        }

        public DateTime DateEnd
        {
            get { return m_dateEnd; }
            set { m_dateEnd = value; }
        }
        
        public string Url
        {
            get { return m_url; }
            set { m_url = value; }
        }

        public long Id
        {
            get { return m_id; }
            set { m_id = value; }
        }

        public string Title
        {
            get { return m_title; }
            set { m_title = value; }
        }

        public string Description
        {
            get { return m_description; }
            set { m_description = value; }
        }

        public LinkItemCategories Category
        {
            get { return m_category; }
            set { m_category = value; }
        }

        #endregion
    }

    [DataContract(Namespace = "http://schemas.thatindigogirl.com/samples/2006/06")]
    public enum LinkItemCategories
    {
        [EnumMember(Value= "Event")]
        Gig,
        [EnumMember(Value="Music")]
        MP3,
        [EnumMember(Value="Pics")]
        Photo
    }
}
