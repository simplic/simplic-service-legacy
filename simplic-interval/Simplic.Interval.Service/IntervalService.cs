﻿using Dapper;
using Simplic.Data;
using Simplic.Framework.DAL.DatabaseMetadata;
using Simplic.Sql;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Simplic.Interval.Service
{
    public class IntervalService : IIntervalService
    {
        #region Fields

        private readonly ISqlService sqlService;
        private readonly IIntervalRepository intervalRepository;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sqlService"></param>
        public IntervalService(ISqlService sqlService, IIntervalRepository intervalRepository)
        {
            this.sqlService = sqlService;
            this.intervalRepository = intervalRepository;
        }

        #endregion Constructor

        #region Public methods

        /// <summary>
        /// Gets all available configured intervals
        /// </summary>
        /// <returns>Intervals</returns>
        public IEnumerable<Interval> GetAll()
        {
            return sqlService.OpenConnection((connection) =>
            {
                return connection.Query<Interval>("Select * from IT_Interval order by LastExecute");
            });
        }

        /// <summary>
        /// Gets a single interval
        /// </summary>
        /// <param name="intervalId"></param>
        /// <returns>Interval</returns>
        public Interval Get(Guid intervalId)
        {
            return intervalRepository.Get(intervalId);
        }

        /// <summary>
        /// Write interval
        /// </summary>
        /// <param name="interval"></param>
        public void Save(Interval interval)
        {
            if (interval == null)
                throw new ArgumentNullException(nameof(interval));

            interval.Guid = GuidUtility.GetNewIfNullOrEmpty(interval.Guid);
            var dlColumns = ColumnHelper.GetModelDBColumnNames("IT_Interval", typeof(Interval), null);

            sqlService.OpenConnection((connection) =>
            {
                string setDLStatement = $"INSERT INTO IT_Interval ({string.Join(", ", dlColumns.Select(item => item.Key))}) ON EXISTING UPDATE VALUES "
                    + $" ({string.Join(", ", dlColumns.Select(k => ":" + (string.IsNullOrWhiteSpace(k.Value) ? k.Key : k.Value)))});";
                return connection.Execute(setDLStatement, interval);
            });
        }

        /// <summary>
        /// Removes the interval by id
        /// </summary>
        /// <param name="intervalId"></param>
        public bool Delete(Guid intervalId)
        {
            return intervalRepository.Delete(intervalId);
        }

        /// <summary>
        /// Saves the interval
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool IRepositoryBase<Guid, Interval>.Save(Interval obj)
        {
            return intervalRepository.Save(obj);
        }

        /// <summary>
        /// Removes the interval by object
        /// </summary>
        /// <param name="Interval"></param>
        public bool Delete(Interval obj)
        {
            return intervalRepository.Delete(obj);
        }

        /// <summary>
        /// Gets the unique id
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public Guid GetId(Interval obj)
        {
            return obj.Guid;
        }

        /// <summary>
        /// Calculates the next execute
        /// </summary>
        /// <param name="intervalItem">TransactionItemInterval</param>
        /// <returns>Next execute date</returns>
        public DateTime CalculateNextIntervalExecute(Guid intervalId)
        {
            var nextExecute = DateTime.MinValue;
            var interval = intervalRepository.Get(intervalId);
            var intervalType = (IntervalDefinition)interval.IntervalTypeId;
            int month = interval.MonthNumberofExecution;
            int day = interval.DayNumberOfExecution;
            int dayName = interval.DayNameOfExecution;

            switch (intervalType)
            {
                case IntervalDefinition.HalfYearly:
                    nextExecute = GetNextExecuteByHalfYear(month, day);
                    break;

                case IntervalDefinition.MonthlyDay:
                    nextExecute = GetNextExecuteByMonthlyDayName(dayName);
                    break;

                case IntervalDefinition.MonthlyDayNumber:
                    nextExecute = GetNextExecuteByMonthly(day);
                    break;

                case IntervalDefinition.Quarterly:
                    nextExecute = GetNextExecuteByQuarter(day);
                    break;

                case IntervalDefinition.Yearly:
                    nextExecute = GetNextExecuteByYearly(month, day);
                    break;
            }
            return nextExecute;
        }



        #endregion Public methods

        #region Private methods

        /// <summary>
        ///  Get the next execute for the Quarter
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        private DateTime GetNextExecuteByQuarter(int day)
        {
            int quarter = (DateTime.Now.Month + 2) / 3;
            int startMonth = ((quarter - 1) * 3) + 1;
            int year = DateTime.Now.Year;
            var current = new DateTime(year, startMonth, day);
            if (current >= DateTime.Now)
            {
                return current;
            }
            else
            {
                quarter = quarter + 1;
                if (quarter > 4)
                {
                    quarter = 1;
                    year = year + 1;
                }
                startMonth = ((quarter - 1) * 3) + 1;
                return new DateTime(year, startMonth, day);
            }
        }

        /// <summary>
        /// Get the next execute for monthly
        /// </summary>
        /// <param name="dayWeek"></param>
        /// <returns></returns>
        private DateTime GetNextExecuteByMonthlyDayName(int dayWeek)
        {
            var month = DateTime.Now.Month;
            var year = DateTime.Now.Year;

            var current = new DateTime(DateTime.Now.Year, month, GetFirstDayOfWeek(year, month, dayWeek));
            if (current >= DateTime.Now)
            {
                return current;
            }
            else
            {
                month = month + 1;
                if (month > 12)
                {
                    year = year + 1;
                    month = 1;
                }
                return new DateTime(year, month, GetFirstDayOfWeek(year, month, dayWeek));
            }
        }

        /// <summary>
        /// Gets the first day of week ind the month
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="dayWeek"></param>
        /// <returns></returns>
        private int GetFirstDayOfWeek(int year, int month, int dayWeek)
        {
            for (int n = 1; n < 8; n++)
            {
                var current = new DateTime(year, month, n);
                if ((int)current.DayOfWeek == dayWeek)
                {
                    return current.Day;
                }
            }
            return 0;
        }

        /// <summary>
        /// Get the next execute for monthly
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        private DateTime GetNextExecuteByMonthly(int day)
        {
            var month = DateTime.Now.Month;
            var current = new DateTime(DateTime.Now.Year, month, day);
            if (current >= DateTime.Now)
            {
                return current;
            }
            else
            {
                var year = DateTime.Now.Year;
                month = month + 1;
                if (month > 12)
                {
                    year = year + 1;
                    month = 1;
                }
                return new DateTime(year, month, day);
            }
        }

        /// <summary>
        /// Get the next execute for yearly
        /// </summary>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        private DateTime GetNextExecuteByYearly(int month, int day)
        {
            var current = new DateTime(DateTime.Now.Year, month, day);
            if (current >= DateTime.Now)
            {
                return current;
            }
            else
            {
                return new DateTime(DateTime.Now.Year + 1, month, day);
            }
        }

        /// <summary>
        /// Get the next execute for half year
        /// </summary>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        private DateTime GetNextExecuteByHalfYear(int month, int day)
        {
            int startMonth = 1;
            if ((DateTime.Now.Month / 6) > 1)
                startMonth = 7;
            startMonth = startMonth + (month - 1);
            var current = new DateTime(DateTime.Now.Year, startMonth, day);
            if (current >= DateTime.Now)
            {
                return current;
            }
            else
            {
                int year = DateTime.Now.Year;
                startMonth = 7;
                if ((DateTime.Now.Month / 6) > 1)
                {
                    startMonth = 1;
                    year = year + 1;
                }
                startMonth = startMonth + (month - 1);
                return new DateTime(year, startMonth, day);
            }
        }

        #endregion
    }
}