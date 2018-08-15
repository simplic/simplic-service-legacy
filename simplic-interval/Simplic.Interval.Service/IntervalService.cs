using Dapper;
using Simplic.Framework.DAL.DatabaseMetadata;
using Simplic.Sql;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Simplic.Interval.Service
{
    public class IntervalService : IIntervalService
    {
        #region Fields

        private readonly ISqlService sqlService;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sqlService"></param>
        public IntervalService()
        {
            this.sqlService = CommonServiceLocator.ServiceLocator.Current.GetInstance<Simplic.Sql.ISqlService>();
        }

        #endregion Constructor

        #region Public methods

        /// <summary>
        /// Gets all available configured intervals
        /// </summary>
        /// <returns>Intervals</returns>
        public IEnumerable<Interval> GetAll()
        {
            return sqlService.OpenConnection((connection) =>
            {
                return connection.Query<Interval>("Select * from IT_Interval");
            });
        }

        /// <summary>
        /// Gets a single interval
        /// </summary>
        /// <param name="intervalId"></param>
        /// <returns>Interval</returns>
        public Interval Get(Guid intervalId)
        {
            return sqlService.OpenConnection((connection) =>
            {
                return connection.Query<Interval>("Select * from IT_Interval where Guid = :id",new {id= intervalId }).SingleOrDefault();
            });
        }

        /// <summary>
        /// Write interval 
        /// </summary>
        /// <param name="interval"></param>
        public void Save(Interval interval)
        {
            if (interval == null)
                throw new ArgumentNullException(nameof(interval));

            interval.Guid = GuidUtility.GetNewIfNullOrEmpty(interval.Guid);
            var dlColumns = ColumnHelper.GetModelDBColumnNames("IT_Interval", typeof(Interval), null);

            sqlService.OpenConnection((connection) =>
            {
                string setDLStatement = $"INSERT INTO IT_Interval ({string.Join(", ", dlColumns.Select(item => item.Key))}) ON EXISTING UPDATE VALUES "
                    + $" ({string.Join(", ", dlColumns.Select(k => ":" + (string.IsNullOrWhiteSpace(k.Value) ? k.Key : k.Value)))});";
                return connection.Execute(setDLStatement, interval);
            });
        }

        /// <summary>
        /// Removes the interval by id
        /// </summary>
        /// <param name="intervalId"></param>
        public void Delete(Guid intervalId)
        {
            sqlService.OpenConnection((connection) =>
            {
                connection.Execute("Delete from IT_Interval  WHERE Guid = :id", new { id = intervalId });
            });
        }

        #endregion Public methods
    }
}