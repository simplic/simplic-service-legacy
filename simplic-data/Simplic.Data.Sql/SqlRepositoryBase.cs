using Dapper;
using Simplic.Cache;
using Simplic.Data;
using Simplic.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.Data.Sql
{
    /// <summary>
    /// Sql repository base implementation
    /// </summary>
    /// <typeparam name="TModel">Model</typeparam>
    /// <typeparam name="TId">Id</typeparam>
    public abstract class SqlRepositoryBase<TId, TModel> : IRepositoryBase<TId, TModel>
    {
        private readonly ISqlService sqlService;
        private readonly ISqlColumnService sqlColumnService;
        private readonly ICacheService cacheService;

        /// <summary>
        /// Initialize sql service
        /// </summary>
        /// <param name="sqlService">Sql service</param>
        /// <param name="sqlColumnService">Sql column service</param>
        public SqlRepositoryBase(ISqlService sqlService, ISqlColumnService sqlColumnService, ICacheService cacheService)
        {
            this.sqlService = sqlService;
            this.sqlColumnService = sqlColumnService;
            this.cacheService = cacheService;
        }

        /// <summary>
        /// Get data by id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Instance of <see cref="TModel"/> if exists</returns>
        public TModel Get(TId id)
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
        protected TModel GetByColumn<T>(string columnName, T id)
        {
            var key = $"{TableName}_{columnName}_{id}";

            TModel obj = default(TModel);
            if (UseCache)
            {
                obj = cacheService.Get<TModel>(key);
                if (obj != null)
                    return obj;
            }

            return sqlService.OpenConnection((connection) =>
            {
                obj = connection.Query<TModel>($"SELECT * FROM {TableName} WHERE {columnName} = :id",
                    new { id = id }).FirstOrDefault();

                if (UseCache)
                    cacheService.Set<TModel>(key, obj);

                return obj;
            });
        }

        /// <summary>
        /// Get all objects
        /// </summary>
        /// <returns>Enumerable of <see cref="TModel"/></returns>
        public IEnumerable<TModel> GetAll()
        {
            return sqlService.OpenConnection((connection) =>
            {
                return connection.Query<TModel>($"SELECT * FROM {TableName} ORDER BY {PrimaryKeyColumn}");
            });
        }

        /// <summary>
        /// Get all objects where a given column value match
        /// </summary>
        /// <returns>Enumerable of <see cref="TModel"/></returns>
        protected IEnumerable<TModel> GetAllByColumn<T>(string columnName, T id)
        {
            return sqlService.OpenConnection((connection) =>
            {
                return connection.Query<TModel>($"SELECT * FROM {TableName} WHERE {columnName} = :id",
                    new { id = id });
            });
        }

        /// <summary>
        /// Create or update data
        /// </summary>
        /// <param name="obj">Object to save</param>
        /// <returns>True if successful</returns>
        public bool Save(TModel obj)
        {
            var columns = sqlColumnService.GetModelDBColumnNames(TableName, typeof(TModel), DifferentColumnNames);

            return sqlService.OpenConnection((connection) =>
            {
                if (UseCache)
                {
                    var key = $"{TableName}_{PrimaryKeyColumn}_{GetId(obj)}";
                    cacheService.Remove<TModel>(key);
                }

                string sqlStatement = $"INSERT INTO {TableName} ({string.Join(", ", columns.Select(item => item.Key))}) ON EXISTING UPDATE VALUES "
                    + $" ({string.Join(", ", columns.Select(k => "?" + (string.IsNullOrWhiteSpace(k.Value) ? k.Key : k.Value) + "?"))});";

                return connection.Execute(sqlStatement, obj) > 0;
            });
        }

        /// <summary>
        /// Delete data
        /// </summary>
        /// <param name="obj">Object to delete</param>
        /// <returns>True if successful</returns>
        public bool Delete(TModel obj)
        {
            return sqlService.OpenConnection((connection) =>
            {
                return connection.Execute($"DELETE FROM {TableName} WHERE {PrimaryKeyColumn} = :id",
                    new { id = GetId(obj) }) > 0;
            });
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
