using Dapper;
using Simplic.Security.Cryptography;
using Simplic.Sql;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Simplic.User.Service
{
    /// <summary>
    /// This service contains methods to manipulate user data 
    /// </summary>
    public class UserService : IUserService
    {
        #region Private Members
        private const string UserTableName = "ESS_MS_Intern_User";
        private readonly ISqlService sqlService;
        #endregion

        #region Constructor
        public UserService(ISqlService sqlService)
        {
            this.sqlService = sqlService;
        } 
        #endregion

        #region [Delete]
        /// <summary>
        /// Deletes a user
        /// </summary>
        /// <param name="id">User id to be deleted.</param>
        /// <returns>True if successfull</returns>        
        public bool Delete(int id)
        {
            return sqlService.OpenConnection((connection) => {
                var affectedRows = connection.Execute($"DELETE FROM {UserTableName} WHERE Ident = :Ident", new { Ident = id });
                return affectedRows > 0;
            });            
        }
        #endregion

        #region [GetAll]
        /// <summary>
        /// Gets a list of all users
        /// </summary>
        /// <returns>A list of <see cref="User"/></returns>
        public IEnumerable<User> GetAll()
        {
            return sqlService.OpenConnection((connection) => {
                return connection.Query<User>($"SELECT * FROM {UserTableName} ORDER BY Ident");
            });
        }
        #endregion

        #region [GetAllSortedByFirstName]
        /// <summary>
        /// Gets a list of all users sorted by first name
        /// </summary>
        /// <param name="activeOnly">If set to true, only active user will be selected</param>
        /// <returns>A list of <see cref="User"/></returns>
        public IEnumerable<User> GetAllSorted(bool activeOnly = true)
        {
            return sqlService.OpenConnection((connection) =>
            {
                return connection.Query<User>($"SELECT * FROM {UserTableName} WHERE {(activeOnly ? " IsActive = 1 AND " : "")} " +
                    $" Ident > 0 AND Ident < 32000 ORDER BY FirstName, LastName, UserName");
            });
        }
        #endregion
        
        #region [GetById]
        /// <summary>
        /// Gets a user given by its id
        /// </summary>
        /// <param name="id">Id of the user to search</param>
        /// <returns>A <see cref="User"/></returns>
        public User GetById(int id)
        {
            return sqlService.OpenConnection((connection) =>
            {
                return connection.Query<User>($"SELECT * FROM {UserTableName} WHERE Ident = :Ident", new { Ident = id })
                    .FirstOrDefault();
            });
        }
        #endregion

        #region [GetByName]
        /// <summary>
        /// Gets a user given by its name
        /// </summary>
        /// <param name="userName">Name of the user to search</param>
        /// <returns>A <see cref="User"/></returns>
        public User GetByName(string userName)
        {
            return sqlService.OpenConnection((connection) =>
            {
                return connection.Query<User>($"SELECT * FROM {UserTableName} WHERE UserName = :userName", new { userName = userName })
                    .FirstOrDefault();
            });
        }
        #endregion

        #region [Save]
        /// <summary>
        /// Inserts or updates a user into the database
        /// </summary>
        /// <param name="user"><see cref="User"/> to be saved.</param>
        /// <returns>True if successfull</returns>
        public bool Save(User user)
        {
            return sqlService.OpenConnection((connection) =>
            {
                if (user.Ident == 0)
                    user.Ident = connection.Query<int>($"SELECT Get_Identity('{UserTableName}')").FirstOrDefault();

                var affectedRows = connection.Execute($"INSERT INTO {UserTableName} " +
                    $"(Ident, UserName, IsADUser, IsActive, FirstName, LastName, EMail, KeepLoggedIn, " +
                    $" LanguageID, ApiKey) "
                     + " ON EXISTING UPDATE VALUES " +
                     "(:Ident, :UserName, :IsADUser, :IsActive, :FirstName, :LastName, :EMail, :KeepLoggedIn, "
                     + " :LanguageID, :ApiKey)",
                     user);

                return affectedRows > 0;
            });
        }
        #endregion

        #region [Register]
        /// <summary>
        /// Register new user
        /// </summary>
        /// <param name="user">User instance</param>
        /// <returns>True if registration was successfull</returns>
        public bool Register(User user)
        {
            if (!Save(user))
                return false;

            // Implementation to be backwards compatible. Should be changed later
            sqlService.OpenConnection((connection) =>
            {
                var dbUserName = "";

                if (user.IsADUser)
                    dbUserName = $"su.{Environment.UserDomainName}.{user.UserName}";
                else
                    dbUserName = $"su.{user.UserName}";

                var pwd = "integratedpw";
                var createUserStatement = String.Format("CREATE USER \"{0}\" IDENTIFIED BY '{1}';\r\n", dbUserName, pwd);
                var grant = String.Format("GRANT DBA TO \"{0}\";\r\n", dbUserName);
                var grantMembership = String.Format("GRANT MEMBERSHIP IN GROUP \"admin\" TO \"{0}\";", dbUserName);

                connection.Execute(createUserStatement + grant + grantMembership);

                // Sybase workaround
                System.Threading.Thread.Sleep(500);
                connection.Execute("commit;");
                System.Threading.Thread.Sleep(500);
                connection.Execute("commit;");             
            });

            if (!user.IsADUser)
                SetPassword(user.Ident, user.Password);

            return true;
        }
        #endregion

        #region [SetPassword]
        /// <summary>
        /// Set new password for a given user
        /// </summary>
        /// <param name="userId">Current user id</param>
        /// <param name="rawPassword">Raw password</param>
        /// <returns>True if setting the password was successfull</returns>
        public bool SetPassword(int userId, string rawPassword)
        {
            if (string.IsNullOrEmpty(rawPassword))
                return false;

            return sqlService.OpenConnection((connection) =>
            {
                var passwordHash = CryptographyHelper.GetMD5Hash(rawPassword);
                var affectedRows = connection.Execute($"UPDATE {UserTableName} " +
                    $" SET Password = :password WHERE Ident = :userId", new
                    {
                        password = passwordHash,
                        userId
                    });

                return affectedRows > 0;
            });
        }
        #endregion

        #region [GetByExternAccount]
        /// <summary>
        /// Get the simplic user by an external account name
        /// </summary>
        /// <param name="externAccountName">External account name</param>
        /// <returns>User id if found</returns>
        public User GetByExternAccount(string externAccountName)
        {
            return sqlService.OpenConnection((connection) =>
            {

                return connection.Query<User>("SELECT UserId FROM User_Extern_Account " +
                    " WHERE UserName = :externAccountName AND IsActive = 1", new { externAccountName })
                    .FirstOrDefault();
            });
        }
        #endregion
    }
}
