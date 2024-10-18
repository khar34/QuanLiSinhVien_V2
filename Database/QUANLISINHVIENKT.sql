/*
Created		10/18/2024
Modified		10/18/2024
Project		
Model			
Company		
Author		
Version		
Database		MS SQL 2005 
*/
Create Database QUANLISINHVIENKT
use QUANLISINHVIENKT

Create table [Lop]
(
	[MaLop] Char(3) NOT NULL,
	[TenLop] Nvarchar(30) NOT NULL,
Primary Key ([MaLop])
) 
go

Create table [Sinhvien]
(
	[MaSV] Char(6) NOT NULL,
	[HotenSV] Nvarchar(40) NULL,
	[NgaySinh] Smalldatetime NULL,
	[MaLop] Char(3) NOT NULL,
Primary Key ([MaSV])
) 
go


Alter table [Sinhvien] add  foreign key([MaLop]) references [Lop] ([MaLop])  on update no action on delete no action 
go


Set quoted_identifier on
go


Set quoted_identifier off
go


