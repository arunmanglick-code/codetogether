using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Arun.Manglick.UI
{
    public class Invoice
    {
        #region Member variable
        int m_InvoiceNumber;
        string m_CustomerName;        
        double m_InvoicePrice;
       
        #endregion Member variable

        #region Properties

        /// <summary>
        ///Gets or Sets InvoiceNumber
        /// </summary>
        public int InvoiceNumber
        {
            get
            {
                return m_InvoiceNumber;
            }
            set
            {
                m_InvoiceNumber = value;
            }
        }

        /// <summary>
        ///Gets or Sets CustomerNumber
        /// </summary>
        public string CustomerName
        {
            get
            {
                return m_CustomerName;
            }
            set
            {
                m_CustomerName = value;
            }
        }
                
        /// <summary>
        ///Gets or Sets TotalPrice
        /// </summary>
        public double InvoicePrice
        {
            get
            {
                return m_InvoicePrice;
            }
            set
            {
                m_InvoicePrice = value;
            }
        }

        #endregion

        #region Constructor

        public Invoice(int invoiceNumber, string customerName, double price)
        {
            m_InvoiceNumber = invoiceNumber;
            m_CustomerName = customerName;
            m_InvoicePrice = price;
        }

        public Invoice()
        {
        }

        #endregion

        #region Methods

        /// <summary>
        ///Retuns URL to the PDF containing all the details of invoices 
        /// </summary>
        public string GetInvoiceDetails()
        {
            return null;
        }

        #endregion

        #region Comparable Classes

        //public class SortingInvoicesByInvoiceNumber : IComparer<Invoice>
        //{
        //    public int Compare(Invoice obj1, Invoice obj2)
        //    {
        //        return obj1.InvoiceNumber.CompareTo(obj2.InvoiceNumber);
        //    }
        //}

        //public class SortingInvoicesByCustomerName : IComparer<Invoice>
        //{
        //    public String Compare(Invoice obj1, Invoice obj2)
        //    {
        //        return obj1.CustomerName.CompareTo(obj2.CustomerName);
        //    }
        //}

        //public class SortingInvoicesByPrice : IComparer<Invoice>
        //{
        //    public double Compare(Invoice obj1, Invoice obj2)
        //    {
        //        return obj1.InvoicePrice.CompareTo(obj2.InvoicePrice);
        //    }
        //}

        #endregion
    }
}
