IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'VOC_DN_SaveAgentDeviceInformation')
	BEGIN
		DROP PROCEDURE VOC_DN_SaveAgentDeviceInformation
		PRINT 'Droped Stored Procedure: dbo.VOC_DN_SaveAgentDeviceInformation'
	END
GO        
/******************************************************************************          
**  File: VOC_DN_SaveAgentDeviceInformation.sql          
**  Name: VOC_DN_SaveAgentDeviceInformation          
**  Desc: Saves Device Information        
**           
**  Called by: Device Notification popup        
**                        
**  Auth: Sudhir C         
**  Date: 08.June.09        
*******************************************************************************          
**  Change History          
*******************************************************************************          
**  Date:  Author:  Description:          
**  -------- -------- -------------------------------------------          
**  08.June.09 SDC   Created        
**	12.July.09 SDC   changed condition for finding duplicate device name
*******************************************************************************/            
        
CREATE PROCEDURE [dbo].[VOC_DN_SaveAgentDeviceInformation]        
@groupID INT,        
@deviceID INT = 0,        
@deviceTypeID INT,        
@deviceName VARCHAR(50),        
@carrier VARCHAR(100),        
@deviceAddress VARCHAR(100),        
@gateway VARCHAR(100),        
        
@notificationID INT = 0,        
@facilityID INT = -1,        
@findingID INT = 0,        
@eventID INT = -1,        
@initialPause decimal,        
@callCenterID INT = 0,        
@output INT OUTPUT        
        
AS              
 SET @output = 0        
 DECLARE @existingDeviceID INT        
 SET @existingDeviceID = 0       
      
 -- TRY TO FIND DUPLICATE DEVICE INFORMATION        
 SELECT @existingDeviceID = AgentdeviceID FROM VOC_ACC_AgentDevices        
 WHERE CallCenterID = @callCenterID AND DeviceID = @deviceTypeID         
   AND RTRIM(LTRIM(ISNULL(Carrier, ''))) = @carrier         
   AND RTRIM(LTRIM(ISNULL(Gateway, ''))) = @gateway          
   AND RTRIM(LTRIM(ISNULL(DeviceAddress, ''))) = @deviceAddress          
   AND CASE WHEN @deviceID = 0 THEN 1 ELSE CASE WHEN AgentdeviceID <> @deviceID THEN 1 ELSE 0 END END =1         
        
 IF @deviceID > 0 and LEN(@deviceName) = 0        
 BEGIN        
  SELECT @deviceName = DeviceName FROM VOC_ACC_AgentDevices WHERE AgentdeviceID = @deviceID         
 END        
        
 IF LEN(@deviceName) = 0        
 BEGIN        
        
  DECLARE @counter  int        
  DECLARE @deviceShortDescription varchar(50)        
        
  SELECT @counter = 1 + COUNT(*) FROM VOC_ACC_AgentDevices WHERE CallCenterID = @callCenterID AND DeviceID = @deviceTypeID      
  SELECT @deviceShortDescription = DeviceShortDescription FROM Devices WHERE DeviceID = @deviceTypeID        
        
  WHILE (SELECT COUNT(*) FROM VOC_ACC_AgentDevices WHERE DeviceName LIKE @deviceShortDescription + '_' + LTRIM(STR(@counter)) AND CallCenterID = @callCenterID AND DeviceID = @deviceTypeID) > 0         
  BEGIN        
   SELECT @counter = @counter + 1         
  END        
  SELECT @deviceName = @deviceShortDescription + '_' + LTRIM(STR(@counter))        
 END        
         
 IF (SELECT COUNT(*) FROM VOC_ACC_AgentDevices         
 WHERE CallCenterID = @callCenterID AND DeviceID = @deviceTypeID AND DeviceName = @deviceName         
 AND CASE WHEN @deviceID > 0 THEN CASE WHEN AgentdeviceID <> @deviceID THEN 1 ELSE 0 END ELSE 1 END =1        
 AND CASE WHEN @existingDeviceID > 0 THEN CASE WHEN AgentdeviceID = @existingDeviceID THEN 1 ELSE 0 END ELSE 1 END =1) > 0        
 BEGIN        
  SET @output = 1        
  RETURN        
 END        
         
 IF @existingDeviceID > 0         
 BEGIN        
          
  IF @deviceID > 0        
  BEGIN        
   -- IF DUPLICATE DEVICE FOUND AND DEVICE TO UPDATE,        
   -- UPDATE THE GROUP NOTIFICATIONS REFERENCE WITH NEW  GROUP DEVICE ID        
   -- DELETE THE CURRENT DEVICE        
   UPDATE VOC_ACC_AgentNotifications        
   SET AgentdeviceID = @existingDeviceID        
   WHERE AgentdeviceID = @deviceID        
           
   DELETE FROM VOC_ACC_AgentDevices WHERE AgentdeviceID = @deviceID        
  END        
  -- SET CURRENT DEVICE ID WITH MATCHED DEVICE ID        
  SELECT @deviceID = @existingDeviceID     
  UPDATE VOC_ACC_AgentDevices        
  SET         
   DeviceName = @deviceName        
  WHERE AgentdeviceID = @deviceID        
 END        
 ELSE        
 BEGIN        
  IF @deviceID = 0         
  BEGIN        
   -- INSERT NEW DEVICE        
   INSERT INTO VOC_ACC_AgentDevices (CallCenterID, DeviceID, DeviceName, DeviceAddress, Gateway, Carrier, InitialPause)        
   VALUES(@callCenterID, @deviceTypeID, @deviceName, @deviceAddress, @gateway, @carrier, @initialPause)        
        
   SELECT @deviceID = SCOPE_IDENTITY()        
  END        
  ELSE        
  BEGIN        
   -- UPDATE DEVICE INOFRMATION        
   UPDATE VOC_ACC_AgentDevices        
   SET         
    DeviceName = @deviceName,        
    DeviceAddress = @deviceAddress,        
    Gateway = @gateway,        
    Carrier = @carrier,      
 InitialPause = @initialPause      
   WHERE AgentdeviceID = @deviceID        
  END        
 END        
         
 IF @notificationID > 0         
 BEGIN        
  IF @eventID = 0 AND @facilityID = 0        
  BEGIN        
   DELETE FROM VOC_ACC_AgentNotifications WHERE AgentNotificationID = @notificationID        
   --DELETE FROM VOC_CTM_RuleNotificationDevices WHERE GroupNotificationID = @notificationID        
  END        
  ELSE IF @eventID <> 0        
  BEGIN         
   UPDATE VOC_ACC_AgentNotifications        
   SET         
    AgentdeviceID = @deviceID,        
    AgentNotifyEventID = @eventID,        
    FindingID = @findingID,        
    FacilityID = @facilityID      
 WHERE AgentNotificationID = @notificationID        
  END        
 END        
 ELSE        
 BEGIN        
  IF @eventID <> 0         
  BEGIN        
   INSERT INTO VOC_ACC_AgentNotifications(AgentNotifyEventID, AgentdeviceID, FindingID, FacilityID,GroupID)        
   VALUES(@eventID, @deviceID, @findingID, @facilityID,@groupID)        
  END        
 END 

GO