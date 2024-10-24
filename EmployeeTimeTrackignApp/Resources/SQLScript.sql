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
	foreign key (ProjectID) references Projects(ProjectID)
);
GO