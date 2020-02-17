using Dapper;
using Simplic.Data;
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
        private readonly IIntervalRepository intervalRepository;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sqlService"></param>
        public IntervalService(ISqlService sqlService, IIntervalRepository intervalRepository)
        {
            this.sqlService = sqlService;
            this.intervalRepository = intervalRepository;
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
                return connection.Query<Interval>("Select * from IT_Interval order by LastExecute");
            });
        }

        /// <summary>
        /// Gets a single interval
        /// </summary>
        /// <param name="intervalId"></param>
        /// <returns>Interval</returns>
        public Interval Get(Guid intervalId)
        {
            return intervalRepository.Get(intervalId);
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
        public bool Delete(Guid intervalId)
        {
            return intervalRepository.Delete(intervalId);
        }

        /// <summary>
        /// Saves the interval
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool IRepositoryBase<Guid, Interval>.Save(Interval obj)
        {
            return intervalRepository.Save(obj);
        }

        /// <summary>
        /// Removes the interval by object
        /// </summary>
        /// <param name="Interval"></param>
        public bool Delete(Interval obj)
        {
            return intervalRepository.Delete(obj);
        }

        /// <summary>
        /// Gets the unique id
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public Guid GetId(Interval obj)
        {
            return obj.Guid;
        }

        #endregion Public methods
    }
}