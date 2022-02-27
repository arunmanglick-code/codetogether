
#region File History

/******************************File History***************************
 * File Name        : Vocada.Veriphy.BusinessClasses/Subscriber.cs
 * Author           : Suhas Tarihalkar
 * Created Date     : 21-Feb-08
 * Purpose          : This class gives details of role task. TaskCollection Class used to create collection of task, for logged in user.
 *                  : 
 *                  :

 * *********************File Modification History*********************

 * Date(mm-dd-yyyy) Developer Reason of Modification

 * ------------------------------------------------------------------- 
 *                          
 */

#endregion

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Vocada.CSTools.DataAccess
{
    public class Task
    {
        #region Private Members
        private int taskID;
        private string taskKey;
        #endregion Private Members

        #region Property
        /// <summary>
        ///  Get or Set Task ID
        /// </summary>
        public int TaskID
        {
            get
            {
                return taskID;
            }
            set
            {
                taskID = value;
            }
        }

        /// <summary>
        ///  Get or Set Task Key
        /// </summary>
        public string TaskKey
        {
            get
            {
                return taskKey;
            }
            set
            {
                taskKey = value;
            }
        }

        #endregion Property
    }

    public class CollectionTask 
    {
        #region Private Member
        private Collection<Task> taskCollection = new Collection<Task>();
        #endregion Private Member

        #region Property
        /// <summary>
        /// Get or Set Collection of Tasks
        /// </summary>
        public Collection<Task> Items
        {
            get
            {
                return taskCollection;
            }
            set
            {
                taskCollection = value;
            }
        }
        #endregion Property
        
    }

}
