using Dapper;
using iAnywhere.Data.SQLAnywhere;
using Simplic.Framework.DAL;
using System.Collections.Generic;
using System.Linq;

namespace Simplic.Group.Service
{
    public class GroupService : IGroupService
    {
        #region Private Consts
        private const string GroupTableName = "ESS_MS_Intern_Groups";
        private const string UserAssignmentTableName = "ESS_MS_Intern_UserAssignment";
        #endregion

        #region Public Methods

        #region [Delete]
        /// <summary>
        /// Deletes a group
        /// </summary>
        /// <param name="id">Ident to be deleted.</param>
        /// <returns>True if successfull</returns>
        public bool Delete(int id)
        {
            using (var connection = ConnectionManager.GetOpenPoolConnection<SAConnection>())
            {
                var affectedRows = connection.Execute($"DELETE FROM {GroupTableName} WHERE Ident = :Ident", new { Ident = id });

                return affectedRows > 0;
            }
        }
        #endregion

        #region [DeleteByGroupId]
        /// <summary>
        /// Deletes a group
        /// </summary>
        /// <param name="groupId">Group Id to be deleted.</param>
        /// <returns>True if successfull</returns>
        public bool DeleteByGroupId(int groupId)
        {
            using (var connection = ConnectionManager.GetOpenPoolConnection<SAConnection>())
            {
                var affectedRows = connection.Execute($"DELETE FROM {GroupTableName} WHERE GroupId = :GroupId", new { GroupId = groupId });

                return affectedRows > 0;
            }
        }
        #endregion

        #region [GetAll]
        /// <summary>
        /// Gets a list of all groups
        /// </summary>
        /// <returns>A list of <see cref="Group"/></returns>
        public IEnumerable<Group> GetAll()
        {
            using (var connection = ConnectionManager.GetOpenPoolConnection<SAConnection>())
            {
                return connection.Query<Group>($"SELECT * FROM {GroupTableName} ORDER BY Ident");
            }
        }
        #endregion

        /// <summary>
        /// Gets a list of all groups sorted by group name
        /// </summary>
        /// <returns>A list of <see cref="Group"/></returns>
        public IEnumerable<Group> GetAllSortedByName()
        {
            using (var connection = ConnectionManager.GetOpenPoolConnection<SAConnection>())
            {
                return connection.Query<Group>($"SELECT * FROM {GroupTableName} ORDER BY Name");
            }
        }

        #region [GetAllByUserId]
        /// <summary>
        /// Gets a list of all groups
        /// </summary>
        /// <returns>A list of <see cref="Group"/></returns>
        public IEnumerable<Group> GetAllByUserId(int userId)
        {
            using (var connection = ConnectionManager.GetOpenPoolConnection<SAConnection>())
            {
                return connection.Query<Group>(
                    $"SELECT g.* FROM {UserAssignmentTableName} ua join {GroupTableName} g on ua.GroupId = g.GroupId " +
                    $"WHERE ua.UserId = :userId order by g.GroupId", new { userId });
            }
        }
        #endregion

        #region [GetById]
        /// <summary>
        /// Gets a group given by its id
        /// </summary>
        /// <param name="id">Id of the group to search</param>
        /// <returns>A <see cref="Group"/></returns>
        public Group GetById(int id)
        {
            using (var connection = ConnectionManager.GetOpenPoolConnection<SAConnection>())
            {
                return connection.Query<Group>($"SELECT * FROM {GroupTableName} WHERE Ident = :Ident", new { Ident = id })
                    .FirstOrDefault();
            }
        }
        #endregion

        #region [Save]
        /// <summary>
        /// Inserts or updates a group into the database
        /// </summary>
        /// <param name="group"><see cref="Group"/> to be saved.</param>
        /// <returns>True if successfull</returns>
        public bool Save(Group group)
        {
            using (var connection = ConnectionManager.GetOpenPoolConnection<SAConnection>())
            {
                var affectedRows = connection.Execute($"INSERT INTO {GroupTableName} " +
                    $"(Ident, Name, GroupId, IsDefaultGroup) "
                     + " ON EXISTING UPDATE VALUES (:Ident, :Name, :GroupId, :IsDefaultGroup)", group);

                return affectedRows > 0;
            }
        }
        #endregion

        #endregion
    }
}
