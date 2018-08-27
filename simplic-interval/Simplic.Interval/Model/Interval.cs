using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.Interval
{
    /// <summary>
    /// Interval definition
    /// </summary>
    public class Interval
    {
        #region Fields

        private Guid guid;
        private int dayNameOfExecution;
        private int dayNumberOfExecution;
        private int executeCount;
        private DateTime lastExecute;
        private int monthNumberOfExecution;
        private int intervalTypeId;

        #endregion

        #region  Constrcutor
        /// <summary>
        /// Constructor
        /// </summary>
        public Interval()
        {
        }

        #endregion

        #region Public member

        /// <summary>
        /// Gets or sets the unique id
        /// </summary>
        public Guid Guid { get { return guid; } set { guid = value; } }
        /// <summary>
        /// Gets or sets the name (enum) of the start day 
        /// </summary>
        public int DayNameOfExecution { get { return dayNameOfExecution; } set { dayNameOfExecution = value; } }
        /// <summary>
        /// Gets or sets the number of a day in the selected month where to start
        /// </summary>
        public int DayNumberOfExecution { get { return dayNumberOfExecution; } set { dayNumberOfExecution = value; } }
        /// <summary>
        /// Gets or sets the count of exections of this interval
        /// </summary>
        public int ExecuteCount { get { return executeCount; } set { executeCount = value; } }
        /// <summary>
        /// Gets or sets the date of the last execution
        /// </summary>
        public DateTime LastExecute { get { return lastExecute; } set { lastExecute = value; } }
        /// <summary>
        /// Gets or sets the selected month by number 
        /// </summary>
        public int MonthNumberOfExecution { get { return monthNumberOfExecution; } set { monthNumberOfExecution = value; } }
        /// <summary>
        /// Gets or sets the number of the selected type
        /// </summary>
        public int IntervalTypeId { get { return intervalTypeId; } set { intervalTypeId = value; } }

        #endregion
    }
}
