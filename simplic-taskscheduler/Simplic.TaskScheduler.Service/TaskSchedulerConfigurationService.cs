using System;
using System.Collections.Generic;

namespace Simplic.TaskScheduler.Service
{
    /// <summary>
    /// Service implementation
    /// </summary>
    public class TaskSchedulerConfigurationService : ITaskSchedulerConfigurationService
    {
        private readonly ITaskSchedulerConfigurationRepository repositoryService;

        /// <summary>
        /// Initialize configuration service
        /// </summary>
        /// <param name="repositoryService">Service repository</param>
        public TaskSchedulerConfigurationService(ITaskSchedulerConfigurationRepository repositoryService)
        {
            this.repositoryService = repositoryService;
        }

        /// <summary>
        /// Check configuration. Exception with a message will ne thrown, if the configuration is invalid
        /// </summary>
        /// <param name="configuration">Configuration instance</param>
        /// <returns>True if the configuration is correct</returns>
        public bool CheckCronConfiguration(TaskSchedulerConfiguration configuration)
        {
            return true;
        }

        /// <summary>
        /// Delete configuration from database
        /// </summary>
        /// <param name="configuration">Configuration instance</param>
        /// <returns>True if successfull</returns>
        public bool Delete(TaskSchedulerConfiguration configuration)
        {
            return repositoryService.Delete(configuration);
        }

        /// <summary>
        /// Delete configuration from database
        /// </summary>
        /// <param name="id">Configuration id</param>
        /// <returns>True if successfull</returns>
        public bool Delete(Guid id)
        {
            return repositoryService.Delete(id);
        }

        /// <summary>
        /// Get by id
        /// </summary>
        /// <returns>Configuration instance</returns>
        public TaskSchedulerConfiguration Get(Guid id)
        {
            return repositoryService.Get(id);
        }

        /// <summary>
        /// Get all configurations
        /// </summary>
        /// <returns>Configuration list</returns>
        public IEnumerable<TaskSchedulerConfiguration> GetAll()
        {
            return repositoryService.GetAll();
        }

        /// <summary>
        /// Get cron configuration
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public string GetCronConfiguration(TaskSchedulerConfiguration configuration)
        {
            return $"{configuration.Minute} {configuration.Hour} {configuration.Day} {configuration.Month} {configuration.DayOfWeek}";
        }

        /// <summary>
        /// Save configuration to database
        /// </summary>
        /// <param name="configuration">Configuration instnace</param>
        /// <returns>True if successfull</returns>
        public bool Save(TaskSchedulerConfiguration configuration)
        {
            return repositoryService.Save(configuration);
        }
    }
}
