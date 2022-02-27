#region File History

/******************************File History***************************
 * File Name        : RoleTask.cs
 * Author           : Prerak Shah.
 * Created Date     : 10 July 07
 * Purpose          : Asssign Task to a Role.
 *                  : 
 *                  :

 * *********************File Modification History*********************

 * * Date(dd-mm-yyyy) Developer Reason of Modification
 *   03-12-2007 - Prerak - Call getOpenConnection function from Utility Class.
 *   03-12-2007 - IAK    - Added new method GetRolesForGroup
 * ------------------------------------------------------------------- 
 */
#endregion
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections ;
using Vocada.CSTools.Common;
using Vocada.VoiceLink.DataAccess;
namespace Vocada.CSTools.DataAccess
{
    public class RoleTask
    {
        #region  Constants

        /// <summary>
        /// This constant stores name of stored procedure which will get the list of Roles from Database
        /// </summary>
        private const string SP_GET_ROLES = "dbo.VOC_CST_getRoles";
        /// <summary>
        /// This constant stores name of stored procedure which will get the list of Roles from Database
        /// </summary>
        private const string SP_GET_ROLES_FOR_GROUP = "dbo.VOC_CST_getRolesForGroup";
        /// <summary>
        /// This constant stores name of stored procedure which will get the information of institution from Database
        /// </summary>
        private const string SP_GET_TASK_BY_ROLE = "dbo.Voc_getTaskByUserRole";
        /// <summary>
        /// This constant stores name of stored procedure which will update Task assigned to perticular Role in Database
        /// </summary>
        private const string SP_INSERT_TASK_FOR_ROLE = "dbo.VOC_CST_insertTaskForRole";
        /// <summary>
        /// This constant stores name of stored procedure which will get the unassigned Task for perticular Role.
        /// </summary>
        private const string SP_GET_UNASSIGNED_TASKS = "dbo.VOC_CST_getUnassingedTasks";
        /// <summary>
        /// This constant stores name of stored procedure which will delete Tasks of perticular Role from Database
        /// </summary>
        private const string SP_DELETE_TASKS_FOR_ROLE = "dbo.VOC_CST_deleteTasksForRole";
        /// <summary>
        /// This constant stores name of stored procedure which will update Role of Description in Database
        /// </summary>
        private const string SP_UPDATE_ROLE = "dbo.VOC_CST_updateRole";


        
        #endregion

        #region  Private Variables
        private const string ROLE_ID = "@roleID";
        private const string ROLE_DESCRIPTION = "@roleDescription";
        private const string TASK_ID = "@taskID";
        private const string TASK_DESCRIPTION = "@taskDescription";
        private const string TASK_LIST = "@assignTask";
        private const string GROUP_ID = "@groupID";
     
        #endregion 

        #region PublicMethods
        /// <summary>
        /// Gets Roles 
        /// </summary>
        /// <returns>DataTable containing Role id and description</returns>
        public DataTable GetRoles()
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                DataTable dtRoles = new DataTable();
                SqlDataReader drRoles = SqlHelper.ExecuteReader(cnx, CommandType.StoredProcedure, SP_GET_ROLES);
                dtRoles.Load(drRoles);
                return dtRoles;
            }
        }

        /// <summary>
        /// Gets Roles for given group as it is lab/rad group
        /// </summary>
        /// <returns>DataTable containing Role id and description</returns>
        public DataTable GetRolesForGroup(int groupID)
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                DataTable dtRoles = new DataTable();
                SqlParameter[] arSqlParams = new SqlParameter[1];

                arSqlParams[0] = new SqlParameter(GROUP_ID, SqlDbType.Int);
                arSqlParams[0].Value = groupID;

                SqlDataReader drRoles = SqlHelper.ExecuteReader(cnx, CommandType.StoredProcedure, SP_GET_ROLES_FOR_GROUP, arSqlParams);
                dtRoles.Load(drRoles);
                return dtRoles;
            }
        }


        /// <summary>
        /// Get Assigned Tasks by Role id 
        /// </summary>
        ///<param name="RoleID">Role id</param>
        /// <returns>DataTable containing Task id and description and role id</returns>

        public DataTable GetAssignedTask(int roleID)
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                DataTable dtTask = new DataTable();
                SqlParameter[] arSqlParams = new SqlParameter[1];

                arSqlParams[0] = new SqlParameter(ROLE_ID, SqlDbType.Int);
                arSqlParams[0].Value = roleID;

                SqlDataReader drTask = SqlHelper.ExecuteReader(cnx, CommandType.StoredProcedure, SP_GET_TASK_BY_ROLE,arSqlParams);
                dtTask.Load(drTask);
                return dtTask;
            }
        }

        /// <summary>
        /// Gets available Tasks to assign for Role
        /// </summary>
        /// <param name="RoleID"></param>
        /// <returns></returns>
        public DataTable GetUnAssignedTasks(int roleID)
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                DataTable dtUnAssignedTasks = new DataTable();
                SqlParameter[] arSqlParams = new SqlParameter[1];

                arSqlParams[0] = new SqlParameter(ROLE_ID, SqlDbType.Int);
                arSqlParams[0].Value = roleID;

                SqlDataReader drUnAssignedTasks = SqlHelper.ExecuteReader(cnx, CommandType.StoredProcedure, SP_GET_UNASSIGNED_TASKS, arSqlParams);
                dtUnAssignedTasks.Load(drUnAssignedTasks);
                return dtUnAssignedTasks;
            }
        }

        public void UpdateRoleTask(Int32 roleID, string taskList)
        {
            SqlParameter[] arrParams = new SqlParameter[2];

            try
            {
                using(SqlConnection conn = Utility.getOpenConnection())
                {
                    arrParams[0] = new SqlParameter(ROLE_ID, SqlDbType.Int);
                    arrParams[0].Value = roleID;

                    arrParams[1] = new SqlParameter(TASK_LIST, SqlDbType.VarChar, 200);
                    arrParams[1].Value = taskList;
                   
                   SqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, SP_INSERT_TASK_FOR_ROLE, arrParams);                
                }
            }
            catch(Exception ex)
            {
                throw;
            }
            finally
            {
                arrParams = null;
            }
        }

        /// <summary>
        /// Delete Tasks 
        /// </summary>
        /// <param name="sqlTransaction"></param>
        /// <param name="RoleID"></param>
        /// <returns></returns>
        public int DeleteRoleTasks(SqlTransaction sqlTransaction, int roleID)
        {
            try
            {
                SqlParameter[] arrParams = new SqlParameter[1];

                arrParams[0] = new SqlParameter(ROLE_ID, SqlDbType.Int);
                arrParams[0].Value = roleID;

                return SqlHelper.ExecuteNonQuery(sqlTransaction, CommandType.StoredProcedure, SP_DELETE_TASKS_FOR_ROLE , arrParams);
            }
            catch
            {
                throw;
            }
        }

        public void UpdateRole(int roleID, string roleDesc)
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                SqlParameter[] arSqlParams = new SqlParameter[2];

                arSqlParams[0] = new SqlParameter(ROLE_ID, SqlDbType.Int);
                arSqlParams[0].Value = roleID;

                arSqlParams[1] = new SqlParameter(ROLE_DESCRIPTION, SqlDbType.VarChar);
                arSqlParams[1].Value = roleDesc;

                SqlHelper.ExecuteNonQuery(cnx, CommandType.StoredProcedure, SP_UPDATE_ROLE, arSqlParams);

            }
        }
        #endregion
    }
}
