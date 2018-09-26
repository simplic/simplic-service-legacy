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
    public abstract class SqlRepositoryBase<T, I> : IRepositoryBase<T, I>
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
        /// <returns>Instance of <see cref="T"/> if exists</returns>
        public T Get(I id)
        {
            T obj = default(T);
            if (UseCache)
            {
                obj = cacheService.Get<T>(id?.ToString());
                if (obj != null)
                    return obj;
            }

            return sqlService.OpenConnection((connection) =>
            {
                obj = connection.Query<T>($"SELECT * FROM {TableName} WHERE {PrimaryKeyColumn} = :id",
                    new { id = id }).FirstOrDefault();

                if (UseCache)
                    cacheService.Set<T>(id?.ToString(), obj);

                return obj;
            });
        }

        /// <summary>
        /// Get all objects
        /// </summary>
        /// <returns>Enumerable of <see cref="T"/></returns>
        public IEnumerable<T> GetAll()
        {
            return sqlService.OpenConnection((connection) =>
            {
                return connection.Query<T>($"SELECT * FROM {TableName} ORDER BY {PrimaryKeyColumn}");
            });
        }

        /// <summary>
        /// Create or update data
        /// </summary>
        /// <param name="obj">Object to save</param>
        /// <returns>True if successful</returns>
        public bool Save(T obj)
        {
            var columns = sqlColumnService.GetModelDBColumnNames(TableName, typeof(T), DifferentColumnNames);

            return sqlService.OpenConnection((connection) =>
            {
                if (UseCache)
                    cacheService.Remove<T>(GetId(obj).ToString());

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
        public bool Delete(T obj)
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
        public abstract I GetId(T obj);

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
