
insert into tblStudents (StudentId,StudentName,StudentEmail) values (ABS(CHECKSUM(NewId())) ,N'Furkan Gözükara ÜİÇÖ home''s school','furkangozukara@gmail.com')

insert into tblStudents (StudentId,StudentName,StudentEmail,IsMale) values (ABS(CHECKSUM(NewId())),N'Furkan Gözükara ÜİÇÖ home''s school','furkangozukara@gmail.com',1)

insert into tblStudents (StudentId,StudentName,StudentEmail,IsMale,NickName) values (ABS(CHECKSUM(NewId())),N'Furkan Gözükara ÜİÇÖ home''s school','furkangozukara@gmail.com',1,'Teacher')


insert into tblStudents values (ABS(CHECKSUM(NewId())),N'Furkan Gözükara ÜİÇÖ home''s school','furkangozukara@gmail.com',1,'Teacher')

--insert into tblStudents values (75213545,N'Furkan Gözükara ÜİÇÖ home''s school','furkangozukara@gmail.com',1) this fails because we didnt provide exactly same number of values Column name or number of supplied values does not match table definition.
