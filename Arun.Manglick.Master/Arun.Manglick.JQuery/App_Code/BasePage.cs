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
        #region BasePage Methods

        /// <summary>
        /// Constructor
        /// </summary>
        /// <history created="Paresh B"></history>
        /// <history date="Nov 6, 2007"></history>
        public BasePage()
        {
            //
            // TODO: Add constructor logic here
            //
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
                //if (!User.Identity.IsAuthenticated)
                //{
                //    Server.Transfer(Resources.AppLinks.Login,false);
                //}
                //else
                //{
                base.OnLoad(e);
                //}
            }
            catch (Exception ex)
            {
                //
                // TODO: Logging exception code goes here
                //
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

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
