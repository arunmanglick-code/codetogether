using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Arun.Manglick.Silverlight.Views.Templates_And_CustomControls.CustomControls
{
    public partial class ListMover : UserControl
    {
        public ListMover()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(ListMover_Loaded);
        }

        void ListMover_Loaded(object sender, RoutedEventArgs e)
        {
            List<Instruction> alertsList = new List<Instruction>{
               {new Instruction("India")},
               {new Instruction("America")},
               {new Instruction("Africa")},
               {new Instruction("Australia")}
            };
            LeftListBoxElement.ItemsSource = alertsList;
        }

        private void MoveRightButtonElement_Click(object sender, RoutedEventArgs e)
        {
            Instruction leftItem = LeftListBoxElement.Items[LeftListBoxElement.SelectedIndex] as Instruction; // LeftListBoxElement.se.SelectedItem as ListBoxItem;
            RightListBoxElement.Items.Add(leftItem);
            //LeftListBoxElement.Items.Remove(leftItem);
        }

        private void MoveLeftButtonElement_Click(object sender, RoutedEventArgs e)
        {
            Instruction leftItem = RightListBoxElement.SelectedItem as Instruction;
            LeftListBoxElement.Items.Add(leftItem);
            //LeftListBoxElement.Items.Remove(leftItem);
        }

        
    }

    public class Instruction
    {
        public string MyText { get; set; }
        public Instruction(string val)
        {
            MyText = val;
        }
    }
}
