create database app_b;
use app_b;

create table clients (
	id int primary key not null identity(1,1),
	cedula char(11) not null unique,
	[path] varchar(250) not null
);

insert into clients values
('00100000001', 'images/invoices/1.png'),
('00100000002', 'images/invoices/2.png');

select * from clients