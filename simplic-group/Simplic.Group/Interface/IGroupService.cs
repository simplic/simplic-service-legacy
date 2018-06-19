using System.Collections.Generic;

namespace Simplic.Group
{
    public interface IGroupService
    {
        /// <summary>
        /// Deletes a group
        /// </summary>
        /// <param name="id">Ident to be deleted.</param>
        /// <returns>True if successfull</returns>
        bool Delete(int id);

        /// <summary>
        /// Deletes a group
        /// </summary>
        /// <param name="groupId">Group Id to be deleted.</param>
        /// <returns>True if successfull</returns>
        bool DeleteByGroupId(int groupId);

        /// <summary>
        /// Gets a list of all groups
        /// </summary>
        /// <returns>A list of <see cref="Group"/></returns>
        IEnumerable<Group> GetAll();

        /// <summary>
        /// Gets a list of all groups sorted by group name
        /// </summary>
        /// <returns>A list of <see cref="Group"/></returns>
        IEnumerable<Group> GetAllSortedByName();

        /// <summary>
        /// Gets a list of all groups of a user
        /// </summary>
        /// <returns>A list of <see cref="Group"/></returns>
        IEnumerable<Group> GetAllByUserId(int userId);

        /// <summary>
        /// Gets a group given by its id
        /// </summary>
        /// <param name="id">Id of the group to search</param>
        /// <returns>A <see cref="Group"/></returns>
        Group GetById(int id);

        /// <summary>
        /// Inserts or updates a group into the database
        /// </summary>
        /// <param name="group"><see cref="Group"/> to be saved.</param>
        /// <returns>True if successfull</returns>
        bool Save(Group group);       
    }
}
