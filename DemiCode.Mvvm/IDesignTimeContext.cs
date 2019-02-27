namespace DemiCode.Mvvm
{
    /// <summary>
    /// Specifies a context for use by viewmodels at design time. Passed to the appropriate viewmodel constructor
    /// at design time.
    /// </summary>
    public interface IDesignTimeContext
    {
        /// <summary>
        /// Gets or sets a model containing sample data.
        /// </summary>
        object SampleModel { get; set; }
    }
}