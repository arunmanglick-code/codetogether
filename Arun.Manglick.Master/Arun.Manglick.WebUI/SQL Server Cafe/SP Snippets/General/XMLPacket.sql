


--Created By VT 16Jan06
--For General/Host  Modification
CREATE     PROCEDURE GetXMLPacket
@Panelid int ,
@TimeDiff int ,
@ChangeDate datetime

AS

Declare @RecCount int
    select @RecCount =count(*) from syscolumns where id IN (select id from sysobjects where [name]= 'Reservations' ) and [name] = 'General/Host'

if (@RecCount > 0 )

begin
declare @SelectStat Nvarchar(900)
declare @NPanelID Nvarchar(5)
declare @NTimeDiff Nvarchar(5)
declare @NChangeDate Nvarchar(20)
--Changed By VT 27-01-06 For Multi Day Reservation
select @selectStat = 'select Room_ID, Reservation_ID ReservationID, [General/Host] HostName,
case
when (convert(datetime, convert(nvarchar(10),[Actual Start],112),112)< @changeDate)
then 0 else DatePart(hh, [Actual Start])+ @TimeDiff
end
as BookedStartTimeHour,
case
when (convert(datetime, convert(nvarchar(10),[Actual Start],112),112)< @changeDate)
then 0 else DatePart(mi, [Actual Start])
end
as BookedStartTimeMinutes,
case
when (convert(datetime, convert(nvarchar(10),[Actual Start],112),112)< @changeDate)
then DateDiff(mi,@changeDate,[Actual End]) else DateDiff(mi,[Actual Start],[Actual End])
end
as BookedDurationMinutes
from LCDPanel_RoomList A, reservations B
where A.Rm_RoomId=B.Room_ID
and A.Rm_PanelId=@PanelId
and convert(datetime, convert(nvarchar(10),[Actual Start],112),112)<= @ChangeDate
and convert(datetime, convert(nvarchar(10),[Actual End],112),112)>= @ChangeDate'

--and convert(nvarchar(10),[Actual Start],112)= convert(nvarchar(10),convert(datetime,@ChangeDate) ,112)  '


select @NPanelID = convert(Nvarchar(5), @Panelid), @NTimeDiff = convert(Nvarchar(5), @TimeDiff), @NChangeDate = convert(Nvarchar(20), @ChangeDate)

Execute  sp_executesql @SelectStat,N'@PanelID INT, @TimeDiff INT, @changeDate DATETIME', @NPanelID, @NTimeDiff,@NChangeDate
end

else

begin
--Changed By VT 27-01-06 For Multi Day Reservation
select Room_ID, Reservation_ID ReservationID, '' HostName,
case
when (convert(datetime, convert(nvarchar(10),[Actual Start],112),112)< @changeDate)
then 0 else DatePart(hh, [Actual Start])+ @TimeDiff
end
as BookedStartTimeHour,
case
when (convert(datetime, convert(nvarchar(10),[Actual Start],112),112)< @changeDate)
then 0 else DatePart(mi, [Actual Start])
end
as BookedStartTimeMinutes,
case
when (convert(datetime, convert(nvarchar(10),[Actual Start],112),112)< @changeDate)
then DateDiff(mi,@changeDate,[Actual End]) else DateDiff(mi,[Actual Start],[Actual End])
end
as BookedDurationMinutes
from LCDPanel_RoomList A, reservations B
where  A.Rm_RoomId=B.Room_ID and A.Rm_PanelId=@PanelId
and convert(datetime, convert(nvarchar(10),[Actual Start],112),112)<= @ChangeDate
and convert(datetime, convert(nvarchar(10),[Actual End],112),112)>= @ChangeDate
        
--and convert(nvarchar(10),[Actual Start],112)= convert(nvarchar(10),convert(datetime,@ChangeDate) ,112)

end




GO

Send instant messages to your online friends http://in.messenger.yahoo.com

Stay connected with your friends even when away from PC. Link: http://in.mobile.yahoo.com/new/messenger/ 