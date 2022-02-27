CREATE PROCEDURE [dbo].[pr_AdminModule_AddEditData_ShowEventTickets]
@EventId    int
AS

create table #tTemp (event_Ticket_Id int , price decimal(10,2), qty int , descr varchar(255) , availableqty int 

Insert into #tTemp
Select
    event_Ticket1_Id,
    event_ticket1_price    = convert( decimal(10,2) , event_ticket1_price) ,
    event_ticket1_qoh,
    event_ticket1_desc,
    event_ticket1_AvailableQty
    
From bEvent_Tickets
Where
    Event_Id    = @EventId

Insert into #tTemp
select
    event_Ticket2_Id,
    event_ticket2_price     =convert(decimal(10,2) , event_ticket2_price),
    event_ticket2_qoh,
    event_ticket2_desc,
    event_ticket2_AvailableQty
    
From bEvent_Tickets
Where
    Event_Id    = @EventId

Insert into #tTemp
select
    event_Ticket3_Id,
    event_ticket3_price    = Convert(decimal(10,2) ,event_ticket3_price) ,
    event_ticket3_qoh ,
    event_ticket3_desc ,
    event_ticket3_AvailableQty
    
From bEvent_Tickets
Where
    Event_Id    = @EventId

select * from #tTemp Where availableqty > 0  order by event_Ticket_Id

drop table #tTemp



GO