namespace Simplic.TaskScheduler
{
    /// <summary>
    /// Scheduler service interface
    /// </summary>
    public interface ITaskSchedulerConfigurationService : ITaskSchedulerConfigurationRepository
    {
        /// <summary>
        /// Get cron configuration
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        string GetCronConfiguration(TaskSchedulerConfiguration configuration);

        /// <summary>
        /// Check configuration. Exception with a message will ne thrown, if the configuration is invalid
        /// </summary>
        /// <param name="configuration">Configuration instance</param>
        /// <returns>True if the configuration is correct</returns>
        bool CheckCronConfiguration(TaskSchedulerConfiguration configuration);
    }
}
