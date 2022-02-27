CREATE PROCEDURE [dbo].[pr_AdminModule_Report_SearchEvent]
@EventDate    varchar(10)
AS

Select  
    drpdntext    =Convert( varchar(10),Event_ID) +'  -  '+  ltrim(rtrim(event_title))+ ' at   ' + isnull(ltrim(rtrim(event_start_time)),'') ,
    drpdnValue    = Event_ID
from bEvent_Calendar
where Convert(datetime,@EventDate,101)  between event_start_date and  event_end_date and event_Schedule = 0

UNION

Select     
    drpdntext    =Convert( varchar(10),Event_ID) +'  -  '+  ltrim(rtrim(event_title))+ ' at   ' + isnull(ltrim(rtrim(event_start_time)),'') ,
    drpdnValue    = Event_ID
from bEvent_Calendar
where Convert(datetime,@EventDate,101)  between event_start_date and  event_end_date and
datepart(dw,@EventDate) = event_Schedule_WeekDay and event_Schedule = 1

UNION  -- AM (Added by Me to handle the DeActivated Non_Weekly Events)

Select     
    drpdntext    =Convert( varchar(10),Event_ID) +'  -  '+  ltrim(rtrim(event_title))+ ' at   ' + isnull(ltrim(rtrim(event_start_time)),'') ,
    drpdnValue    = Event_ID
from bEvent_Calendar
where Convert(datetime,@EventDate,101)  between event_start_date and  event_end_date and  event_Schedule =2

UNION  -- AM (Added by Me to handle the DeActivated Weekly Events)

Select     
    drpdntext    =Convert( varchar(10),Event_ID) +'  -  '+  ltrim(rtrim(event_title))+ ' at   ' + isnull(ltrim(rtrim(event_start_time)),'') ,
    drpdnValue    = Event_ID
from bEvent_Calendar
where Convert(datetime,@EventDate,101)  between event_start_date and  event_end_date and
datepart(dw,@EventDate) = event_Schedule_WeekDay and event_Schedule = 3

GO