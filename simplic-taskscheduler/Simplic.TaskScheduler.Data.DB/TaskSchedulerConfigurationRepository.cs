using Newtonsoft.Json;
using Simplic.Framework.Repository;
using System;
using System.Collections.Generic;

namespace Simplic.TaskScheduler.Data.DB
{
    /// <summary>
    /// Repository implementation
    /// </summary>
    public class TaskSchedulerConfigurationRepository : ITaskSchedulerConfigurationRepository
    {
        /// <summary>
        /// Base path within the directory
        /// </summary>
        public const string BasePath = "/public/TaskScheduler/";

        /// <summary>
        /// Delete configuration from database
        /// </summary>
        /// <param name="configuration">Configuration instance</param>
        /// <returns>True if successfull</returns>
        public bool Delete(TaskSchedulerConfiguration configuration)
        {
            RepositoryManager.Singleton.DeleteFromDatabase($"{BasePath}{configuration.Id}.json");

            return true;
        }

        /// <summary>
        /// Delete configuration from database
        /// </summary>
        /// <param name="id">Configuration id</param>
        /// <returns>True if successfull</returns>
        public bool Delete(Guid id)
        {
            RepositoryManager.Singleton.DeleteFromDatabase($"{BasePath}{id}.json");

            return true;
        }

        /// <summary>
        /// Get all configurations
        /// </summary>
        /// <returns>Configuration list</returns>
        public IEnumerable<TaskSchedulerConfiguration> GetAll()
        {
            var directory = RepositoryManager.Singleton.GetDirectory(BasePath);
            if (directory != null)
            {
                foreach (var content in RepositoryManager.Singleton.GetDirectoryContent(directory.Guid))
                {
                    yield return JsonConvert.DeserializeObject<TaskSchedulerConfiguration>(content.ContentAsString);
                }
            }
        }

        /// <summary>
        /// Get by id
        /// </summary>
        /// <returns>Configuration instance</returns>
        public TaskSchedulerConfiguration Get(Guid id)
        {
            return JsonConvert.DeserializeObject<TaskSchedulerConfiguration>(RepositoryManager.Singleton.ReadAllText($"{BasePath}{id}.json"));
        }

        /// <summary>
        /// Save configuration to database
        /// </summary>
        /// <param name="configuration">Configuration instnace</param>
        /// <returns>True if successfull</returns>
        public bool Save(TaskSchedulerConfiguration configuration)
        {
            var content = RepositoryManager.Singleton.CreateRepositoryContent(configuration.Id, $"{BasePath}{configuration.Id}.json");
            
            RepositoryManager.Singleton.SaveToDatabase(content);
            RepositoryManager.Singleton.WriteAllText(content.Id, JsonConvert.SerializeObject(configuration));

            return true;
        }
    }
}
