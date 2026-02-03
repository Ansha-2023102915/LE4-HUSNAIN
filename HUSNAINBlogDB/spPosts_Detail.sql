CREATE PROCEDURE [dbo].[spPosts_Detail]
    @postId int
AS
begin
    set nocount on;

    SELECT p.Id, p.UserId, p.Title, p.Content, p.CreatedAt,
           u.Username, u.FirstName, u.LastName
    FROM dbo.Post p
    INNER JOIN dbo.Users u ON p.UserId = u.Id
    WHERE p.Id = @postId
end