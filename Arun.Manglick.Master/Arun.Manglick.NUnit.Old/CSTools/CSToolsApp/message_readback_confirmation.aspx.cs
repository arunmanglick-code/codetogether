#region File History

/******************************File History***************************
 * File Name        : message_readback_confirmation.aspx.cs
 * Author           : SSK
 * Created Date     : 05/10/2007
 * Purpose          : Opens the popup window for Readback confirmation.
 *                  : 
 *                  :

 * *********************File Modification History*********************

 * Date(mm-dd-yyyy) Developer Reason of Modification

 * ------------------------------------------------------------------- 
 *  07-18-2007      IAK          Changes w.r.t Department integration
 *  27-12-2007      ZNK          Added Message Readback - Reject Readback : add note for CSTools
 *  03-03-2008      IAK          Message Note: New column createdBySystem added
 *  03-06-2008      IAK          Message Note: insertMessageNote() parameter NoteType passed
 */
#endregion

#region Using block
using System;
using System.Text;
using System.Web.UI.WebControls;
using Vocada.CSTools.DataAccess;
using Vocada.CSTools.Common;
using Vocada.VoiceLink.Utilities;
using Vocada.Veriphy;
#endregion

namespace Vocada.CSTools
{
    /// <summary>
    /// This class takes care all the functionality to opens the popup window for Readback confirmation in case of Radiology.
    /// </summary>
    public partial class message_readback_confirmation : System.Web.UI.Page
    {
        #region Page Variables
        private int readBackID;
        private int messageID;
        private string voiceURL;
        private int isDeptMsg = 0;
        private int userID = 0;
        private int isLabMessage = 0;
        private int groupID = 0;
        #endregion

        #region Public and Protected methods and events
        /// <summary>
        /// This event will set the values of control on Page load.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session[SessionConstants.LOGGED_USER_ID] == null)
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "RefreshParent", "window.parent.document.location.reload(true);", true);
                
                userID = Convert.ToInt32(Session[SessionConstants.LOGGED_USER_ID].ToString());
                readBackID = Convert.ToInt32(Request["ReadBackID"]);
                messageID = Convert.ToInt32(Request["MessageID"]);
                isDeptMsg = Convert.ToInt32(Request["IsDeptMsg"]);
                groupID = Convert.ToInt32(Request["GroupID"]);
                isLabMessage = 0; // Radiology message has Readbacks
                hidMessageID.Value = messageID.ToString();
                voiceURL = Request["VoiceURL"].ToString();
                hidReadbackID.Value = readBackID.ToString();

                hidVoiceURL.Value = voiceURL;
                hidDeptMsg.Value = isDeptMsg.ToString();
            }
            catch (Exception objException)
            {
                Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("message_readback_confirmation - Page_Load", userID.ToString(), objException.Message, objException.StackTrace), userID);
                throw objException;
            }
        }

        /// <summary>
        /// Inserts the record in the Message readback table with status as accepted,
        /// inserts note in MessageNotes table
        /// calls the method to update Notification history table.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void butAccept_Click(object sender, EventArgs e)
        {
            MessageDetails objMessageDetails;
            try
            {
                //[08.12.2006, Nitin]removed code to update readback as the readback is updated from Notifier service and row 
                //is inserted in notificationhistories also. Send notification to OC
                StringBuilder myScript = new StringBuilder();
                int retVal = 0;
                if (isDeptMsg == 0)
                    retVal = Utils.InsertNotificationForReadback(readBackID, messageID, true, isDeptMsg);

                if (retVal == 0)
                {
                    objMessageDetails = new MessageDetails();
                    objMessageDetails.MarkReadbackAcceptReject(readBackID, '1', '0', string.Empty, isDeptMsg);
                    objMessageDetails.InsertMessageNote(messageID, isDeptMsg, Session[SessionConstants.LOGGED_USER_NAME].ToString(), "Readback Accepted", groupID, isLabMessage, MessageNoteType.Default);
                }
                else
                {
                    myScript.Append("alert('Unable to send notification.'); \n");
                }

                myScript.Append("window.close(); \n");

                //Close this window
                if (!ClientScript.IsStartupScriptRegistered("CloseReadbackConfirm"))
                {
                    Utils.RegisterJS("CloseReadbackConfirm", myScript.ToString(), this);
                }
                myScript = null;
            }
            catch (Exception objException)
            {
                Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("message_readback_confirmation - butAccept_Click", userID.ToString(), objException.Message, objException.StackTrace), userID);
                throw objException;
            }
            finally
            {
                objMessageDetails = null;
            }
        }
        #endregion
    }
}