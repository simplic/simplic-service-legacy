using Simplic.Data;
using System;
using System.Collections.Generic;

namespace Simplic.TaskScheduler
{
    /// <summary>
    /// Repository interface
    /// </summary>
    public interface ITaskSchedulerConfigurationRepository : IRepositoryBase<Guid, TaskSchedulerConfiguration>
    {

    }
}
