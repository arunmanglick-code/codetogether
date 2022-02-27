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
using System.Windows.Browser;

namespace Arun.Manglick.Silverlight.Views.Browser_Integration.JSToSilverlight
{
    [ScriptableType()]
    public partial class ScriptableSilverlight : UserControl
    {
        public ScriptableSilverlight()
        {
            InitializeComponent();

            HtmlPage.RegisterScriptableObject("Page", this);
            HtmlPage.RegisterCreateableType("RandomNumbers", typeof(RandomNumbers));
        }

        [ScriptableMember()]
        public void ChangeText(string newText)
        {
            lbl.Text = newText;
        }

    }

    [ScriptableType()]
    public class RandomNumbers
    {
        private Random random = new Random();

        [ScriptableMember()]
        public int GetRandomNumberInRange(int from, int to)
        {
            return random.Next(from, to+1);
        }
    }
}
