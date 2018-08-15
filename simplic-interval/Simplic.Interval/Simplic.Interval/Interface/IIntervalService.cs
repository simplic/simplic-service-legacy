using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.Interval
{
    public interface IIntervalService
    {
        /// <summary>
        /// Gets all available configured intervals
        /// </summary>
        /// <returns>Intervals</returns>
        IEnumerable<Interval> GetAll();

        /// <summary>
        /// Gets a single interval
        /// </summary>
        /// <param name="intervalId"></param>
        /// <returns>Interval</returns>
        Interval Get(Guid intervalId);

        /// <summary>
        /// Write interval 
        /// </summary>
        /// <param name="interval"></param>
        void Save(Interval interval);

        /// <summary>
        /// Removes the interval by id
        /// </summary>
        /// <param name="intervalId"></param>
        void Delete(Guid intervalId);
    }
}
