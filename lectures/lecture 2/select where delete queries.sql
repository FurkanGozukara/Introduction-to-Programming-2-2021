select * from school.dbo.tblStudents

select tblStudents.StudentId from tblStudents

select StudentId,tblStudents.IsMale,'gg '+StudentName+CONVERT(varchar,StudentId) from tblStudents

select * from tblStudents where StudentId > 75213545

--select * from tblStudents where NickName != null this would return nothing because null can not be compared with null

select * from tblStudents where NickName is not null

select * from tblStudents where NickName is null

select * from tblStudents where StudentId >= 75213545 or NickName is not null

select * from tblStudents where StudentId >= 75213545 and NickName is not null

select * from tblStudents where StudentId >= 75213545 or NickName is not null and IsMale=1

select * from tblStudents where (StudentId >= 75213545 or NickName is not null) and IsMale=1

select * from tblStudents where StudentId=1114549066

delete from tblStudents where StudentId = 1114549066