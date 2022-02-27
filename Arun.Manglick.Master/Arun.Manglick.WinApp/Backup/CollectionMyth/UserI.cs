using System;
using System.Data;
using System.Configuration;


namespace WindowsApplication1.CollectionMyth
{
    /// <summary>
    /// Summary description for UserI
    /// </summary>
    public class UserI
    {
        #region Private Variables

        private string _au_id;
        private string _au_fname;
        

        #endregion

        #region Public Properties

        public string Id
        {
            get
            {
                return _au_id;
            }
            set
            {
                _au_id = value;
            }
        }
        public string Fname
        {
            get
            {
                return _au_fname;
            }
            set
            {
                _au_fname = value;
            }
        }
        

        #endregion
    }
}
