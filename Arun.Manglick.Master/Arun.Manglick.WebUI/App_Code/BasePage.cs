using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.IO;

namespace Arun.Manglick.UI
{
    /// <summary>
    /// BaseSitemap class:Manipulates the websitmap  with customised aapproch 
    /// </summary>
    /// <history created="Paresh B"></history>
    /// <history date="Nov 6, 2007"></history>
    public class BasePage : System.Web.UI.Page
    {
        protected Logger logger = null;

        #region BasePage Methods

        /// <summary>
        /// Constructor
        /// </summary>
        /// <history created="Paresh B"></history>
        /// <history date="Nov 6, 2007"></history>
        public BasePage()
        {            
            logger = Logger.CreateInstance();            
        }

        /// <summary>
        /// Takes a second call to achieve the security. First call is taken care by Web.config
        /// </summary>
        /// <param name="e"></param>
        /// <returns>void</returns>
        /// <history created="Arun M"></history>
        /// <history date="Nov 8, 2007"></history>
        protected override void OnLoad(EventArgs e)
        {
            try
            {
                if (SessionManager.SelectedNodes == null)
                {
                    SelectedNodes nodes = new SelectedNodes();
                    SessionManager.SelectedNodes = nodes;
                }
                base.OnLoad(e);
            }
            catch (Exception ex)
            {
                //
                // TODO: Logging exception code goes here
                //
            }
        }       

        #endregion

        #region Public Methods

        public void ReadTextStream(String pathName)
        {
            String result = String.Empty;
            StreamReader strmRead = null;
            if (File.Exists(pathName))
            {
                strmRead = new StreamReader(pathName); // Generic Approach
                //strmRead = File.OpenText(pathName1);  // TextFile Approach
                result = strmRead.ReadToEnd();
                strmRead.Close();

                SessionManager.NotePadText = result;
            }
        }

        #endregion
    }
}
