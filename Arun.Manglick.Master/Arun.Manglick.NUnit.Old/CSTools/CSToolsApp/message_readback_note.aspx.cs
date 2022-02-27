#region File History

/******************************File History***************************
 * File Name        : message_readback_note.aspx.cs
 * Author           : SSK
 * Created Date     : 05/10/2007
 * Purpose          : Opens the popup window to enter the note for Rejecting Readback.
 *                  : 
 *                  :

 * *********************File Modification History*********************

 * Date(mm-dd-yyyy) Developer Reason of Modification

 * ------------------------------------------------------------------- 
 *  07-18-2007      IAK          Changes w.r.t Department integration
 *  27-12-2007      ZNK          Added Message Readback - Accept Readback for CSTools
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
    public partial class message_readback_note : System.Web.UI.Page
    {
        #region Page Variables
        private int readBackID = 0;
        private int messageID;
        private int isDeptMsg = 0;
        private int userID = 0;
        private int isLabMessage = 0;
        private int groupID = 0;
        #endregion

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
                butSave.Attributes.Add("onclick", "return validateText();");
                readBackID = Convert.ToInt32(Request["ReadBackID"]);
                messageID = Convert.ToInt32(Request["MessageID"]);
                isDeptMsg = Convert.ToInt32(Request["IsDeptMsg"]);
                groupID = Convert.ToInt32(Request["GroupID"]);
                isLabMessage = 0; // Radiology message has Readbacks

                butCancel.Attributes.Add("onClick", "onCancel();");
            }
            catch (Exception objException)
            {
                Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("message_readback_note - Page_Load", userID.ToString(), objException.Message, objException.StackTrace), userID);
                throw objException;
            }
        }

        /// <summary>
        /// Inserts the record in the Message readback table with status as rejected with reason for rejecting,
        /// inserts note in MessageNotes table
        /// calls the method to update Notification history table.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void butSave_Click(object sender, EventArgs e)
        {
            MessageDetails objMessageDetails;
            try
            {
                //[08.12.2006, Nitin]removed code to update readback as the readback is updated from Notifier service and row 
                // is inserted in notificationhistories also. Send readback rejected notification to OC
                StringBuilder myScript = new StringBuilder();

                int retVal = 0;
                if(isDeptMsg == 0)
                    retVal = Utils.InsertNotificationForReadback(readBackID, messageID, false, isDeptMsg);

                if(retVal == 0)
                {
                    objMessageDetails = new MessageDetails();
                    objMessageDetails.MarkReadbackAcceptReject(readBackID, 0, 1, txtReadbackNote.Text.Trim(), isDeptMsg);
                    objMessageDetails.InsertMessageNote(messageID, isDeptMsg, Session[SessionConstants.LOGGED_USER_NAME].ToString(), txtReadbackNote.Text, groupID, isLabMessage, MessageNoteType.Default);

                    myScript.Append(" try{ alert('Readback Rejected! You should contact the Ordering Clinician directly to resolve any outstanding communications. No more notifications will be sent and this message is now closed.'); \n");
                }
                else
                {
                    myScript.Append("alert('Unable to send notification.'); \n");
                }
                myScript.Append("window.returnValue = '1'; \n ");
                myScript.Append("window.close(); \n");
                myScript.Append("} catch(e){}");

                if (!ClientScript.IsStartupScriptRegistered("CloseReadbackNote"))
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "CloseReadbackNote", myScript.ToString(), true);
                }
                myScript = null;
            }
            catch (Exception objException)
            {
                Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("message_readback_note - butSave_Click", userID.ToString(), objException.Message, objException.StackTrace), userID);
                throw objException;
            }
            finally
            {
                objMessageDetails = null;
            }
        }
    }
}
