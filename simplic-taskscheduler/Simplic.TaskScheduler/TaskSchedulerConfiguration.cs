using System;

namespace Simplic.TaskScheduler
{
    /// <summary>
    /// Task scheduler configuration
    /// </summary>
    public class TaskSchedulerConfiguration
    {
        /// <summary>
        /// Gets or sets the unique id
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the current machine name
        /// </summary>
        public string MachineName
        {
            get;
            set;
        } = Environment.MachineName;

        /// <summary>
        /// Gets or sets the application server name
        /// </summary>
        public string AppServerName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets whether the scheduler is private
        /// </summary>
        public bool IsPrivate { get; set; }

        /// <summary>
        /// Gets or sets the execution mode
        /// </summary>
        public ExecutionTimeMode ExecutionTimeMode { get; set; } = ExecutionTimeMode.Cron;

        /// <summary>
        /// Gets or sets the recurring seconds
        /// </summary>
        public int Seconds { get; set; }

        /// <summary>
        /// Gets or sets cron "minute" settings
        /// </summary>
        public string Minute { get; set; } = "*";

        /// <summary>
        /// Gets or sets cron "hour" settings
        /// </summary>
        public string Hour { get; set; } = "*";

        /// <summary>
        /// Gets or sets cron "day" settings
        /// </summary>
        public string Day { get; set; } = "*";

        /// <summary>
        /// Gets or sets cron "month" settings
        /// </summary>
        public string Month { get; set; } = "*";

        /// <summary>
        /// Gets or sets cron "DayOfWeek" settings
        /// </summary>
        public string DayOfWeek { get; set; } = "*";

        /// <summary>
        /// Gets or sets whether the job is active
        /// </summary>
        public bool IsActive { get; set; }
    }
}
