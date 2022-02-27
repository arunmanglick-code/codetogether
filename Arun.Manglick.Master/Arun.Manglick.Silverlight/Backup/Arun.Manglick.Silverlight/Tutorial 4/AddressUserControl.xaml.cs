using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Arun.Manglick.Silverlight;

namespace Arun.Manglick.Silverlight
{
    public partial class AddressUserControl : UserControl
    {
        private Address theAddress = null;
        public AddressUserControl()
        {
            InitializeComponent();
            theAddress = new Address();
            Loaded += new RoutedEventHandler(Page_Loaded);
        }

        void Page_Loaded(object sender, RoutedEventArgs e)
        {
            AddressGrid.KeyDown += new KeyEventHandler(AddressGrid_KeyDown);
        }

        void AddressGrid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.C && Keyboard.Modifiers == ModifierKeys.Control)
            {
                theAddress.Location = "The Computer History Museum ";
                theAddress.Address1 = "No Longer in Boston!";
                theAddress.Address2 = "1401 N. Shoreline Blvd";
                theAddress.City = "Mountain View, CA 94043";
            }
            if (e.Key == Key.M && Keyboard.Modifiers == ModifierKeys.Control)
            {
                theAddress.Location = "Microsoft";
                theAddress.Address1 = "One Microsoft Way";
                theAddress.Address2 = "Building 10";
                theAddress.City = "Redmond, WA 98052";
            }
            AddressGrid.DataContext = theAddress;
        }   // end KeyDown event handler
    }       // end class AddressUserControl
}


