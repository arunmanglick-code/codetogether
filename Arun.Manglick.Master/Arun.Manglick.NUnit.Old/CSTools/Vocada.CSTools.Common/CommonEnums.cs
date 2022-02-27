#region File History

/******************************File History***************************
 * File Name        : CommonEnums.cs
 * Author           : Prerak Shah.
 * Created Date     : 22 Auguest 2007
 * Purpose          : Shows/Update grammar for all OC's of selected directory.
 *                  : 
 *                  :
 * *********************File Modification History*********************
 * Date(dd-mm-yyyy) Developer Reason of Modification
 * 18-10-2007   IAK     Defect 2128, 2130, 2135, 2136
 * 31-10-2007   Prerak  Added New Role for system admin and institution Admin  
 * 03-12-2007   IAK     DeviceType enum added
 * 18-12-2007   IAK     added Embargo in MessageStatus Enum
 * 08-02-2008   IAK     device added in NotificationDevice Enum
 * 27-02-2008   Suhas   TAP Pager Alpha implementation
 * 02-28-2008   Suhas   Added new User Roles to the UserRoles enum.
 * 03-06-2008 - IAK     Message Note NoteType Enum added
 * 13-03-2008   Suhas   Code Review Fixes.
 * 04-03-2008 - Prerak  Open Replies and Open Readback added for implementing CR#197
 * 04-11-2008 - Prerak  Added enum  SMS_WebLink and DesktopAlert in NotificationDevice 
 * 30-06-2008 - Suhas   CR# 256 - Support Monitoring Report Implementation.
 * 18 Mar-2009      SD       TTP #162 Amcom Pager changes done
 * ------------------------------------------------------------------- 
 */
#endregion
using System;
using System.Collections.Generic;
using System.Text;

namespace Vocada.CSTools.Common
{
    #region Enum
    /// <summary>
    /// Get Differnt type of veriphy users
    /// </summary>
    public enum UserRoles
    {
        Specialist = 1,
        GroupAdministrator = 2,
        GroupRepresentative = 3,
        LabTechnician = 4,
        UnitAdmin = 5,
        ChargeNurse = 6,
        Nurse = 7,
        LabGroupAdmin = 8,
        ReferringPhysician = 9,
        SystemAdmin = 10,
        InstitutionAdmin = 11,
        SupportLevel1 = 12,
        SupportLevel2 = 13,
        AccountServices = 14
    }

    /// <summary>
    /// This Enum used to get Notification Devices ID's
    /// </summary>
    public enum NotificationDevice : int
    {
        EMail = 1,
        SMS = 2,
        PagerAlpha = 3,
        Fax = 4,
        PagerNumRegular = 5,
        PagerNumSkyTel = 6,
        PagerNumUSA = 7,
        pagerPartner = 10,
        OutboundCallCB = 11,
        OutboundCallRS = 12,
        OutboundCallCI = 13,
        OutboundCallAS = 14,
        PagerTAP = 15,
        PagerTAPA = 16,
        SMS_WebLink = 17,
        DesktopAlert = 18,
        AmcomPager = 19,
        SelectAll = -1
    }


    /// <summary>
    /// This Enum used to get value for Message status for message cemter  
    /// </summary>
    public enum MessageStatus
    {
        Open = 0,
        RecentlyClose = 1,
        DirectCommunication = 2,
        Embargo = 3,
        All = 4,
        OpenReplies = 5,
        OpenReadBacks = 6,
        SupportMonitoringReport = 7
    }
    /// <summary>
    /// Device type
    /// </summary>
    public enum DeviceType : int
    {
        SELECT_ALL = -1,
        EMAIL = 1,
        CELL = 2,
        PAGER_ALPHA = 3,
        FAX = 4,
        PAGER_NUM_REG = 5,
        PAGER_NUM_SKYTELL = 6,
        PAGER_NUMU_PAGERUSA = 7,
        PAGER_PARTNER = 10,
        DESKTOP = 11,
        OUTBOUND_WITH_CALLBACK = 12,
        OUTBOUND_WITH_LAB_RESULT = 13
    }

    /// <summary>
    /// Get type of veriphy applications 
    /// </summary>
    public enum Module
    {
        VeriphyDesktop = 1,
        Veriphy = 2,
        VeriphyLabWeb = 3,
        CSTools = 4
    }

    /// <summary>
    /// Message Note Type
    /// </summary>
    public enum MessageNoteType : int
    {
        /// <summary>
        /// Default for narmal note
        /// </summary>
        Default = 1,
        /// <summary>
        /// Note added for forwarded message
        /// </summary>
        Forwarded = 2
    }
    #endregion
}
