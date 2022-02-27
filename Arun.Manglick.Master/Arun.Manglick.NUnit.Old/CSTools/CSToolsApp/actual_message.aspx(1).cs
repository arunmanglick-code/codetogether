#region File History

/******************************File History***************************
 * File Name        : actual_message.aspx.cs
 * Author           : Raju G
 * Created Date     : 20-Sep-2008
 * Purpose          : To provide UI to view the actual message sent to the recipient.
 
 * *********************File Modification History*********************

 * Date(mm-dd-yyyy) Developer Reason of Modification

 * ------------------------------------------------------------------- 

 */
#endregion

#region using

using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Vocada.CSTools.DataAccess;
using Vocada.VoiceLink.Utilities;

#endregion

namespace Vocada.CSTools
{
    /// <summary>
    /// Class to display actual message sent to rcipient on popup
    /// </summary>
    public partial class actual_message : System.Web.UI.Page
    {
        #region Page Events

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack && Request.QueryString.Count > 0)
                {
                    //Get the Notification History ID from QueryString Collection
                    string notificationHistoryIDText = Request.QueryString["NotificationHistoryID"];
                    string messageTypeText = Request.QueryString["MessageType"];
                
                    if (notificationHistoryIDText != null && notificationHistoryIDText.Trim().Length > 0
                        && messageTypeText != null && messageTypeText.Trim().Length > 0)
                    {
                        int notificationHistoryID = Convert.ToInt32(notificationHistoryIDText);
                        int messageType = Convert.ToInt32(messageTypeText);
                     
                        MessageDetails objMessageDetails = new MessageDetails();
                        //Get the Actual message Details
                        DataTable dtActualMessage = objMessageDetails.GetActualNotificationMessage(notificationHistoryID, messageType);
                        if (dtActualMessage.Columns.Contains("Subject") && dtActualMessage.Rows.Count > 0)
                        {
                            lblSubjectText.Text = dtActualMessage.Rows[0]["Subject"].ToString();
                        }

                        if (dtActualMessage.Columns.Contains("Body") && dtActualMessage.Rows.Count > 0)
                        {
                            lblBodyText.Text = dtActualMessage.Rows[0]["Body"].ToString();
                        }
                        dtActualMessage.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null && Session[SessionConstants.USER_ID].ToString().Length > 0)
                   Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("actual_message.Page_Load", Session[SessionConstants.USER_ID].ToString(), ex.Message, ex.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID].ToString()));
            }
        }

        #endregion
    }
}
