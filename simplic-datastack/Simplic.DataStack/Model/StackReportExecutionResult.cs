namespace Simplic.DataStack
{
    /// <summary>
    /// StackReportExecutionResult
    /// </summary>
    public enum StackReportExecutionResult
    {
        Success,
        ActionNotSupported,
        ReportTypeNotSupported,
        ReportNotFound,
        PythonClassBeforeNotFound,
        PythonClassAfterNotFound,
        Canceled,
        UnknownError
    }
}
