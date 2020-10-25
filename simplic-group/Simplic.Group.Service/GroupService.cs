using Dapper;
using Simplic.Sql;
using System.Collections.Generic;
using System.Linq;

namespace Simplic.Group.Service
{
    public class GroupService : IGroupService
    {
        #region Private Members
        private const string GroupTableName = "ESS_MS_Intern_Groups";
        private const string UserAssignmentTableName = "ESS_MS_Intern_UserAssignment";

        private ISqlService sqlService;
        #endregion

        #region Constructor
        public GroupService(ISqlService sqlService)
        {
            this.sqlService = sqlService;
        }
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
            return sqlService.OpenConnection((connection) =>
            {
                var affectedRows = connection.Execute($"DELETE FROM {GroupTableName} WHERE Ident = :Ident", new { Ident = id });
                return affectedRows > 0;
            });
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
            return sqlService.OpenConnection((connection) =>
            {
                var affectedRows = connection.Execute($"DELETE FROM {GroupTableName} WHERE GroupId = :GroupId", new { GroupId = groupId });

                return affectedRows > 0;
            });
        }
        #endregion

        #region [GetAll]
        /// <summary>
        /// Gets a list of all groups
        /// </summary>
        /// <returns>A list of <see cref="Group"/></returns>
        public IEnumerable<Group> GetAll()
        {
            return sqlService.OpenConnection((connection) =>
            {
                return connection.Query<Group>($"SELECT * FROM {GroupTableName} ORDER BY Ident");
            });
        }
        #endregion

        /// <summary>
        /// Gets a list of all groups sorted by group name
        /// </summary>
        /// <returns>A list of <see cref="Group"/></returns>
        public IEnumerable<Group> GetAllSortedByName()
        {
            return sqlService.OpenConnection((connection) =>
            {
                return connection.Query<Group>($"SELECT * FROM {GroupTableName} ORDER BY Name");
            });
        }

        #region [GetAllByUserId]
        /// <summary>
        /// Gets a list of all groups
        /// </summary>
        /// <returns>A list of <see cref="Group"/></returns>
        public IEnumerable<Group> GetAllByUserId(int userId)
        {
            return sqlService.OpenConnection((connection) =>
            {
                return connection.Query<Group>(
                    $"SELECT g.* FROM {UserAssignmentTableName} ua join {GroupTableName} g on ua.GroupId = g.GroupId " +
                    $"WHERE ua.UserId = :userId order by g.GroupId", new { userId });
            });
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
            return sqlService.OpenConnection((connection) =>
            {
                return connection.Query<Group>($"SELECT * FROM {GroupTableName} WHERE Ident = :Ident", new { Ident = id })
                    .FirstOrDefault();
            });
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
            return sqlService.OpenConnection((connection) =>
            {   
                if (group.Ident == 0)
                {
                    group.Ident = connection.Query<int>($"SELECT GET_Identity('{GroupTableName}')").First();
                    if(group.GroupId == 0)
                        group.GroupId = connection.Query<int>($"SELECT MAX (GroupId) FROM {GroupTableName}").First() + 100;
                }


                var affectedRows = connection.Execute($"INSERT INTO {GroupTableName} " +
                    $"(Ident, Name, GroupId, IsDefaultGroup) "
                     + " ON EXISTING UPDATE VALUES (:Ident, :Name, :GroupId, :IsDefaultGroup)", group);

                return affectedRows > 0;
            });
        }
        #endregion

        #endregion
    }
}
