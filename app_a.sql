create database app_a;
go

use app_a;
go

create table clients (
	id int primary key not null identity(1,1),
	name varchar(200) not null,
	cedula char(11) not null
);

create table invoices (
	id int primary key not null identity(1,1),
	[description] varchar(200) not null,
	amount decimal(13,2) not null,
	client_id int not null
);

insert into clients values ('Cliente 3', '00100000003'), ('Cliente 2', '00100000002');

insert into invoices values ('Factura cliente 1', 1250.50, 1), ('Factura cliente 2', 525.00, 2);

create table checks (
	id int primary key not null identity(1,1),
	[number] int not null,
	concept varchar(200),
	client_id int not null,
	payment_date datetime not null,
	amount decimal(13,2) not null,
	amount_in_letters varchar(200) not null,
	invoice_id int not null
);

alter table invoices add constraint fk_invoices_client_id_clients_id foreign key (client_id) references clients(id);
alter table checks add constraint fk_checks_client_id_clients_id foreign key (client_id) references clients(id);
alter table checks add constraint fk_checks_invoice_id_invoices_id foreign key (invoice_id) references invoices(id);

select * from checks