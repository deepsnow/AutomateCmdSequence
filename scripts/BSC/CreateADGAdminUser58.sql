use ADG 

DELETE FROM ADG58.cmn_UserEntityRights WHERE UserID = 1005
GO

DELETE FROM ADG58.cmn_Users WHERE UserID = 1005
go

insert into ADG58.cmn_Users(UserID, UserName, CreationDate, IsActive, IsAuthorizedToChangePassword, IsDeleted, IsSuperAdmin, Password, LastActivityDate, LastModifiedDate) 
values (1005, 'Administrator', GETDATE(), 1, 1, 0, 0, 'A@D3FBC0863D976980C165F0D7D81FD0F119B10974', GETDATE(), GETDATE())
GO 

declare @newEntityId bigint 
select @newEntityId = ADG58.nextValue('cmn_UserEntityRights_PK_Seq')
insert into ADG58.cmn_UserEntityRights(UserEntityRightsID, UserID, EntityType, RoleID, Disallow)
values(@newEntityId, 1005, 2, 100, 0)
go