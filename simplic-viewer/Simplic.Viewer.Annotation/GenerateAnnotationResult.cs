namespace Simplic.Viewer.Annotation
{
    /// <summary>
    /// Generation result
    /// </summary>
    public class GenerateAnnotationResult
    {
        /// <summary>
        /// Gets or sets the annotation text
        /// </summary>
        public string Text
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the error text
        /// </summary>
        public string ErrorText
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets whether generating was successfull
        /// </summary>
        public bool IsSuccessful
        {
            get;
            set;
        } = true;
    }
}
