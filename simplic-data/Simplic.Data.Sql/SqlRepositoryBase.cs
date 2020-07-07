using Dapper;
using Simplic.Cache;
using Simplic.Change.Tracking;
using Simplic.Sql;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Simplic.Data.Sql
{
    /// <summary>
    /// Connection cache
    /// </summary>
    internal static class ConnectionInfo
    {
        private static IDictionary<string, string> connections;

        public static IDictionary<string, string> Connections { get => connections; set => connections = value; }
    }

    /// <summary>
    /// Sql repository base implementation
    /// </summary>
    /// <typeparam name="TModel">Model</typeparam>
    /// <typeparam name="TId">Id</typeparam>
    public abstract class SqlRepositoryBase<TId, TModel> : IRepositoryBase<TId, TModel>, IModelIdentity<TId, TModel>
    {
        private readonly ISqlService sqlService;
        private readonly ISqlColumnService sqlColumnService;
        private readonly ICacheService cacheService;
        private readonly IChangeTrackingService changeTrackingService;


        /// <summary>
        /// Initialize sql service
        /// </summary>
        /// <param name="sqlService">Sql service</param>
        /// <param name="sqlColumnService">Sql column service</param>
        [Obsolete]
        public SqlRepositoryBase(ISqlService sqlService, ISqlColumnService sqlColumnService, ICacheService cacheService)
        {
            this.sqlService = sqlService;
            this.sqlColumnService = sqlColumnService;
            this.cacheService = cacheService;
            changeTrackingService = CommonServiceLocator.ServiceLocator.Current.GetInstance<IChangeTrackingService>();
        }

        /// <summary>
        /// Initialize sql service
        /// </summary>
        /// <param name="sqlService">Sql service</param>
        /// <param name="sqlColumnService">Sql column service</param>
        public SqlRepositoryBase(ISqlService sqlService, ISqlColumnService sqlColumnService, ICacheService cacheService, IChangeTrackingService requestChangeService)
        {
            this.sqlService = sqlService;
            this.sqlColumnService = sqlColumnService;
            this.cacheService = cacheService;
            this.changeTrackingService = requestChangeService;
        }


        /// <summary>
        /// Get data by id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Instance of <see cref="TModel"/> if exists</returns>
        public virtual TModel Get(TId id)
        {

            return GetByColumn<TId>(PrimaryKeyColumn, id);
        }

        /// <summary>
        /// Get a model instance from the database, using a custom column name
        /// </summary>
        /// <typeparam name="T">Id type</typeparam>
        /// <param name="columnName">Column name</param>
        /// <param name="id">Id value</param>
        /// <returns>Model if exists</returns>
        protected virtual TModel GetByColumn<T>(string columnName, T id)
        {
            var key = $"{TableName}_{columnName}_{id}";

            TModel obj = default(TModel);
            if (UseCache)
            {
                obj = cacheService.Get<TModel>(key);
                if (obj != null)
                {
                    try
                    {
                        if (obj is ITrackable trackable)
                        {
                            trackable.Snapshot = changeTrackingService.CreateDeepCopy<TModel>(obj);
                            return obj;
                        }
                        else
                        {
                            return obj;
                        }

                    }
                    catch (Exception)
                    {
                        
                    }

                }

            }

            return sqlService.OpenConnection((connection) =>
            {
                obj = connection.Query<TModel>($"SELECT * FROM {TableName} WHERE {columnName} = :id",
                    new { id = id }).FirstOrDefault();

                if (UseCache)
                    cacheService.Set<TModel>(key, obj);

                return CreateSnaphot(obj);

            }, GetConnection());
        }

        /// <summary>
        /// Get all objects
        /// </summary>
        /// <returns>Enumerable of <see cref="TModel"/></returns>
        public virtual IEnumerable<TModel> GetAll()
        {
            return sqlService.OpenConnection((connection) =>
            {
                return connection.Query<TModel>($"SELECT * FROM {TableName} ORDER BY {PrimaryKeyColumn}")
                                                .Select(CreateSnaphot);
            }, GetConnection());
        }

        private TModel CreateSnaphot(TModel item)
        {
            if (item is ITrackable trackable)
                trackable.Snapshot = changeTrackingService.CreateDeepCopy<TModel>(item);

            return item;
        }

        /// <summary>
        /// Get all objects where a given column value match
        /// </summary>
        /// <returns>Enumerable of <see cref="TModel"/></returns>
        protected virtual IEnumerable<TModel> GetAllByColumn<T>(string columnName, T id)
        {
            return sqlService.OpenConnection((connection) =>
            {
                return connection.Query<TModel>($"SELECT * FROM {TableName} WHERE {columnName} = :id ORDER BY {columnName}",
                    new { id = id });
            }, GetConnection());
        }

        /// <summary>
        /// Create or update data
        /// </summary>
        /// <param name="obj">Object to save</param>
        /// <returns>True if successful</returns>
        public virtual bool Save(TModel obj)
        {
            var columns = sqlColumnService.GetModelDBColumnNames(TableName, obj.GetType(), DifferentColumnNames);
            try
            {
                if (changeTrackingService.IsTrackable<TModel>(obj))
                {

                    changeTrackingService.TrackChange<TModel, TId>(obj, CrudType.Update, TableName, null, GetId(obj));
                }
                else
                {
                    var snapshot = Get(GetId(obj));
                    CrudType crudType = CrudType.Insert;
                    if (snapshot == null)
                    {
                        crudType = CrudType.Insert;
                    }
                    else
                    {
                        crudType = CrudType.Update;
                    }
                    changeTrackingService.TrackChange<TModel, TId>(obj, crudType, TableName, snapshot, GetId(obj));
                }

            }
            catch (Exception)
            {
                
            }


            return sqlService.OpenConnection((connection) =>
            {
                if (UseCache)
                {
                    var key = $"{TableName}_{PrimaryKeyColumn}_{GetId(obj)}";
                    cacheService.Remove<TModel>(key);
                }

                string sqlStatement = $"INSERT INTO {TableName} ({string.Join(", ", columns.Select(item => item.Key))}) ON EXISTING UPDATE VALUES "
                    + $" ({string.Join(", ", columns.Select(k => ":" + (string.IsNullOrWhiteSpace(k.Value) ? k.Key : k.Value)))});";
                return connection.Execute(sqlStatement, obj) > 0;
            }, GetConnection());
        }

        /// <summary>
        /// Delete data
        /// </summary>
        /// <param name="obj">Object to delete</param>
        /// <returns>True if successful</returns>
        public virtual bool Delete(TModel obj)
        {
            try
            {
                if (changeTrackingService.IsTrackable<TModel>(obj))
                {
                    changeTrackingService.TrackChange<TModel, TId>(obj, CrudType.Delete, TableName, null, GetId(obj));
                }
                else
                {
                    changeTrackingService.TrackChange<TModel, TId>(obj, CrudType.Delete, TableName, Get(GetId(obj)), GetId(obj));
                }
            }
            catch (Exception)
            {
                
            }

            return sqlService.OpenConnection((connection) =>
            {
                return connection.Execute($"DELETE FROM {TableName} WHERE {PrimaryKeyColumn} = :id",
                    new { id = GetId(obj) }) > 0;
            }, GetConnection());
        }

        /// <summary>
        /// Delete data by id
        /// </summary>
        /// <param name="obj">Object to delete</param>
        /// <returns>True if successful</returns>
        public virtual bool Delete(TId id)
        {
            return sqlService.OpenConnection((connection) =>
            {
                return connection.Execute($"DELETE FROM {TableName} WHERE {PrimaryKeyColumn} = :id",
                    new { id = id }) > 0;
            }, GetConnection());
        }

        /// <summary>
        /// Gets the repository group if exists
        /// </summary>
        /// <returns>group name</returns>
        private string CheckGroupAttribute()
        {
            var attributes = this.GetType().GetCustomAttributes(true);

            if (attributes.Any())
            {
                var groupAttribute = attributes.OfType<RepositoryGroupAttribute>().SingleOrDefault();

                if (groupAttribute != null)
                {
                    return groupAttribute.GroupName;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the connection name
        /// </summary>
        /// <returns>connection name</returns>
        public string GetConnection()
        {
            string connectionName = "default";

            if (ConnectionInfo.Connections == null)
                ConnectionInfo.Connections = new Dictionary<string, string>();

            if (ConnectionInfo.Connections.ContainsKey(this.GetType().Name))
            {
                connectionName = ConnectionInfo.Connections[this.GetType().Name];
            }
            else
            {
                var groupName = CheckGroupAttribute();

                if (!string.IsNullOrWhiteSpace(groupName))
                {
                    connectionName = sqlService.OpenConnection((connection) =>
                    {
                        var obj = connection.Query<string>($"SELECT c.mnd_name FROM ESS_DC_BASE_DBConnection_RepositoryGroup g join ESS_DC_BASE_DBConnection c on c.id = g.ConnectionId  WHERE g.Name = :name",
                            new { name = groupName }).FirstOrDefault();

                        return obj;
                    });
                }

                ConnectionInfo.Connections.Add(this.GetType().Name, connectionName);

            }

            return connectionName;
        }

        /// <summary>
        /// Gets the id of a model
        /// </summary>
        /// <param name="obj">Model to get the id of</param>
        /// <returns>Id value</returns>
        public abstract TId GetId(TModel obj);

        /// <summary>
        /// Gets the current table name
        /// </summary>
        public abstract string TableName { get; }

        /// <summary>
        /// Gets the current primary column name
        /// </summary>
        public abstract string PrimaryKeyColumn { get; }

        /// <summary>
        /// Gets a list of different column names
        /// </summary>
        public virtual IDictionary<string, string> DifferentColumnNames { get; private set; }

        /// <summary>
        /// Gets or sets whether to use the cache
        /// </summary>
        public virtual bool UseCache { get; set; } = false;




    }


}