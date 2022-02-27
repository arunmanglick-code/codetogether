#region File History

/******************************File History***************************
 * File Name        : Constants.cs
 * Author           : Indrajeet K
 * Created Date     : 20 Apr 2007
 * Purpose          : This class class is used to declare constant which will be used thr out project
 *                  : 
 *                  :

 * *********************File Modification History*********************

 * Date(mm-dd-yyyy) Developer Reason of Modification

 * -------------------------------------------------------------------   
 * 05-03-2008       ZNK      Added 3 VIS-DS reports const string for Exception Data, New Records, and Updates Reports.
 * 09-10-2008   Prerak  Added constants for Device Address and Email Gateway.
 * 
 */
#endregion

#region Using
using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
#endregion

namespace Vocada.CSTools
{
    /// <summary>
    /// This class has constant which will be used thr out project
    /// </summary>
    public class Constants
    {
        #region Constants
        public const string RPT_CLINICIAN_PROFILE = "ClinicianProfileReport";
        public const string RPT_COMPLIANCE_STAT = "ComplianceStatisticalReport";
        public const string RPT_DETAILED_CANDS = "DetailedReportForCandS";
        public const string RPT_NUM_MSG_BY_MONTH_GRAPH = "NumMsgByMnthByCatGraph";
        public const string RPT_PHY_DIR_MSG_ACTIVITY_LOG = "PhyDirMsgActivityLogDetails";
        public const string RPT_PHY_DIR_MSG_COUNT_DETAIL = "PhyDirMsgCountDetailReport";
        public const string RPT_PHY_DIR_MAIN = "PhyDirReportMain";
        public const string RPT_VISDS_EXCEPTION_DATA = "VOC_VIS_ExceptionDataReport";
        public const string RPT_VISDS_RECORD_UPDATES = "VOC_VIS_RecordUpdatesReport";
        public const string RPT_VISDS_NEW_RECORDS = "VOC_VIS_NewDataReport";
        public const string RPT_SUMMARY = "SummaryReport";
        public const string RPT_WITHIN_COMPLIANCE = "WithinComplGoalGraph";
        public const string RPT_DATA_SOURCE = "Voicelink";
        public const string RPT_SERVICE_NAME = "/ReportServer/ReportService.asmx";
        public const string RPT_PDF = "PDF";
        public const string RPT_PDF_CONTENT_TYPE = "application/pdf";
        public const string RPT_XLS = "EXCEL";
        public const string RPT_HTML_VERSION_4 = "HTML4.0";
        public const string RPT_HTML_CONTENT_TYPE = "text/html";
        public const string RPT_HTML = "HTML";
        public const string RPT_XLS_CONTENT_TYPE = "application/vnd.ms-excel";

        public const string DEVICE_ADDRESS = "DiviceAddress";
        public const string EMAIL_GATEWAY = "EmailGateway";

        #endregion Constants

    }
}