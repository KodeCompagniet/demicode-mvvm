using System;

namespace DemiCode.Mvvm
{
    /// <summary>
    /// Holds a context for use by viewmodels at design time. Passed to the appropriate viewmodel constructor
    /// at design time.
    /// </summary>
    public class DesignTimeContext : IDesignTimeContext
    {
        /// <summary>
        /// Constructs a new <see cref="DesignTimeContext"/>
        /// </summary>
        public DesignTimeContext()
        {
            
        }

        /// <summary>
        /// Gets or sets a model containing sample data.
        /// </summary>
        public object SampleModel { get; set; }
    }
}
