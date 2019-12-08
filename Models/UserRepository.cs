using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace Employee.Models
{
    public interface IUserRepository
    {
        int Create(User user);
        void Delete(int id);
        User Get(int id);
        List<User> GetUsers();
        List<User> Search(int id);
        void Update(User user);
    }
    public class UserRepository : IUserRepository
    {
        string connection;
        public UserRepository(string connect)
        {
            connection = connect;
        }

        public List<User> GetUsers()
        {
            using IDbConnection db = new SqlConnection(connection);
            return db.Query<User>("SELECT * FROM wokers").ToList();
        }

        public User Get(int id)
        {
            using IDbConnection db = new SqlConnection(connection);
            return db.Query<User>("SELECT * FROM wokers WHERE Id = @id", new { id }).FirstOrDefault();
        }

        public int Create(User user)
        {
            using IDbConnection db = new SqlConnection(connection);
            var sqlQuery = "INSERT INTO wokers (Name, Surname, Phone, CompanyId, PassportType, PassportNumber) VALUES (@Name, @Surname, @Phone, @CompanyId, @PassportType, @PassportNumber); SELECT CAST (SCOPE_IDENTITY() as int)";
            int? userId = db.Query<int>(sqlQuery, user).FirstOrDefault();
            return user.Id = userId.Value;
        }

        public List<User> Search(int id)
        {
            using IDbConnection db = new SqlConnection(connection);
            List<User> user = db.Query<User>("SELECT * FROM wokers WHERE CompanyId = @id", new { id }).ToList();
            return user;
        }

        public void Delete(int id)
        {
            using IDbConnection db = new SqlConnection(connection);
            var sqlQuery = "DELETE FROM wokers WHERE Id = @id";
            db.Execute(sqlQuery, new { id });
        }

        public void Update(User user)
        {
            using IDbConnection db = new SqlConnection(connection);
            var sqlQuery = "UPDATE wokers SET Name = @Name, Surname = @Surname, Phone = @Phone, CompanyId = @CompanyId, PassportType = @PassportType, PassportNumber = @PassportNumber WHERE Id = @id";
            db.Execute(sqlQuery, user);
        }
    }
}
