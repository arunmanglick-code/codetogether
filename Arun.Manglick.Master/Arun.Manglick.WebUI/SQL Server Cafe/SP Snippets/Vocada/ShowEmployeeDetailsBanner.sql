CREATE PROCEDURE [dbo].[pr_AdminModule_AddEditData_ShowEmployeeDetls_Banner]
@Orgid        int ,
@EmpId    varchar(50)


AS

If( Exists (Select  1 From bEmployee Where Employee_ID = @EmpId) and (@EmpId not like '0%'))
Begin
Select     
    employee_ID            = ltrim(rtrim(isnull(b.employee_ID, '0'))) ,
    employee_Name        = ltrim(rtrim(isnull(employee_Firstname_display,'')))+' '+ltrim(rtrim(isnull(employee_LastName_display,''))),
    employee_vita_link        = ltrim(rtrim(isnull(employee_vita_link,''))) ,
    employee_picture_link        = ltrim(rtrim(isnull(employee_picture_link,''))) ,
From
    bBanner_Persons    a    JOIN
    bEmployee         b    ON
    b.Employee_ID        = a.Banner_ID
Where
    a.Banner_ID        = @EmpId
Order by  employee_LastName_display
End
Else If (@EmpId = '0' )
Begin
    Select     
        employee_ID        = c.employee_ID ,
        employee_Name    = ltrim(rtrim(isnull(employee_LastName_display,'')))+' '+ltrim(rtrim(isnull(employee_Firstname_display,''))),
        employee_vita_link ,
        employee_picture_link ,  
    From
        vAcademic_Organization_Validation a    JOIN
        rFaculty_Academic_Organization  b    ON
		a.academic_organization= b.academic_organization    JOIN
        bEmployee  c                ON
        b.Employee_ID        = c.Employee_ID
    Where
        a.academic_organization    = @OrgId
    Order by  employee_LastName_display
End
else
Begin
    Select     
        employee_ID            = ltrim(rtrim(isnull(a.Banner_ID,'0'))) ,
        employee_Name        = ltrim(rtrim(isnull(a.Banner_Name_Last,'')))+' '+ltrim(rtrim(isnull(a.Banner_Name_First,''))) ,  
        employee_vita_link        = '' ,
        employee_picture_link        ='' ,  
    From
        bBanner_Persons a
    Where
        a.Banner_Id    = @EmpId
Order by  employee_Name
End

GO