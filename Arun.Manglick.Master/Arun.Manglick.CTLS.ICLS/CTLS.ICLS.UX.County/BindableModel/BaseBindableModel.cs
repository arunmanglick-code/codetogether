using System;
using System.ComponentModel;
using CTLS.ICLS.DX;

namespace CTLS.ICLS.UX.CountySearch
{
    public class BaseBindableModel : INotifyPropertyChanged
    {      
        #region .ctor
        /// <summary>
        /// Constructor
        /// </summary>
        public BaseBindableModel()
        {            
        }
        #endregion

        #region Properties

        /// <summary>
        /// This Holds and Returns HeaderInfo
        /// </summary>
        public HeaderInfo HeaderInfo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ErrorMessage { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string OrderCompletedBy { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string OrderCompletedOn { get; set; }
                        
        #endregion
        
        #region Property Changed Implementation

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Proprty Changed Then Notify Event Handler
        /// </summary>
        /// <param name="info">String</param>
        /// <returns>void</returns>
        protected void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        #endregion
    }
}
