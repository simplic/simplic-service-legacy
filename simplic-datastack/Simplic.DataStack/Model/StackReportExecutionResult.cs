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
        ClassBeforeNotFound,
        ClassAfterNotFound,
        Canceled,
        MethodAfterFalse,
        UnknownError
    }
}
