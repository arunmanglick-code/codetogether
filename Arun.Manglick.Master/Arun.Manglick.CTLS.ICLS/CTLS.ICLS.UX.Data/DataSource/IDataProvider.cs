using System.Collections;
using System.ComponentModel;
using CT.SLABB.Data;

namespace CTLS.ICLS.UX.Data
{
    /// <summary>
    /// A Data provider that can provide data to consumers like DataSource.
    /// </summary>
    public interface IDataProvider : INotifyPropertyChanged
    {
        #region Properties

        IEnumerable Items {
            get;
        }

        RefreshableStatus Status
        {
            get;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Starts an asynchronous data fetch operation.
        /// </summary>
        void BeginLoad(object sender, FilterDescriptionCollection searchPredicates, SortDescriptionCollection sortPredicates, int maxRecords);

        /// <summary>
        /// Overload added by Roman Rubin
        /// Starts an asynchronous data fetch operation.
        /// </summary>
        void BeginLoad(object sender, FilterDescriptionCollection searchPredicates, SortDescriptionCollection sortPredicates, int startRecord, int endRecord, string dataSourceId);

        /// <summary>
        /// Cancels a previous asynchronous data fetch operation.
        /// </summary>
        void CancelLoad();

        #endregion
    }
}
