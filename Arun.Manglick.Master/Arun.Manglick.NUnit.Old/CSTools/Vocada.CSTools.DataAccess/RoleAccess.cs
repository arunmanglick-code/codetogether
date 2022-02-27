#region File History

/******************************File History***************************
 * File Name        : Vocada.CSTools.DataAccess/RoleAccess.cs
 * Author           : Suhas Tarihalkar
 * Created Date     : 21-Feb-08
 * Purpose          : To get role acces for logged in user
 *                  : 
 *                  
 * *********************File Modification History*********************

 * Date(mm-dd-yyyy) Developer Reason of Modification

 * ------------------------------------------------------------------- 
 *                          
 */
#endregion


using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Vocada.VoiceLink.DataAccess;
using Vocada.CSTools.Common;

namespace Vocada.CSTools.DataAccess
{
    public class RoleAccess
    {
        #region  Constants
        /// <summary>
        /// This constant holding name of stored procedure which retrives role access information for gien role
        /// </summary>
        private const string SP_GET_ROLE_ACCESS = "dbo.VOC_CST_getRoleAccess";
        /// <summary>
        /// This constant stores name of stored procedure which will add new record for nurse shift assignment in Database
        /// </summary>
        private const string ROLE_ID = "@RoleID";
        /// <summary>
        /// This constant stores name of stored procedure which will add new record for nurse shift assignment in Database
        /// </summary>
        private const string MODULE_ID = "@ModuleID";
        #endregion  Constants

        #region Private Members
        private CollectionTask taskCollection = new CollectionTask();
        #endregion Private Members

        #region Property
        /// <summary>
        /// Get Collection of Tasks assign for given role
        /// </summary>
        public CollectionTask Tasks
        {
            get
            {
                return taskCollection;
            }
        }
        #endregion Property

        #region Constructor
        /// <summary>
        /// Get Tasks assigned for given role and moduleID
        /// </summary>
        /// <param name="roleID"></param>
        /// <param name="moduleID"></param>
        public RoleAccess(int roleID)
        {
            getTasks(roleID);
        }
        #endregion Constructor

        #region Private Methods
        /// <summary>
        /// Fill the task collection for given role and moduleID
        /// </summary>
        /// <param name="roleID"></param>
        /// <param name="moduleID"></param>
        private void getTasks(int roleID)
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                SqlParameter[] objSqlParameter = new SqlParameter[1];

                objSqlParameter[0] = new SqlParameter(ROLE_ID, SqlDbType.Int);
                objSqlParameter[0].Value = roleID;

                SqlDataReader drTasks = SqlHelper.ExecuteReader(cnx, CommandType.StoredProcedure, SP_GET_ROLE_ACCESS, objSqlParameter);
                
                Task task;
                while (drTasks.Read())
                {
                    task = new Task();
                    task.TaskID = drTasks.GetInt32(drTasks.GetOrdinal("TaskID"));
                    task.TaskKey = drTasks.GetString(drTasks.GetOrdinal("TaskKey"));
                    taskCollection.Items.Add(task);
                }
                drTasks.Close();
            }
        }
        #endregion Private Methods
    }
}
