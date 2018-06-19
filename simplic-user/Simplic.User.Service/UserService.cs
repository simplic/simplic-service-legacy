using Dapper;
using iAnywhere.Data.SQLAnywhere;
using Simplic.Framework.DAL;
using System.Collections.Generic;
using System.Linq;
using System;
using Simplic.Security.Cryptography;

namespace Simplic.User.Service
{
    /// <summary>
    /// This service contains methods to manipulate user data 
    /// </summary>
    public class UserService : IUserService
    {
        #region Private Consts
        private const string UserTableName = "ESS_MS_Intern_User";
        #endregion

        #region [Delete]
        /// <summary>
        /// Deletes a user
        /// </summary>
        /// <param name="id">User id to be deleted.</param>
        /// <returns>True if successfull</returns>        
        public bool Delete(int id)
        {
            using (var connection = ConnectionManager.GetOpenPoolConnection<SAConnection>())
            {
                var affectedRows = connection.Execute($"DELETE FROM {UserTableName} WHERE Ident = :Ident", new { Ident = id });

                return affectedRows > 0;
            }
        }
        #endregion

        #region [GetAll]
        /// <summary>
        /// Gets a list of all users
        /// </summary>
        /// <returns>A list of <see cref="User"/></returns>
        public IEnumerable<User> GetAll()
        {
            using (var connection = ConnectionManager.GetOpenPoolConnection<SAConnection>())
            {
                return connection.Query<User>($"SELECT * FROM {UserTableName} ORDER BY Ident");
            }
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
            using (var connection = ConnectionManager.GetOpenPoolConnection<SAConnection>())
            {
                return connection.Query<User>($"SELECT * FROM {UserTableName} WHERE {(activeOnly ? " IsActive = 1 AND " : "")} Ident > 0 AND Ident < 32000 ORDER BY FirstName, LastName, UserName");
            }
        }
        #endregion

        #region [GetById/GetByName]
        /// <summary>
        /// Gets a user given by its id
        /// </summary>
        /// <param name="id">Id of the user to search</param>
        /// <returns>A <see cref="User"/></returns>
        public User GetById(int id)
        {
            using (var connection = ConnectionManager.GetOpenPoolConnection<SAConnection>())
            {
                return connection.Query<User>($"SELECT * FROM {UserTableName} WHERE Ident = :Ident", new { Ident = id })
                    .FirstOrDefault();
            }
        }

        /// <summary>
        /// Gets a user given by its name
        /// </summary>
        /// <param name="userName">Name of the user to search</param>
        /// <returns>A <see cref="User"/></returns>
        public User GetByName(string userName)
        {
            using (var connection = ConnectionManager.GetOpenPoolConnection<SAConnection>())
            {
                return connection.Query<User>($"SELECT * FROM {UserTableName} WHERE UserName = :userName", new { userName = userName })
                    .FirstOrDefault();
            }
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
            using (var connection = ConnectionManager.GetOpenPoolConnection<SAConnection>())
            {
                if (user.Ident == 0)
                    user.Ident = connection.Query<int>($"SELECT Get_Identity('{UserTableName}')").FirstOrDefault();

                var affectedRows = connection.Execute($"INSERT INTO {UserTableName} " +
                    $"(Ident, UserName, IsADUser, IsActive, FirstName, LastName, EMail, KeepLoggedIn, " +
                    $"LanguageID, ApiKey) "
                     + " ON EXISTING UPDATE VALUES " +
                     "(:Ident, :UserName, :IsADUser, :IsActive, :FirstName, :LastName, :EMail, :KeepLoggedIn,"
                     + " :LanguageID, :ApiKey)",
                     user);

                return affectedRows > 0;
            }
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

            // Implementation to be backword compatible. Should be changed later
            using (var connection = ConnectionManager.GetOpenPoolConnection<SAConnection>())
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
            }

            if (user.IsADUser)
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

            using (var connection = ConnectionManager.GetOpenPoolConnection<SAConnection>())
            {
                var affectedRows = connection.Execute($"UPDATE {UserTableName} " +
                    $" SET Passsword = :password where Ident = :userId", new
                    {
                        password = CryptographyHelper.GetMD5Hash(rawPassword),
                        userId = userId
                    });

                return affectedRows > 0;
            }
        }
        #endregion
    }
}
