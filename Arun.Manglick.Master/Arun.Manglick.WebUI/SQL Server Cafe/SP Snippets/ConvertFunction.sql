Convert Date to String:      convert(nvarchar(10),[Actual Start],112)
Convert String to Datetime : convert(datetime, '21-June-06',112)
Both Together : (convert(datetime, convert(nvarchar(10),[Actual Start],112),112))

Convert int to String :  convert(Nvarchar(5), @Panelid)