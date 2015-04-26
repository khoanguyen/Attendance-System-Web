DELETE [Class];

DECLARE @ClassId INT;

INSERT INTO Class(Name,ProfessorName,StartDate,EndDate,ExcusedTime) 
VALUES('Software Engineering','Dr.Xu','2015-01-15','2015-05-10','00:30:00');

SET @ClassId = SCOPE_IDENTITY();

INSERT INTO ClassSession(ClassId, Weekday, StartTime, EndTime, Room) VALUES
(@ClassId, 'Sat','9:00:00','18:00:00','Auditorium'),
(@ClassId, 'Sun','9:00:00','18:00:00','Auditorium');


INSERT INTO Class(Name,ProfessorName,StartDate,EndDate,ExcusedTime) 
VALUES('Programming Paradigm','Dr.Hoffman','2015-02-01','2015-05-01','00:30:00');

SET @ClassId = SCOPE_IDENTITY();

INSERT INTO ClassSession(ClassId, Weekday, StartTime, EndTime, Room) VALUES
(@ClassId, 'Fri','18:00:00','20:30:00','306');


INSERT INTO Class(Name,ProfessorName,StartDate,EndDate,ExcusedTime) 
VALUES('Android Programming','Dr.Nguyen','2015-01-15','2015-05-10','00:30:00');

SET @ClassId = SCOPE_IDENTITY();

INSERT INTO ClassSession(ClassId, Weekday, StartTime, EndTime, Room) VALUES
(@ClassId, 'Fri','18:00:00','20:30:00','Auditorium');


INSERT INTO Class(Name,ProfessorName,StartDate,EndDate,ExcusedTime) 
VALUES('Probability and Statistic','Dr.Yurong','2015-01-15','2015-05-10','00:30:00');

SET @ClassId = SCOPE_IDENTITY();

INSERT INTO ClassSession(ClassId, Weekday, StartTime, EndTime, Room) VALUES
(@ClassId, 'Sun','17:30:00','20:00:00','301');
