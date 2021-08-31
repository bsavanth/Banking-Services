drop database LoginDB;

create database LoginDB;
use LoginDB;

create table Customer (

	CID int primary key identity(101,1), 
	SSN int unique, 
	Name varchar(30), 
	Age int, 
	Address1 varchar(300), 
	Address2 varchar(300), 
	City varchar(20), 
	State varchar(20),
	LastUpdated varchar(200)
	) ;

Alter table Customer Add constraint unique_constraint unique(SSN);
exec sp_help Customer;

select * from Customer;
delete from Customer;

insert into Customer values
(234567342, 'Test', 20, '702 W Trout Dr', 'Apt #1', 'Fayetteville', 'Arkansas', 'Added Record'); 



create table Account (
	
	AID int primary key identity(201,1), 
	CID int foreign key references Customer(CID), 
	AccountType varchar(30), 
	Balance money,
	LastUpdated varchar(200)
	)



select * from Account;
delete from Account;
create table Transactions (

	TID int primary key identity(301,1), 
	AID int foreign key references Account(AID),
	TransactionType varchar(20),
	TransactionAmount money,
	Date datetime,
	Description varchar(200)
	)

delete  from Transactions;
drop table Transactions;
select * from Transactions;

create table Error (
	
	ErrorID int primary key identity(401,1),
	Description varchar(500),
	PageDetail varchar(100)
	);

Delete from Error;

select * from Error;



drop table Account;

drop table Customer;

select * from Transactions;

delete from Account where Balance = -20;