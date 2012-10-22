/*
drop table dbo.UserActivities
drop table dbo.UsersByRoles
drop table dbo.UserRoles
drop table dbo.Users
*/
-----------------------------------

create table Users
(
    UserId int identity(1, 1) constraint PK_Users primary key,
    [Login] nvarchar(255) not null constraint UQ_Users_Login unique,
    PasswordHash nvarchar(512) not null,
    IsActive bit not null default(1),
    Email nvarchar(255) null,
    UserFIO nvarchar(255) null,
    Telephone nvarchar(255) null,
    Fax nvarchar(255) null,

    CreatorUserId int null constraint FK_Users_CreatorUserId foreign key(CreatorUserId) references dbo.Users (UserId),
    CreatedDate datetime not null default(getdate()),
);

go

create table UserRoles 
(
    UserRoleId int constraint PK_UserRoles primary key,
    Name nvarchar(128) not null constraint UQ_UserRoles unique,
    Description nvarchar(max) null,
    CreatedDate datetime not null default(getdate())
)
GO


create table UsersByRoles 
(
    UserId int not null constraint FK_UsersByRoles_UserId foreign key(UserId) references dbo.Users (UserId) on delete cascade,
    UserRoleId int not null constraint FK_UsersByRoles_UserRoleId foreign key(UserRoleId) references dbo.UserRoles (UserRoleId)
        on delete cascade,
    constraint PK_UsersByRoles primary key clustered (UserId asc, UserRoleId asc),
    
    CreatorUserId int not null constraint FK_UsersByRoles_CreatorUserId foreign key(CreatorUserId) references dbo.Users (UserId),
    CreatedDate datetime not null default(getdate())
)
go



create table UserActivities 
(
    UserActivityId int identity(1, 1) constraint PK_UserActivities primary key nonclustered,
    UserId int not null constraint FK_UserActivities_UserId foreign key(UserId)
        references dbo.Users (UserId)
        on delete cascade,
        
    IsChangePsw bit not null default (0),
    RawUrl nvarchar(max) null,
    Browser nvarchar(255) null,
    UserHostAddress nvarchar(64) null,
    IsPostBack bit not null default(0),
    ImpersonatedByUserId int null constraint FK_UserActivities_ImpersonatedByUserId foreign key(ImpersonatedByUserId)
        references dbo.Users (UserId),    
        
    CreatedDate datetime not null default (getdate())
)
go

-------------------------

insert into dbo.Users(Login,UserFIO,PasswordHash)
values  ('root','root','')

insert into dbo.UserRoles(UserRoleId, Name, Description) values  (1, N'Суперадмин', N'Ограничения не применяются');
insert into dbo.UserRoles(UserRoleId, Name, Description) values  (2, N'Админ', N'Редактирование справочников. Нельзя делать ставки' );
insert into dbo.UserRoles(UserRoleId, Name, Description) values  (3, N'Смотритель', N'Только смотреть' );

insert into dbo.UsersByRoles(UserId,UserRoleId,CreatorUserId)
values (1,1,1)


