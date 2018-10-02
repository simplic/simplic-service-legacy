using System.Collections.Generic;

namespace Simplic.User
{
    /// <summary>
    /// Contains methods to manipulate user data 
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Deletes a user
        /// </summary>
        /// <param name="id">User id to be deleted.</param>
        /// <returns>True if successfull</returns>
        bool Delete(int id);

        /// <summary>
        /// Gets a list of all users
        /// </summary>
        /// <returns>A list of <see cref="User"/></returns>
        IEnumerable<User> GetAll();

        /// <summary>
        /// Gets a list of all users sorted by first name
        /// </summary>
        /// <param name="activeOnly">If set to true, only active user will be selected</param>
        /// <returns>A list of <see cref="User"/></returns>
        IEnumerable<User> GetAllSorted(bool activeOnly = true);

        /// <summary>
        /// Get the simplic user by an external account name
        /// </summary>
        /// <param name="externAccountName">External account name</param>
        /// <returns>User id if found</returns>
        User GetByExternAccount(string externAccountName);

        /// <summary>
        /// Gets a user given by its id
        /// </summary>
        /// <param name="id">Id of the user to search</param>
        /// <returns>A <see cref="User"/></returns>
        User GetById(int id);

        /// <summary>
        /// Gets a user given by its name
        /// </summary>
        /// <param name="userName">Name of the user to search</param>
        /// <returns>A <see cref="User"/></returns>
        User GetByName(string userName);

        /// <summary>
        /// Get user by apikey and username
        /// </summary>
        /// <param name="apiKey">api key</param>
        /// <param name="userName">User name</param>
        /// <returns>A <see cref="User"/></returns>
        User GetApiUser(string apiKey, string userName);

        /// <summary>
        /// Inserts or updates a user into the database
        /// </summary>
        /// <param name="user"><see cref="User"/> to be saved.</param>
        /// <returns>True if successfull</returns>
        bool Save(User user);

        /// <summary>
        /// Register new user
        /// </summary>
        /// <param name="user">User instance</param>
        /// <returns>True if registration was successfull</returns>
        bool Register(User user);

        /// <summary>
        /// Set new password for a given user
        /// </summary>
        /// <param name="userId">Current user id</param>
        /// <param name="rawPassword">Raw password</param>
        /// <returns>True if setting the password was successfull</returns>
        bool SetPassword(int userId, string rawPassword);

        /// <summary>
        /// Assigns a user to a group (updates on existing values)
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <param name="groupId">Group Id</param>
        /// <returns>True if successfull</returns>
        bool SetGroup(int userId, int groupId);
    }
}
