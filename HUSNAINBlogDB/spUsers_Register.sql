CREATE PROCEDURE [dbo].[spUsers_Register]
    @username nvarchar(16),
    @firstName nvarchar(50),
    @lastName nvarchar(50),
    @password nvarchar(16)
AS
begin
    set nocount on;

    INSERT INTO dbo.Users
    (Username, FirstName, LastName, Password)
    VALUES (@username, @firstName, @lastName, @password)
end