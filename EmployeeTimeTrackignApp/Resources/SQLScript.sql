use EmployeesWorkHoursLog;
GO

create table Employees (
	EmployeeID int primary key identity(1, 1),
	Username nvarchar(30) not null unique,
    	Password nvarchar(255) not null,
	PasswordCreatedAt datetime default getdate(),
	Email nvarchar(100) not null,
	Role nvarchar(20) check (Role IN ('Employee', 'Manager', 'Admin')) not null,
	IsActive bit default 1,
	CreatedAt datetime default getdate(),
	RemainingLeaveDays int not null,
	PasswordUpdated bit default 0 not null
);
GO

create table Projects (
	ProjectID int primary key identity(10000, 1),
	OwnerID int not null,
	Name nvarchar(50) not null,
	IsActive bit default 0,
	CreatedAt datetime default getdate()
	foreign key (OwnerID) references Employees(EmployeeID)
);
GO

create table WorkHours (
	WorkHoursID int primary key identity(2000, 1),
	EmployeeID int not null,
	ProjectID int not null,
	AddedHours int not null,
	CreatedAt datetime default getdate(),
	Status nvarchar(255) check (Status IN ('Pending', 'Accepted', 'Rejected')) default 'Pending',
	Comment nvarchar(255),
	foreign key (EmployeeID) references Employees(EmployeeID),
	foreign key (ProjectID) references Projects(ProjectID) ON DELETE CASCADE
);
GO

--Procedures
--Procedure for Login
CREATE PROCEDURE GetEmployeeByCredentials
    @Username NVARCHAR(30),
    @Password NVARCHAR(255)
AS
BEGIN
    SELECT EmployeeID, Username, Password, PasswordCreatedAt, Email, Role, IsActive, CreatedAt, RemainingLeaveDays, PasswordUpdated 
    FROM Employees
    WHERE Username = @Username AND Password = @Password;
END;
GO

--Procedure for getting all added working hours
CREATE PROCEDURE FindAllByEmployeeID
    @EmployeeID INT
AS
BEGIN
    SELECT WorkHoursID, EmployeeID, ProjectID, AddedHours, CreatedAt, Status, Comment
    FROM WorkHours
    WHERE EmployeeID = @EmployeeID;
END;
GO

--Procedure for getting active projects for adding working hours
CREATE PROCEDURE FindAllActiveProjects
AS
BEGIN
	SELECT ProjectID, OwnerID, Name, IsActive, CreatedAt
	FROM Projects
	Where IsActive = 1;
END;
GO

--Procedure for getting all projects
CREATE PROCEDURE FindAllProjectsForAdmin
AS
BEGIN
	SELECT ProjectID, OwnerID, Name, IsActive, CreatedAt
	FROM Projects;
END;
GO

--Procedure for getting all managers for the admin to assign someone to a project
CREATE PROCEDURE FindAllManagersForAdmin
AS
BEGIN
	SELECT EmployeeID, Username, Email, IsActive, RemainingLeaveDays, PasswordUpdated
	FROM Employees
	WHERE Role = 'Manager' AND IsActive = 1;
END;
GO

--Procedure for all employees without admin
CREATE PROCEDURE FindAllEmployees
	@AdminID INT
AS
BEGIN
	SELECT EmployeeID, Username, Email, Role, IsActive, RemainingLeaveDays, PasswordUpdated
	FROM Employees
	WHERE EmployeeID != @AdminID;
END;
GO

--Procedure for getting all projects for this month so we can show statistics for them
CREATE PROCEDURE FindAllProjectsForStatistics
    @EmployeeID INT
AS
BEGIN
    SELECT p.ProjectID, p.Name 
    FROM Projects p
    JOIN WorkHours wh ON p.ProjectID = wh.ProjectID
    WHERE wh.EmployeeID = @EmployeeID 
      AND wh.CreatedAt >= DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE()), 0)
      AND wh.CreatedAt < GETDATE()
    GROUP BY p.ProjectID, p.Name;
END;
GO

--Procedure for getting working hours for statistics
CREATE PROCEDURE WorkingHoursByProject
    @ProjectID INT
AS
BEGIN
    SELECT Status, SUM(AddedHours) AS TotalHours
    FROM WorkHours
    WHERE ProjectID = @ProjectID
      AND CreatedAt >= DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE()), 0)
      AND CreatedAt < GETDATE()
    GROUP BY Status;
END;
GO

--Procedure for getting all projects where owner is logged manager
CREATE PROCEDURE FindAllProjectsForManager
	@OwnerID INT
AS
BEGIN
	SELECT ProjectID, OwnerID, Name, IsActive, CreatedAt
	FROM Projects
	WHERE OwnerID = @OwnerID;
END;
GO

--Procedure for getting added working hours for logged manager so he can manage them
CREATE PROCEDURE GetWorkHoursByManagerAndDateRange
    @OwnerID INT,
    @DateRange DATETIME
AS
BEGIN
    SELECT WorkHoursID, EmployeeID, ProjectID, AddedHours, CreatedAt, Status, Comment
    FROM WorkHours
    WHERE ProjectID IN (SELECT ProjectID FROM Projects WHERE OwnerID = @OwnerID)
    AND CreatedAt >= @DateRange;
END
GO