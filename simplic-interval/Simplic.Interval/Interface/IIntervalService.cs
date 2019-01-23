using Simplic.Data;
using System;

namespace Simplic.Interval
{
    /// <summary>
    /// Access the interval
    /// </summary>
    public interface IIntervalService : IIntervalRepository, IModelIdentity<Guid, Interval>
    {
        /// <summary>
        /// Calculates the next execute
        /// </summary>
        /// <param name="intervalId">unique interval id</param>
        /// <returns>Next execute date</returns>
        DateTime CalculateNextIntervalExecute(Guid intervalId);
    }
}