using Simplic.Framework.UI;
using System;

namespace Simplic.Interval.UI
{
    public class IntervalViewModel : ExtendableViewModel
    {
        #region Fields

        private Interval model;
        private IIntervalService intervalService;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="interval"></param>
        public IntervalViewModel(Interval interval)
        {
            intervalService =  CommonServiceLocator.ServiceLocator.Current.GetInstance<IIntervalService>();
            model = interval;
        }

        #endregion Constructor

        #region Public methods

        /// <summary>
        /// Writes the changes to database
        /// </summary>
        public void Save()
        {
            intervalService.Save(this.model);
        }

        #endregion

        #region Public member

        /// <summary>
        /// Gets or sets the unique id
        /// </summary>
        public Guid Guid { get { return model.Guid; } set { PropertySetter(value, (newValue) => { model.Guid = newValue; }); } }

        /// <summary>
        /// Gets or sets the name (enum) of the start day
        /// </summary>
        public int DayNameOfExecution { get { return model.DayNameOfExecution; } set { PropertySetter(value, (newValue) => { model.DayNameOfExecution = newValue; }); ; } }

        /// <summary>
        /// Gets or sets the number of a day in the selected month where to start
        /// </summary>
        public int DayNumberOfExecution { get { return model.DayNumberOfExecution; } set { PropertySetter(value, (newValue) => { model.DayNumberOfExecution = newValue; }); } }

        /// <summary>
        /// Gets or sets the count of exections of this interval
        /// </summary>
        public int ExecuteCount { get { return model.ExecuteCount; } set { PropertySetter(value, (newValue) => { model.ExecuteCount = newValue; }); } }

        /// <summary>
        /// Gets or sets the date of the last execution
        /// </summary>
        public DateTime LastExecute { get { return model.LastExecute; } set { PropertySetter(value, (newValue) => { model.LastExecute = newValue; }); } }

        /// <summary>
        /// Gets or sets the selected month by number
        /// </summary>
        public int MonthNumberOfExecution { get { return model.MonthNumberOfExecution; } set { PropertySetter(value, (newValue) => { model.MonthNumberOfExecution = newValue; }); } }

        /// <summary>
        /// Gets or sets the number of the selected type
        /// </summary>
        public int IntervalTypeId { get { return model.IntervalTypeId; } set { PropertySetter(value, (newValue) => { model.IntervalTypeId = newValue; }); } }

        /// <summary>
        /// Gets the enabled state of the day number control 
        /// </summary>
        public bool DayByNumber
        {
            get
            {
                if ((IntervalDefinition)IntervalTypeId == IntervalDefinition.Monthly) return false;
                return true;
            }
        }

        /// <summary>
        /// Gets the enabled state of the day by name control 
        /// </summary>
        public bool DayByName
        {
            get
            {
                return !DayByNumber;
            }
        }

        #endregion Public member
    }
}