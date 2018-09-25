using System.Collections.Generic;

namespace Simplic.TaskScheduler
{
    /// <summary>
    /// Repository interface
    /// </summary>
    public interface ITaskSchedulerConfigurationRepository
    {
        /// <summary>
        /// Get all configurations
        /// </summary>
        /// <returns>Configuration list</returns>
        IEnumerable<TaskSchedulerConfiguration> GetAll();

        /// <summary>
        /// Save configuration to database
        /// </summary>
        /// <param name="configuration">Configuration instnace</param>
        /// <returns>True if successfull</returns>
        bool Save(TaskSchedulerConfiguration configuration);

        /// <summary>
        /// Delete configuration from database
        /// </summary>
        /// <param name="configuration">Configuration instance</param>
        /// <returns>True if successfull</returns>
        bool Delete(TaskSchedulerConfiguration configuration);
    }
}
