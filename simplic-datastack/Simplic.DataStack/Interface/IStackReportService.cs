using System;
using System.Collections.Generic;

namespace Simplic.DataStack
{
    public interface IStackReportService
    {
        /// <summary>
        /// Execute stack report (print, mail, ...)
        /// </summary>
        /// <param name="reportItem">Report to execute</param>
        /// <param name="instanceDataGuid">Instance data guid</param>
        /// <returns>Status of execution</returns>
        StackReportExecutionResult Execute(StackReport reportItem, Guid instanceDataGuid);

        /// <summary>
        /// Gets the stack report with the given id
        /// </summary>
        /// <param name="strepid"></param>
        /// <returns></returns>
        StackReport GetStackReport(Guid strepid);

        /// <summary>
        /// Loads all Stack Reports with the given stack Guid
        /// </summary>
        /// <param name="stackGuid"></param>
        /// <returns></returns>
        IEnumerable<StackReport> LoadAllStackReports(Guid stackGuid);

        /// <summary>
        /// Delete a list of stack reports
        /// </summary>
        /// <param name="reports">Reports to delete</param>
        void DeleteStackReports(IList<StackReport> reports);

        /// <summary>
        /// Saves all stack reports in this collection
        /// </summary>
        /// <param name="reports"></param>
        void SaveStackReports(IList<StackReport> reports);

        /// <summary>
        /// Saves a stack report
        /// </summary>
        /// <param name="report"></param>
        void SaveStackReport(StackReport report);

        /// <summary>
        /// Invoke stack report flow event
        /// </summary>
        /// <param name="flowEvent">Flow event</param>
        /// <param name="args">Flow arguments</param>
        void InvokeFlowEvent(StackReportFlowEvent flowEvent, ReportFlowEventArgs args);
    }
}
