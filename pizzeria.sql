IF DB_ID('Pizzeria') IS NULL
CREATE DATABASE Pizzeria
GO
USE Pizzeria;


if exists (SELECT name FROM sys.sysobjects WHERE name = 'tblRecord')
drop table tblRecord;

if exists (SELECT name FROM sys.sysobjects WHERE name = 'tblOrder')
drop table tblOrder;

if exists (SELECT name FROM sys.sysobjects WHERE name = 'tblMenu')
drop table tblMenu;

if exists (SELECT name FROM sys.sysobjects WHERE name = 'tblStatus')
drop table tblStatus;


Create table tblMenu (
MealID int identity (1,1) primary key,
MealName varchar(10) not null,
Price int not null
)

Create table tblStatus (
StatusID int identity (1,1) primary key,
StatusName varchar(10)
)

create table tblOrder(
OrderID int identity(1,1) primary key,
JMBG varchar(30) check(len(JMBG) = 13)  not null,
DateAndTime datetime not null,
StatusID int foreign key (StatusID) references tblStatus(StatusID) not null,
Price int,
constraint checkJMBG check(JMBG not like '%[^0-9]%')
)

create table tblRecord(
RecordID int identity(1,1) primary key,
MealID int foreign key (MealID) references tblMenu(MealID) not null,
Amount int not null,
OrderID int foreign key (OrderID) references tblOrder(OrderID) not null
)


insert into tblStatus (StatusName)
values('Waiting');

insert into tblStatus (StatusName)
values('Approved');

insert into tblStatus (StatusName)
values('Declined');

insert into tblMenu(MealName, Price)
values('Capriciosa', 300);

insert into tblMenu(MealName, Price)
values('Margarita', 350);

insert into tblMenu(MealName, Price)
values('Mexicana', 400);