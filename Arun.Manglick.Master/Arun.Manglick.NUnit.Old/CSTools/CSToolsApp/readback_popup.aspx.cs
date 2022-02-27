#region File History

/******************************File History***************************
 * File Name        : readback_popup.aspx.cs
 * Author           : SSK
 * Created Date     : 05/10/2007
 * Purpose          : Opens the popup window for Message Note.
 *                  : 
 *                  :

 * *********************File Modification History*********************

 * Date(mm-dd-yyyy) Developer Reason of Modification
 * 27-12-2007       ZK      Added readback accept-reject popup for CSTools.                                 
 * 
 * ------------------------------------------------------------------- 
 */
#endregion

#region Using Block
using System;
using System.Web.UI.WebControls;
#endregion

namespace Vocada.CSTools
{
	/// <summary>
	/// Opens the popup window for Message Note to enter reason for rejecting readback.
	/// </summary>
    public partial class readback_popup : System.Web.UI.Page
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
		}
		#endregion
	}
}
