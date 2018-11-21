using System;

namespace Simplic.Viewer.Annotation
{
    /// <summary>
    /// Interface for generating annotation that are basing on a given document
    /// </summary>
    public interface IGenerateAnnotationService
    {
        /// <summary>
        /// Generate annotation result by using a blob guid
        /// </summary>
        /// <param name="blobId">Unique blob guid</param>
        /// <returns>Annotation result</returns>
        GenerateAnnotationResult Generate(Guid blobId);
    }
}
