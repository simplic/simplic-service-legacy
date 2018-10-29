using System;

namespace Simplic.DataStack
{
    /// <summary>
    /// Stack service interface
    /// </summary>
    public interface IStackService
    {
        /// <summary>
        /// Get the content of the instancedata as string
        /// </summary>
        /// <param name="stackGuid">Guid of the stack</param>
        /// <param name="instanceDataGuid">Guid of the instancedata</param>
        /// <returns>All values of the instancedata as string</returns>
        string GetInstanceDataContent(Guid stackGuid, Guid instanceDataGuid);


        /// <summary>
        /// Get the current stack table name by id
        /// </summary>
        /// <param name="stackGuid">Unique stack id</param>
        /// <returns>Table name</returns>
        string GetTableName(Guid stackGuid);

        /// <summary>
        /// Get the current stack name by id
        /// </summary>
        /// <param name="stackName">Stack name</param>
        /// <returns>Stack guid</returns>
        Guid GetStackId(string stackName);
    }
}