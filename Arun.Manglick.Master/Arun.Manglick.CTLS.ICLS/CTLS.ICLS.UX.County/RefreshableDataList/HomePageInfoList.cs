using System.Collections.Generic;
using CT.SLABB.Data;
using System.Linq;
using CTLS.ICLS.DX;
using System.Windows;
using System;
using System.ComponentModel;

namespace CTLS.ICLS.UX.CountySearch
{
    public class HomePageInfoList : Refreshable<HeaderInfo>, INotifyPropertyChanged
    {
        private int __countyCode;
        private HeaderInfo _filteredData;
        private List<string> _filterCountySearchInstructions;
       
        #region .ctor
        protected override void BeginRefresh()
        {
            HeaderInfoRequest homePageInfoRequest = new HeaderInfoRequest();
            HeaderInfoProxy homePageInfoProxy = new HeaderInfoProxy();

            List<string> countySearchInstructions = null;
            countySearchInstructions = new List<string> { "Use 'begins with' search logic.", 
                                                                       "Spaces are NOT disregarded.", 
                                                                       "May/May not use wildcard." ,
                                                                       "Images may/may not be free." ,
                                                                       "May/May not use wildcard." ,
                                                                       "Images may/may not be free." ,
                                                                       "May/May not use wildcard." ,
                                                                       "Images may/may not be free." ,
                                                                       "May/May not use wildcard." ,
                                                                       "Images may/may not be free." 
                                                        };

            List<string> lienTypes = new List<string> { "Grantor/Grantee Index",
                                                        "Fixture Filings",
                                                        "Federal Tax Liens",
                                                        "State Tax Liens",
                                                        "Judgement Liens" };


            List<string> copyCostInstrunctions = new List<string> { "Use 'begins with' search logic.", 
                                                                       "Spaces are NOT disregarded.", 
                                                                       "May/May not use wildcard." ,
                                                                       "Images may/may not be free." 
            };


            CopyCost objCopyCost = new CopyCost();
            objCopyCost.Cost = 25;
            objCopyCost.CopyCostInstrunctions = copyCostInstrunctions;

            HomePageInfo obj = new HomePageInfo();
            obj.CountySearchInstructions = countySearchInstructions;
            obj.LienType = lienTypes;
            obj.CopyCost = objCopyCost;

            this.FilteredData = obj;
            this.RefreshDone(obj);
        }
        #endregion
        
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public int CountyCode
        {
            get
            {
                return __countyCode;
            }
            set
            {
                this.__countyCode = value;

                if (value > 11)
                {
                    //List<String> lstFilter = this.FilteredData.CountySearchInstructions.Where(s => s.StartsWith("M")).ToList();
                    this.FilteredData.CountySearchInstructions = this.Data.CountySearchInstructions.Where(s => s.StartsWith("M")).ToList(); ;
                    //this.FilteredData = this.Data;

                    //this.Refresh();
                }
                if (value == 11)
                {
                    //List<String> lstFilter = this.FilteredData.CountySearchInstructions.Where(s => s.StartsWith("M")).ToList();
                    this.FilteredData.CountySearchInstructions = this.Data.CountySearchInstructions;
                    //this.FilteredData = this.Data;

                    //this.Refresh();
                }
            }
        }
        

        /// <summary>
        /// 
        /// </summary>
        public HomePageInfo FilteredData
        {
            get { return _filteredData; }
            set { _filteredData = value; this.NotifyPropertyChanged("FilteredData"); }
        }

        /// <summary>
        /// 
        /// </summary>
        public List<string> CountySearchInstructions
        {
            get { return _filterCountySearchInstructions; }
            set { _filterCountySearchInstructions = value; this.NotifyPropertyChanged("CountySearchInstructions"); }
        }

        #endregion

        #region Property Changed Implementation

        public event PropertyChangedEventHandler PropertyChanged;

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
