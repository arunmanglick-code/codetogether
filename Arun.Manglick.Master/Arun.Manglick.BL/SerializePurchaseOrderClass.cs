using System;
using System.Collections.Generic;
using System.Text;

namespace Arun.Manglick.BL
{
    public class SerializePurchaseOrderClass
    {
        public AddressTo ShipTo;
        public string OrderDate;
        public OrderedItem[] OrderedItems;        
        public decimal SubTotal;
        public decimal ShipCost;
        public decimal TotalCost;   
    }

    public class AddressTo
    {
        public string Name;
        public string Line1;
        public string City;
        public string State;
        public string Zip;
    }

    public class OrderedItem
    {
        public string ItemName;
        public string Description;
        public decimal UnitPrice;
        public int Quantity;
        public decimal LineTotal;

        public void Calculate()
        {
            LineTotal = UnitPrice * Quantity;
        }
    }


}
