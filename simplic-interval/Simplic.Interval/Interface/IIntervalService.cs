using Simplic.Data;
using System;

namespace Simplic.Interval
{
    /// <summary>
    /// Access the interval
    /// </summary>
    public interface IIntervalService : IIntervalRepository, IModelIdentity<Guid, Interval>
    {
    }
}