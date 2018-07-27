using Dapper;
using Simplic.Memory;
using Simplic.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simplic.Authorization.Service
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly ISqlService sqlService;

        public AuthorizationService(ISqlService sqlService)
        {
            this.sqlService = sqlService;
        }

        #region Public Methods

        #region [CreateBitMask]
        /// <summary>
        /// Queries the database to return a bit set containing all the numbers given
        /// </summary>
        /// <param name="numbers">A list of numbers</param>
        /// <returns>A string containing a bit set of numbers given</returns>
        public string CreateBitMask(IList<int> numbers)
        {
            if (numbers == null || numbers.Count == 0) return null;

            var sqlSb = new StringBuilder();
            foreach (var id in numbers)
            {
                if (sqlSb.Length > 0)
                    sqlSb.Append(" | ");

                sqlSb.Append($" set_bit({id}) ");
            }

            return sqlService.OpenConnection((connection) =>
            {
                return connection.Query<string>($"SELECT ({sqlSb.ToString()}) as BitMask")
                    .FirstOrDefault();
            });
        }
        #endregion

        #region [CreateBitMask]
        /// <summary>
        /// Queries the database to return a bit set
        /// </summary>
        /// <param name="number">A number to convert to a bit set </param>
        /// <returns>A string containing a bit set of numbers</returns>
        public string CreateBitMask(int number)
        {
            return CreateBitMask(new List<int> { number });
        }
        #endregion

        #region [GetAccessRights]
        /// <summary>
        /// Gets the <see cref="RowAccess" /> of given row id
        /// </summary>
        /// <param name="tableName">Table to query</param>
        /// <param name="idColName">Name of the id column</param>
        /// <param name="rowId">Actual row id</param>
        /// <returns>Access rights of given row of the given table</returns>
        public RowAccess GetAccessRights(string tableName, string idColName, object rowId)
        {
            return sqlService.OpenConnection((connection) =>
            {

                var rights = connection.Query<DataRowAccessRight>($"SELECT OwnerId, UserFullAccess, UserReadAccess, UserWriteAccess, " +
                    $"GroupFullAccess, GroupReadAccess, GroupWriteAccess from {tableName} where {idColName} = :rowId",
                        new { rowId }).FirstOrDefault();

                if (rights == null) return null;

                return new RowAccess
                {
                    OwnerId = rights.OwnerId,
                    UserFullAccess = ConvertBitToInt(rights.UserFullAccess),
                    UserReadAccess = ConvertBitToInt(rights.UserReadAccess),
                    UserWriteAccess = ConvertBitToInt(rights.UserWriteAccess),
                    GroupFullAccess = ConvertBitToInt(rights.GroupFullAccess),
                    GroupReadAccess = ConvertBitToInt(rights.GroupReadAccess),
                    GroupWriteAccess = ConvertBitToInt(rights.GroupWriteAccess)
                };

            });
        }
        #endregion

        #region [SetAccess]
        /// <summary>
        /// Sets the access rights for a given row in the given table
        /// </summary>
        /// <param name="tableName">Table to use</param>
        /// <param name="idColName">Name of the id column</param>
        /// <param name="rowId">Actual row id</param>
        /// <param name="rowAccess">Access rights of this spesific row</param>
        /// <returns>true if successfull</returns>
        public bool SetAccess(string tableName, string idColName, object rowId, RowAccess rowAccess)
        {
            var groupReadIds = rowAccess.GroupReadAccess?.ToList() ?? new List<int>();
            var groupWriteIds = rowAccess.GroupWriteAccess?.ToList() ?? new List<int>();
            var groupFullIds = rowAccess.GroupFullAccess?.ToList() ?? new List<int>();

            var userReadIds = rowAccess.UserReadAccess?.ToList() ?? new List<int>();
            var userWriteIds = rowAccess.UserWriteAccess?.ToList() ?? new List<int>();
            var userFullIds = rowAccess.UserFullAccess?.ToList() ?? new List<int>();

            // if full access is given, give read and write access manually
            if (groupFullIds.Count() > 0)
            {
                groupReadIds.AddRange(groupFullIds.Except(groupReadIds));
                groupWriteIds.AddRange(groupFullIds.Except(groupWriteIds));
            }

            // if write access is given, give read access manually
            if (groupWriteIds.Count() > 0)
            {
                groupReadIds.AddRange(groupWriteIds.Except(groupReadIds));
            }

            // if full access is given, give read and write access manually
            if (userFullIds.Count() > 0)
            {
                userReadIds.AddRange(userFullIds.Except(userReadIds));
                userWriteIds.AddRange(userFullIds.Except(userWriteIds));
            }

            // if write access is given, give read access manually
            if (userWriteIds.Count() > 0)
            {
                userReadIds.AddRange(userWriteIds.Except(userReadIds));
            }

            var groupReadAccess = CreateBitMask(groupReadIds);
            var groupWriteAccess = CreateBitMask(groupWriteIds);
            var groupFullAccess = CreateBitMask(groupFullIds);

            var userReadAccess = CreateBitMask(userReadIds);
            var userWriteAccess = CreateBitMask(userWriteIds);
            var userFullAccess = CreateBitMask(userFullIds);
                                    
            return sqlService.OpenConnection((connection) =>
            {

                var affectedRows = connection.Execute($"UPDATE {tableName} SET OwnerId = :OwnerId, " +
                    $"UserReadAccess = :UserReadAccess, UserWriteAccess = :UserWriteAccess, " +
                    $"UserFullAccess = :UserFullAccess,  GroupReadAccess = :GroupReadAccess, " +
                    $"GroupWriteAccess = :GroupWriteAccess, GroupFullAccess = :GroupFullAccess " +
                    $"WHERE {idColName} = :RowId",
                    new
                    {
                        OwnerId = rowAccess.OwnerId,
                        UserReadAccess = userReadAccess,
                        UserWriteAccess = userWriteAccess,
                        UserFullAccess = userFullAccess,
                        GroupReadAccess = groupReadAccess,
                        GroupWriteAccess = groupWriteAccess,
                        GroupFullAccess = groupFullAccess,
                        RowId = rowId
                    });

                return affectedRows > 0;
            });
        }
        #endregion

        #region [ResetAccess]
        /// <summary>
        /// Sets all access rights to null of a given row
        /// </summary>
        /// <param name="tableName">Table to use</param>
        /// <param name="idColName">Name of the id column</param>
        /// <param name="rowId">Actual row id</param>
        /// <returns>true if successfull</returns>
        public bool ResetAccess(string tableName, string idColName, object rowId)
        {
            return SetAccess(tableName, idColName, rowId, null);
        }
        #endregion

        #region [CopyFrom]
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
        public bool CopyFrom(string tableNameFrom, string idColNameFrom, object rowIdFrom, string tableNameTo, string idColNameTo, object rowIdTo)
        {
            var accessRights = GetAccessRights(tableNameFrom, idColNameFrom, rowIdFrom);

            if (accessRights == null)
            {
                //TODO: Log an error message
                return false;
            }

            var setAccessRightsResult = SetAccess(tableNameTo, idColNameTo, rowIdTo, accessRights);
            if (setAccessRightsResult == false)
            {
                //TODO: Log an error message
                return false;
            }
            else
                return true;
        }
        #endregion

        #region [SetOwner]
        /// <summary>
        /// Sets the owner of a given row
        /// </summary>
        /// <param name="tableName">Table to use</param>
        /// <param name="idColName">Name of the id column</param>
        /// <param name="rowId">Actual row id</param>
        /// <param name="ownerId">Owner Id (User Id)</param>
        /// <returns>true if successfull</returns>
        public bool SetOwner(string tableName, string idColName, object rowId, int ownerId)
        {
            return sqlService.OpenConnection((connection) =>
            {

                var affectedRows = connection.Execute($"UPDATE {tableName} set OwnerId = :ownerId " +
                   $" where {idColName} = :rowId",
                   new
                   {
                       rowId,
                       ownerId
                   });

                return affectedRows > 0;
            });
        }
        #endregion

        #region [GetOwnerId]
        /// <summary>
        /// Gets an owner id of a given row
        /// </summary>
        /// <param name="tableName">Table to use</param>
        /// <param name="idColName">Name of the id column</param>
        /// <param name="rowId">Actual row id</param>
        /// <returns>true if successfull</returns>
        public int GetOwnerId(string tableName, string idColName, object rowId)
        {
            return sqlService.OpenConnection((connection) =>
            {

                var ownerId = connection.Execute($"SELECT OwnerId FROM {tableName} where {idColName} = :rowId",
                    new { rowId });

                return ownerId;
            });
        }
        #endregion

        #region [ConvertBitToInt]
        /// <summary>
        /// Converts a bit set ( bit mask ) to a list of integers
        /// </summary>
        /// <param name="bits">Bit set ( bit mask )</param>
        /// <returns></returns>
        public IList<int> ConvertBitToInt(string bits)
        {
            var result = new List<int>();
            if (string.IsNullOrEmpty(bits)) return result;

            var arr = bits.ToCharArray();
            for (int i = 0; i < arr.Count(); i++)
            {
                var bit = (int)Char.GetNumericValue(arr.ElementAt(i));
                if (bit == 1)
                {
                    result.Add(i + 1);
                }
            }

            return result;
        }
        #endregion

        #region [GenerateAccessRightsStatement]
        /// <summary>
        /// Creates the where condition for a particular table and row
        /// </summary>
        /// <param name="type">What kind of statement to be generated</param>
        /// <param name="tableAlias">Optional table alias e.g. SLQ = TableName as tn, tableAlias="tn"</param>
        /// <param name="userId">User id</param>
        /// <param name="userBitMask">User id converted to bit mask</param>
        /// <param name="userGroupBitMask">User access groups converted to bit mask</param>
        /// <returns>Where condition for a row level access control</returns>
        public string GenerateAccessRightsStatement(AccessRightType type, int userId, string userBitMask, string userGroupBitMask, string tableAlias = "")
        {
            if (type == AccessRightType.Read)
                return GenerateStatement(userId, "UserReadAccess", "GroupReadAccess", userBitMask, userGroupBitMask, tableAlias);
            else if (type == AccessRightType.Write)
                return GenerateStatement(userId, "UserWriteAccess", "GroupWriteAccess", userBitMask, userGroupBitMask, tableAlias);
            else if (type == AccessRightType.Full)
                return GenerateStatement(userId, "UserFullAccess", "GroupFullAccess", userBitMask, userGroupBitMask, tableAlias);
            else
                return string.Empty;
        }

        /// <summary>
        /// Generates a where condition for given access column
        /// </summary>
        /// <param name="userId">User Id to be used in OwnerId</param>
        /// <param name="accessColumnUser"></param>
        /// <param name="accessColumnGroup"></param>
        /// <param name="userBitMask">User bit mask</param>
        /// <param name="groupBitMask">Group bit mask</param>
        /// <param name="tableAlias">Tabl alias</param>
        /// <returns>Generated where condition</returns>
        private string GenerateStatement(int userId, string accessColumnUser, string accessColumnGroup, string userBitMask, string userGroupBitMask, string tableAlias = "")
        {
            var sb = new StringBuilder();
            sb.Append("(");

            if (string.IsNullOrEmpty(tableAlias))
            {
                sb.Append($" OwnerId = { userId } ");
                sb.Append($" OR CheckAccess({accessColumnUser}, '{userBitMask}', {accessColumnGroup}, '{userGroupBitMask}') = 1 ");
            }
            else
            {
                sb.Append($" { tableAlias }.OwnerId = { userId } ");
                sb.Append($" OR CheckAccess({ tableAlias }.{accessColumnUser}, '{userBitMask}', { tableAlias }.{accessColumnGroup}, '{userGroupBitMask}') = 1 ");
            }

            sb.Append(")");

            return sb.ToString();
        }
        #endregion

        #region [Migrate]
        /// <summary>
        /// Creates new columns and converts all existing access rights
        /// </summary>
        public IList<MigrationResult> Migrate()
        {
            var result = new List<MigrationResult>();

            #region Table Role
            if (AddColumns("Role"))
            {
                sqlService.OpenConnection((connection) =>
                {
                    try
                    {
                        var roleIds = connection.Query<Guid>("Select RoleId from Role order by RoleId");
                        MigrateAccessRights(result, "Role", "RoleId", roleIds);                        
                    }
                    catch (Exception ex)
                    {
                        result.Add(new MigrationResult
                        {
                            TableName = "Role",
                            Status = MigrationStatus.Failed,
                            Exception = ex
                        });
                    }

                    return 0;
                });
            }

            MemAlloc.FinalizeCollectedData();
            #endregion            

            #region -ESS_DCC_StackRegister
            if (AddColumns("ESS_DCC_StackRegister"))
            {
                sqlService.OpenConnection((connection) =>
                {
                    try
                    {
                        var ids = connection.Query<Guid>("select RegisterGuid from ESS_DCC_StackRegister order by RegisterGuid");
                        MigrateAccessRights(result, "ESS_DCC_StackRegister", "RegisterGuid", ids);                        
                    }
                    catch (Exception ex)
                    {
                        result.Add(new MigrationResult
                        {
                            TableName = "ESS_DCC_StackRegister",
                            Status = MigrationStatus.Failed,
                            Exception = ex
                        });
                    }

                    return 0;
                });
            }

            MemAlloc.FinalizeCollectedData();
            #endregion            

            #region -ESS_DCC_Structure_Stack_Register
            if (AddColumns("ESS_DCC_Structure_Stack_Register"))
            {
                sqlService.OpenConnection((connection) =>
                {
                    try
                    {
                        var ids = connection.Query<Guid>("select Guid from ESS_DCC_Structure_Stack_Register order by Guid");
                        MigrateAccessRights(result, "ESS_DCC_Structure_Stack_Register", "RegisterGuid", ids);
                    }
                    catch (Exception ex)
                    {
                        result.Add(new MigrationResult
                        {
                            TableName = "ESS_DCC_Structure_Stack_Register",
                            Status = MigrationStatus.Failed,
                            Exception = ex
                        });
                    }

                    return 0;
                });
            }
            MemAlloc.FinalizeCollectedData();
            #endregion

            #region -ESS_MS_Intern_Explorer_Directory
            if (AddColumns("ESS_MS_Intern_Explorer_Directory"))
            {
                sqlService.OpenConnection((connection) =>
                {
                    try
                    {
                        var directories = connection.Query<DirectoryOrFile>("SELECT Ident, ReadGuid, ReadWriteGuid, FullGuid FROM ESS_MS_Intern_Explorer_Directory Order By Ident");

                        MigrateAccessRights(result, "ESS_MS_Intern_Explorer_Directory", "Ident",
                            directories.Select(x => x.ReadGuid), directories.Select(x => x.Ident));
                        MigrateAccessRights(result, "ESS_MS_Intern_Explorer_Directory", "Ident",
                            directories.Select(x => x.ReadWriteGuid), directories.Select(x => x.Ident));
                        MigrateAccessRights(result, "ESS_MS_Intern_Explorer_Directory", "Ident",
                            directories.Select(x => x.FullGuid), directories.Select(x => x.Ident));
                    }
                    catch(Exception ex)
                    {
                        result.Add(new MigrationResult
                        {
                            TableName = "ESS_MS_Intern_Explorer_Directory",
                            Status = MigrationStatus.Failed,
                            Exception = ex
                        });
                    }

                    return 0;
                });
            }

            MemAlloc.FinalizeCollectedData();
            #endregion

            #region -ESS_MS_Intern_Explorer_File
            if (AddColumns("ESS_MS_Intern_Explorer_File"))
            {
                sqlService.OpenConnection((connection) =>
                {
                    try
                    {
                        var files = connection.Query<DirectoryOrFile>("SELECT Ident, ReadGuid, ReadWriteGuid, FullGuid FROM ESS_MS_Intern_Explorer_File Order By Ident");

                        MigrateAccessRights(result, "ESS_MS_Intern_Explorer_File", "Ident",
                            files.Select(x => x.ReadGuid), files.Select(x => x.Ident));

                        MigrateAccessRights(result, "ESS_MS_Intern_Explorer_File", "Ident",
                            files.Select(x => x.ReadWriteGuid), files.Select(x => x.Ident));

                        MigrateAccessRights(result, "ESS_MS_Intern_Explorer_File", "Ident",
                            files.Select(x => x.FullGuid), files.Select(x => x.Ident));
                    }
                    catch(Exception ex)
                    {
                        result.Add(new MigrationResult
                        {
                            TableName = "ESS_MS_Intern_Explorer_File",
                            Status = MigrationStatus.Failed,
                            Exception = ex
                        });                        
                    }

                    return 0;
                });
            }
            MemAlloc.FinalizeCollectedData();
            #endregion

            #region -ESS_MS_Intern_Page
            if (AddColumns("ESS_MS_Intern_Page"))
            {
                sqlService.OpenConnection((connection) =>
                {
                    try
                    {
                        var ids = connection.Query<Guid>("select Guid from ESS_MS_Intern_Page order by Guid");
                        MigrateAccessRights(result, "ESS_MS_Intern_Page", "Guid", ids);
                    }
                    catch (Exception ex)
                    {
                        result.Add(new MigrationResult
                        {
                            TableName = "ESS_MS_Intern_Page",
                            Status = MigrationStatus.Failed,
                            Exception = ex
                        });
                    }

                    return 0;
                });
            }
            MemAlloc.FinalizeCollectedData();
            #endregion

            #region UI_Grid_Menu
            if (AddColumns("UI_Grid_Menu"))
            {
                sqlService.OpenConnection((connection) =>
                {
                    try
                    {
                        var ids = connection.Query<Guid>("select Id from UI_Grid_Menu order by Id");
                        MigrateAccessRights(result, "UI_Grid_Menu", "Id", ids);
                    }
                    catch(Exception ex)
                    {
                        result.Add(new MigrationResult
                        {
                            TableName = "UI_Grid_Menu",
                            Status = MigrationStatus.Failed,
                            Exception = ex
                        });
                    }

                    return 0;
                });
            }
            MemAlloc.FinalizeCollectedData();
            #endregion

            #region -ESS_DCC_Stack
            if (AddColumns("ESS_DCC_Stack"))
            {
                sqlService.OpenConnection((connection) =>
                {
                    try
                    {
                        var ids = connection.Query<Guid>("select Guid from ESS_DCC_Stack order by Guid");
                        MigrateAccessRights(result, "ESS_DCC_Stack", "Guid", ids);
                    }
                    catch(Exception ex)
                    {
                        result.Add(new MigrationResult
                        {
                            TableName = "ESS_DCC_Stack",
                            Status = MigrationStatus.Failed,
                            Exception = ex
                        });
                    }

                    return 0;
                });
            }
            MemAlloc.FinalizeCollectedData();
            #endregion

            #region -ESS_DCC_Stack - TableNames
            // get tables from stack
            foreach (var tbl in GetTablesFromStack())
            {
                sqlService.OpenConnection((connection) =>
                {
                    IEnumerable<Guid> ids = null;

                    if (AddColumns(tbl))
                    {
                        try
                        {
                            ids = connection.Query<Guid>($"select Guid from {tbl} order by Guid");
                            MigrateAccessRights(result, tbl, "Guid", ids);
                        }
                        catch (Exception ex)
                        {
                            result.Add(new MigrationResult
                            {
                                TableName = tbl,
                                Status = MigrationStatus.Failed,
                                Exception = ex
                            });
                        }
                    }

                    MemAlloc.FinalizeCollectedData();

                    try
                    {
                        if (AddColumns($"{tbl}_Classification_Configuration"))
                        {
                            ids = connection.Query<Guid>($"select Guid from {tbl}_Classification_Configuration order by Guid");
                            MigrateAccessRights(result, $"{tbl}_Classification_Configuration", "Guid", ids);
                        }
                    }
                    catch (Exception ex)
                    {
                        result.Add(new MigrationResult
                        {
                            TableName = $"{tbl}_Classification_Configuration",
                            Status = MigrationStatus.Failed,
                            Exception = ex
                        });
                    }

                    MemAlloc.FinalizeCollectedData();

                    return 0;
                });
            }
            #endregion            

            return result;
        }

        #region [Migrate - Private Methods]

        #region RightObject
        /// <summary>
        /// Used only in the migrate method
        /// </summary>
        private class RightObject
        {
            public int GroupIdent { get; set; }
            public int UserIdent { get; set; }
            public int RightType { get; set; }
        }
        #endregion

        #region DirectoryOrFile
        /// <summary>
        /// Used only in the migrate method
        /// </summary>
        private class DirectoryOrFile
        {
            public int Ident { get; set; }
            public Guid ReadGuid { get; set; }
            public Guid ReadWriteGuid { get; set; }
            public Guid FullGuid { get; set; }
        }
        #endregion

        #region GetTablesFromStack
        /// <summary>
        /// Gets a list of table names from the stack table
        /// </summary>
        /// <returns></returns>
        private List<string> GetTablesFromStack()
        {
            return sqlService.OpenConnection((connection) =>
            {
                return connection.Query<string>($"select distinct TableName, " +
                    $" (select name from sysobjects where name = TableName) as Tbl from ESS_DCC_Stack " +
                    $" WHERE Tbl is not null order by TableName").ToList();
            });
        }
        #endregion

        #region MigrateAccessRights
        /// <summary>
        /// Reads all permissions from user right object table and creates bit masks and saves them
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="colName"></param>
        /// <param name="ids"></param>
        /// <param name="idents">if given, will be used as row id</param>
        private void MigrateAccessRights(IList<MigrationResult> migrationResults, string tableName, string colName, IEnumerable<Guid> ids, IEnumerable<int> idents = null)
        {
            int rowCounter = 0;

            sqlService.OpenConnection((connection) =>
            {
                int counter = 0;
                foreach (var id in ids)
                {
                    rowCounter++;

                    if (rowCounter % 500 == 0)
                        MemAlloc.FinalizeCollectedData();

                    IEnumerable<RightObject> rightObjects = null;

                    try
                    {
                        rightObjects = connection.Query<RightObject>("SELECT (select Ident from ESS_MS_Intern_Groups where GroupId = ESS_MS_Intern_RightObjects.GroupId) as GroupIdent, UserIdent, RightType from ESS_MS_Intern_RightObjects where Guid = :Guid and AllowAccess = 1", new { Guid = id });

                        migrationResults.Add(new MigrationResult
                        {
                            TableName = "ESS_MS_Intern_Groups",
                            Status = MigrationStatus.Done
                        });
                    }
                    catch (Exception ex)
                    {
                        migrationResults.Add(new MigrationResult
                        {
                            TableName = "ESS_MS_Intern_Groups",
                            Status = MigrationStatus.Failed,
                            Exception = ex
                        });
                    }

                    if (rightObjects == null) continue;

                    var userReadIds = new List<int>();
                    var userWriteIds = new List<int>();
                    var userFullIds = new List<int>();

                    var groupReadIds = new List<int>();
                    var groupWriteIds = new List<int>();
                    var groupFullIds = new List<int>();

                    foreach (var rightObj in rightObjects)
                    {
                        if (rightObj.GroupIdent > 0 && rightObj.GroupIdent < 32000)
                        {
                            // We need to set full acccess here, because the other access types are never used...
                            groupFullIds.Add(rightObj.GroupIdent);
                        }

                        if (rightObj.UserIdent > 0 && rightObj.UserIdent < 32000)
                        {
                            // We need to set full acccess here, because the other access types are never used...
                            userFullIds.Add(rightObj.UserIdent);
                        }
                    }

                    if (userReadIds.Count == 0 && userWriteIds.Count == 0 && userFullIds.Count == 0 &&
                        groupReadIds.Count == 0 && groupWriteIds.Count == 0 && groupFullIds.Count == 0)
                        continue;
                    
                    userReadIds = userReadIds.Distinct().ToList();
                    userWriteIds = userWriteIds.Distinct().ToList();
                    userFullIds = userFullIds.Distinct().ToList();
                    groupReadIds = groupReadIds.Distinct().ToList();
                    groupWriteIds = groupWriteIds.Distinct().ToList();
                    groupFullIds = groupFullIds.Distinct().ToList();
                    
                    // Sort is a void, so is not part of the method chain above.
                    userReadIds.Sort(); userWriteIds.Sort(); userFullIds.Sort();
                    groupReadIds.Sort(); groupWriteIds.Sort(); groupFullIds.Sort();

                    var newAccessRights = new RowAccess();
                    if (userReadIds.Count > 0) newAccessRights.UserReadAccess = userReadIds;
                    if (userWriteIds.Count > 0) newAccessRights.UserWriteAccess = userWriteIds;
                    if (userFullIds.Count > 0) newAccessRights.UserFullAccess = userFullIds;
                    if (groupReadIds.Count > 0) newAccessRights.GroupReadAccess = groupReadIds;
                    if (groupWriteIds.Count > 0) newAccessRights.GroupWriteAccess = groupWriteIds;
                    if (groupFullIds.Count > 0) newAccessRights.GroupFullAccess = groupFullIds;

                    try
                    {
                        if (idents != null)
                            SetAccess(tableName, colName, idents.ElementAtOrDefault(counter), newAccessRights);
                        else
                            SetAccess(tableName, colName, id, newAccessRights);

                        migrationResults.Add(new MigrationResult
                        {
                            TableName = tableName,
                            Status = MigrationStatus.Done                            
                        });
                    }
                    catch (Exception ex)
                    {
                        migrationResults.Add(new MigrationResult {
                            TableName = tableName,
                            Status = MigrationStatus.Failed,
                            Exception = ex
                        });
                    }

                    counter++;
                }

                return 0;
            });
        }
        #endregion

        #region AddColumns
        /// <summary>
        /// Adds new access rights columns to a given table
        /// </summary>
        /// <param name="table">Table to be altered</param>
        /// <returns>True if the columns were added</returns>
        private bool AddColumns(string table)
        {
            var columnsAdded = true;

            return sqlService.OpenConnection((connection) =>
            {
                try { connection.Execute($"ALTER TABLE {table} ADD \"OwnerId\" INTEGER NULL;"); }
                catch { columnsAdded = false; }

                try { connection.Execute($"ALTER TABLE {table} ADD \"UserReadAccess\" LONG VARBIT NULL;"); }

                catch { columnsAdded = false; }
                try { connection.Execute($"ALTER TABLE {table} ADD \"UserWriteAccess\" LONG VARBIT NULL;"); }

                catch { columnsAdded = false; }
                try { connection.Execute($"ALTER TABLE {table} ADD \"UserFullAccess\" LONG VARBIT NULL;"); }

                catch { columnsAdded = false; }
                try { connection.Execute($"ALTER TABLE {table} ADD \"GroupReadAccess\" LONG VARBIT NULL;"); }
                catch { columnsAdded = false; }

                try { connection.Execute($"ALTER TABLE {table} ADD \"GroupWriteAccess\" LONG VARBIT NULL;"); }
                catch { columnsAdded = false; }

                try { connection.Execute($"ALTER TABLE {table} ADD \"GroupFullAccess\" LONG VARBIT NULL;"); }
                catch { columnsAdded = false; }

                return columnsAdded;
            });
        }
        #endregion

        #endregion

        #endregion

        #region [HasAccess]
        /// <summary>
        /// Checks if the given user has access
        /// </summary>
        /// <param name="type">What kind of statement to be generated</param>
        /// <param name="tableName">Table name</param>
        /// <param name="idColName">Id column name</param>
        /// <param name="rowId">Row id</param>
        /// <param name="tableAlias">Optional table alias e.g. SQL = TableName as tn, tableAlias="tn"</param>
        /// <param name="userId">User id</param>
        /// <param name="userBitMask">User id converted to bit mask</param>
        /// <param name="userGroupBitMask">User access groups converted to bit mask</param> 
        /// <returns>True if the user has access</returns>
        public bool HasAccess(AccessRightType type, string tableName, string idColName, object rowId, int userId, string userBitMask, string userGroupBitMask)
        {
            var accessRights = GenerateAccessRightsStatement(type, userId, userBitMask, userGroupBitMask);

            return sqlService.OpenConnection((connection) =>
            {
                var hasAccess = connection.Query<object>($"SELECT {idColName} FROM {tableName} " +
                    $"WHERE {idColName} = :rowId AND {accessRights}", new { rowId });

                return hasAccess.Any();
            });
        }


        /// <summary>
        /// Checks if the given user has access
        /// </summary>
        /// <param name="type">What kind of statement to be generated</param>
        /// <param name="tableName">Table name</param>
        /// <param name="idColName">Id column name</param>
        /// <param name="rowId">Row id</param>       
        /// <param name="session">Session instance</param>
        /// <returns>True if the user has access</returns>
        public bool HasAccess(AccessRightType type, string tableName, string idColName, object rowId, Session.Session session)
        {
            return HasAccess(type, tableName, idColName, rowId, session.UserId,
                session.UserBitMask, session.UserAccessGroupsBitMask);
        }        

        #endregion

        #endregion
    }
}
