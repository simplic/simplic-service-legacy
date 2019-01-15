using Simplic.Cache;
using Simplic.Data.Sql;
using Simplic.Sql;
using System;

namespace Simplic.Interval
{
    /// <summary>
    /// Database access for the interval with standart function from <see cref="SqlRepositoryBase<Guid, Interval>"/>
    /// </summary>
    public class IntervalRepository : SqlRepositoryBase<Guid, Interval>, IIntervalRepository
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sqlService"><see cref="ISqlService></param>
        /// <param name="sqlColumnService"><see cref="ISqlColumnService></param>
        /// <param name="cacheService"><see cref="ICacheService></param>
        public IntervalRepository(ISqlService sqlService, ISqlColumnService sqlColumnService, ICacheService cacheService) : base(sqlService, sqlColumnService, cacheService)
        {
        }

        /// <summary>
        /// Returns the primary key
        /// </summary>
        /// <param name="obj"><see cref="Interval>/param>
        /// <returns><Unique id of <see cref="Guid></returns>
        public override Guid GetId(Interval obj)
        {
            return obj.Guid;
        }

        /// <summary>
        /// Gets the assigned datatable name
        /// </summary>
        public override string TableName { get { return "IT_Interval"; } }

        /// <summary>
        /// Gets the assigned unique key name
        /// </summary>
        public override string PrimaryKeyColumn { get { return "Guid"; } }
    }
}