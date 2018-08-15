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
        private int monthNrofExecution;
        private int typeNumber;

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
        public Guid Guid { get => guid; set => guid = value; }
        /// <summary>
        /// Gets or sets the name (enum) of the start day 
        /// </summary>
        public int DayNameOfExecution { get => dayNameOfExecution; set => dayNameOfExecution = value; }
        /// <summary>
        /// Gets or sets the number of a day in the selected month where to start
        /// </summary>
        public int DayNumberOfExecution { get => dayNumberOfExecution; set => dayNumberOfExecution = value; }
        /// <summary>
        /// Gets or sets the count of exections of this interval
        /// </summary>
        public int ExecuteCount { get => executeCount; set => executeCount = value; }
        /// <summary>
        /// Gets or sets the date of the last execution
        /// </summary>
        public DateTime LastExecute { get => lastExecute; set => lastExecute = value; }
        /// <summary>
        /// Gets or sets the selected month by number 
        /// </summary>
        public int MonthNrofExecution { get => monthNrofExecution; set => monthNrofExecution = value; }
        /// <summary>
        /// Gets or sets the number of the selected type
        /// </summary>
        public int TypeNumber { get => typeNumber; set => typeNumber = value; }

        #endregion
    }
}
