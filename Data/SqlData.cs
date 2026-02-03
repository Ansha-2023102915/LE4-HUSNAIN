using BlogDataLibrary.Database;
using BlogDataLibrary.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace BlogDataLibrary.Data
{
    public class SqlData : ISqlData
    {
        private ISqlDataAccess _db;
        private const string connectionStringName = "Default";

        public SqlData(ISqlDataAccess db)
        {
            _db = db;
        }

        public UserModel Authenticate(string username, string password)
        {
            List<UserModel> results = _db.LoadData<UserModel, dynamic>(
                "dbo.spUsers_Authenticate",
                new { username, password },
                connectionStringName,
                true);

            return results.Count > 0 ? results[0] : null;
        }

        public void Register(string username, string firstName, string lastName, string password)
        {
            _db.SaveData<dynamic>(
                "dbo.spUsers_Register",  // CORRECT: No .sql extension
                new { username, firstName, lastName, password },
                connectionStringName,
                true);
        }

        public void AddPost(PostModel post)
        {
            // Validate input
            if (string.IsNullOrEmpty(post.Content))
            {
                throw new ArgumentException("Post content cannot be empty");
            }

            _db.SaveData(
                "dbo.spPosts_Insert",  // CORRECT: No .sql extension
                new
                {
                    userId = post.UserId,
                    title = post.Title,
                    content = post.Content,
                    dateCreated = post.CreatedAt
                },
                connectionStringName,
                true);
        }

        public List<ListPostModel> ListPosts()
        {
            return _db.LoadData<ListPostModel, dynamic>(
                "dbo.spPosts_List",  // CORRECT: No .sql extension
                new { },
                connectionStringName,
                true);
        }

        public PostModel ShowPostDetails(int postId)
        {
            List<PostModel> results = _db.LoadData<PostModel, dynamic>(
                "dbo.spPosts_Detail",  // CORRECT: No .sql extension
                new { postId },
                connectionStringName,
                true);
            return results.Count > 0 ? results[0] : null;
        }
    }
}

/* LE2
public Task RegisterUser(string username, string password, string firstName, string lastName)
{
    string sql = @"INSERT INTO dbo.Users (UserName, Password, FirstName, LastName) 
                  VALUES (@UserName, @Password, @FirstName, @LastName)";

    return Task.Run(() =>
        _db.SaveData(sql, new { UserName = username, Password = password, FirstName = firstName, LastName = lastName },
                    connectionStringName, false));
}

public async Task<int?> Authenticate(string username, string password)
{
    string sql = "SELECT Id FROM dbo.Users WHERE UserName = @UserName AND Password = @Password";

    var result = await Task.Run(() =>
        _db.LoadData<int, dynamic>(sql, new { UserName = username, Password = password },
                                 connectionStringName, false));

    if (result.Count > 0)
    {
        return result[0];
    }
    else
    {
        return null;
    }
}

public Task AddPost(int userId, string title, string content)
{
    string sql = @"INSERT INTO dbo.Posts (UserId, Title, Content, CreatedAt) 
                  VALUES (@UserId, @Title, @Content, GETUTCDATE())";

    return Task.Run(() =>
        _db.SaveData(sql, new { UserId = userId, Title = title, Content = content },
                    connectionStringName, false));
}

public async Task<List<ListPostModels>> ListPosts()
{
    string sql = @"SELECT p.Id, p.Title, p.Content, p.CreatedAt as DateCreated, 
                          u.UserName, u.FirstName, u.LastName
                   FROM dbo.Posts p
                   INNER JOIN dbo.Users u ON p.UserId = u.Id
                   ORDER BY p.CreatedAt DESC";

    return await Task.Run(() =>
        _db.LoadData<ListPostModels, dynamic>(sql, new { }, connectionStringName, false));
}

public async Task<PostModelss> GetPost(int postId)
{
    string sql = @"SELECT p.Id, p.UserId, p.Title, p.Content, p.CreatedAt as DateCreated,
                  u.UserName, u.FirstName, u.LastName
           FROM dbo.Posts p
           INNER JOIN dbo.Users u ON p.UserId = u.Id
           WHERE p.Id = @PostId";

    var results = await Task.Run(() =>
        _db.LoadData<PostModelss, dynamic>(sql, new { PostId = postId }, connectionStringName, false));

    return results.Count > 0 ? results[0] : null;
*/