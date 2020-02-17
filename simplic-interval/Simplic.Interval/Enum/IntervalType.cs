using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.Interval
{
    /// <summary>
    /// Defines the interval type
    /// </summary>
    public enum IntervalDefinition : byte
    {
        /// <summary>
        /// interval everey month by weekday
        /// </summary>
        MonthlyDay = 0,

        /// <summary>
        /// interval everey quarter of the year
        /// </summary>
        Quarterly = 1,

        /// <summary>
        /// interval everey half of the year
        /// </summary>
        HalfYearly = 2,

        /// <summary>
        /// interval ones in a year
        /// </summary>
        Yearly = 3,

        /// <summary>
        /// interval everey month by day number
        /// </summary>
        MonthlyDayNumber = 4,
    }
}
