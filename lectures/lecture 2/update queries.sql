/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP (5000) [StudentId]
      ,[StudentName]
      ,[StudentEmail]
      ,[IsMale]
      ,[NickName]
  FROM [school].[dbo].[tblStudents]

  
  update tblStudents set StudentName='new student',StudentEmail='newemail@hotmail.com'

  select * from tblStudents where StudentId=0 and StudentName='new student' and StudentEmail='newemail@hotmail.com' and IsMale=1 and NickName is null

  select * from tblStudents where StudentId=45345

    update tblStudents set StudentName='student with unique student id',StudentEmail='awesomemail@hotmail.com' where StudentId=45345

	    update tblStudents set StudentName='this will update 7 records',StudentEmail='awesomemail@hotmail.com' where StudentId=0 and StudentName='new student' and StudentEmail='newemail@hotmail.com' and IsMale=1 and NickName is null


		    update tblStudents set StudentName='multiple students',StudentEmail='multiple@hotmail.com' where StudentId=45345 or StudentId=523412 or StudentId=1393995589

 update tblStudents set StudentName='multiple students',StudentEmail='multiple@hotmail.com' where StudentId in (45345,523412,1393995589) 

 --if you want to update, delete, select particular database records, then you have to have a unique identifier or a certain combination of multiple columns that uniquely identifies that particular record