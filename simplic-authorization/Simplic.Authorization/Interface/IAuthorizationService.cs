using System.Collections.Generic;

namespace Simplic.Authorization
{
    public interface IAuthorizationService
    {
        /// <summary>
        /// Queries the database to return a bit set containing all the numbers given
        /// </summary>
        /// <param name="numbers">A list of numbers</param>
        /// <returns>A string containing a bit set of numbers given</returns>
        string CreateBitMask(IList<int> numbers);

        /// <summary>
        /// Queries the database to return a bit set
        /// </summary>
        /// <param name="number">A number to convert to a bit set </param>
        /// <returns>A string containing a bit set of numbers</returns>
        string CreateBitMask(int number);

        /// <summary>
        /// Gets the <see cref="RowAccess" /> of given row id
        /// </summary>
        /// <param name="tableName">Table to query</param>
        /// <param name="idColName">Name of the id column</param>
        /// <param name="rowId">Actual row id</param>
        /// <returns>Access rights of given row of the given table</returns>
        RowAccess GetAccessRights(string tableName, string idColName, object rowId);

        /// <summary>
        /// Sets the access rights for a given row in the given table
        /// </summary>
        /// <param name="tableName">Table to use</param>
        /// <param name="idColName">Name of the id column</param>
        /// <param name="rowId">Actual row id</param>
        /// <param name="rowAccess">Access rights of this spesific row</param>
        /// <returns>true if successfull</returns>
        bool SetAccess(string tableName, string idColName, object rowId, RowAccess rowAccess);

        /// <summary>
        /// Sets all access rights to null of a given row
        /// </summary>
        /// <param name="tableName">Table to use</param>
        /// <param name="idColName">Name of the id column</param>
        /// <param name="rowId">Actual row id</param>
        /// <returns>true if successfull</returns>
        bool ResetAccess(string tableName, string idColName, object rowId);

        /// <summary>
        /// Copies the access rights of a given row to the desired row
        /// </summary>
        /// <param name="tableNameFrom">Table to copy from</param>
        /// <param name="idColNameFrom">Name of the id column to copy from</param>
        /// <param name="rowIdFrom">Actual row id to copy from</param>
        /// <param name="tableNameTo">Table to save to</param>
        /// <param name="idColNameTo">Name of the id column to save to</param>
        /// <param name="rowIdTo">Actual row id to save to</param>
        /// <returns>true if successfull</returns>
        bool CopyFrom(string tableNameFrom, string idColNameFrom, object rowIdFrom, string tableNameTo, string idColNameTo, object rowIdTo);


        /// <summary>
        /// Sets the owner of a given row
        /// </summary>
        /// <param name="tableName">Table to use</param>
        /// <param name="idColName">Name of the id column</param>
        /// <param name="rowId">Actual row id</param>
        /// <param name="ownerId">Owner Id (User Id)</param>
        /// <returns>true if successfull</returns>
        bool SetOwner(string tableName, string idColName, object rowId, int ownerId);

        /// <summary>
        /// Gets an owner id of a given row
        /// </summary>
        /// <param name="tableName">Table to use</param>
        /// <param name="idColName">Name of the id column</param>
        /// <param name="rowId">Actual row id</param>
        /// <returns>true if successfull</returns>
        int GetOwnerId(string tableName, string idColName, object rowId);

        /// <summary>
        /// Converts a bit set ( bit mask ) to a list of integers
        /// </summary>
        /// <param name="bits">Bit set ( bit mask )</param>
        /// <returns></returns>
        IList<int> ConvertBitToInt(string bits);

        /// <summary>
        /// Creates the where condition for a particular table and row
        /// </summary>
        /// <param name="type">What kind of statement to be generated</param>
        /// <param name="tableAlias">Optional table alias e.g. SQL = TableName as tn, tableAlias="tn"</param>
        /// <param name="userId">User id</param>
        /// <param name="userBitMask">User id converted to bit mask</param>
        /// <param name="userGroupBitMask">User access groups converted to bit mask</param>        
        /// <returns>Where condition for a row level access control</returns>
        string GenerateAccessRightsStatement(AccessRightType type, int userId, string userBitMask, string userGroupBitMask, string tableAlias = "");  

        /// <summary>
        /// Creates new columns and converts all existing access rights
        /// </summary>
        IList<MigrationResult> Migrate();

        /// <summary>
        /// Checks if the given user has access
        /// </summary>
        /// <param name="type">What kind of statement to be generated</param>
        /// <param name="tableName">Table name</param>
        /// <param name="idColName">Id column name</param>
        /// <param name="rowId">Row id</param>       
        /// <param name="userId">User id</param>
        /// <param name="userBitMask">User id converted to bit mask</param>
        /// <param name="userGroupBitMask">User access groups converted to bit mask</param> 
        /// <returns>True if the user has access</returns>
        bool HasAccess(AccessRightType type, string tableName, string idColName, object rowId, int userId, string userBitMask, string userGroupBitMask);

        /// <summary>
        /// Checks if the given user has access
        /// </summary>
        /// <param name="type">What kind of statement to be generated</param>
        /// <param name="tableName">Table name</param>
        /// <param name="idColName">Id column name</param>
        /// <param name="rowId">Row id</param>       
        /// <param name="session">User session</param>
        /// <returns>True if the user has access</returns>
        bool HasAccess(AccessRightType type, string tableName, string idColName, object rowId, Session.Session session);

        ///// <summary>
        ///// Checks if the given user is a super user (admin)
        ///// </summary>
        ///// <param name="userId">User id to check</param>
        ///// <returns>True if the user is super user</returns>
        //bool IsSuperUser(int userId);
    }
}