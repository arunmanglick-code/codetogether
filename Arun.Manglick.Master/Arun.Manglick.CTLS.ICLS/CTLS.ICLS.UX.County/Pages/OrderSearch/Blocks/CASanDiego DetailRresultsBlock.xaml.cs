using System.Windows;
using System.Windows.Controls;
using CTLS.Shared.UX.Controls;

namespace CTLS.ICLS.UX.CountySearch
{
    public partial class CASanDiegoDetailRresultsBlock : UserControl
    {      
        #region .ctor
        /// <summary>
        /// Constructor
        /// </summary>
        public CASanDiegoDetailRresultsBlock()
        {
            InitializeComponent();
            this.grvDetailResults.Loaded += new RoutedEventHandler(grvDetailResults_Loaded);
        }
        #endregion

        #region Event Handlers
        /// <summary>
        /// Loading event of MultiSelectGridView
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">RoutedEventArgs</param>
        /// <returns>void</returns>
        void grvDetailResults_Loaded(object sender, RoutedEventArgs e)
        {
            MultiSelectCheckBoxColumn checkBoxColumn = grvDetailResults.Columns[0] as MultiSelectCheckBoxColumn;
            checkBoxColumn.Visibility = System.Windows.Visibility.Collapsed;
        }
        #endregion     
    }
}
