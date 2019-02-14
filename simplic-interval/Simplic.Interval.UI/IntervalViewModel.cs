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
            intervalService = CommonServiceLocator.ServiceLocator.Current.GetInstance<IIntervalService>();
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

        #endregion Public methods

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
        public int DayNumberOfExecution
        {
            get
            {
                return model.DayNumberOfExecution;
            }
            set
            {
                if (value == 0 && model.DayNumberOfExecution < 0)
                    value = 1;

                if (value == 0 && model.DayNumberOfExecution > 0)
                    value = -1;

                PropertySetter(value, (newValue) => { model.DayNumberOfExecution = newValue; });
            }
        }

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
        public int MonthNumberOfExecution { get { return model.MonthNumberofExecution; } set { PropertySetter(value, (newValue) => { model.MonthNumberofExecution = newValue; }); } }

        /// <summary>
        /// Gets or sets the number of the selected type
        /// </summary>
        public int IntervalTypeId
        {
            get
            {
                return model.IntervalTypeId;
            }
            set
            {
                PropertySetter(value, (newValue) => { model.IntervalTypeId = newValue; });
                RaisePropertyChanged(nameof(DayByNumber));
                RaisePropertyChanged(nameof(DayByName));
                RaisePropertyChanged(nameof(MonthNumber));
                RaisePropertyChanged(nameof(MonthMaximum));
            }
        }

        /// <summary>
        /// Gets the enabled state of the day number control
        /// </summary>
        public bool DayByNumber
        {
            get
            {
                if ((IntervalDefinition)IntervalTypeId != IntervalDefinition.MonthlyDay) return true;
                return false;
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

        /// <summary>
        /// Gets the enabled state of the month number control
        /// </summary>
        public bool MonthNumber
        {
            get
            {
                if ((IntervalDefinition)IntervalTypeId == IntervalDefinition.Yearly
                    || (IntervalDefinition)IntervalTypeId == IntervalDefinition.HalfYearly) return true;
                return false;
            }
        }

        /// <summary>
        /// Gets the maximum vor month select control
        /// </summary>
        public int MonthMaximum
        {
            get
            {
                if ((IntervalDefinition)IntervalTypeId == IntervalDefinition.HalfYearly) return 5;
                if ((IntervalDefinition)IntervalTypeId == IntervalDefinition.Quarterly) return 2;
                return 12;
            }
        }

        /// <summary>
        /// Gets the model
        /// </summary>
        public Interval Model { get => model; }

        #endregion Public member
    }
}