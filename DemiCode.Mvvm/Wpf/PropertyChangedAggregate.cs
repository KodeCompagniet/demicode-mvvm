using System;
using System.ComponentModel;

namespace DemiCode.Mvvm.Wpf
{
    /// <summary>
    /// An NPC that aggregates <see cref="INotifyPropertyChanged.PropertyChanged"/> events from one or more sources and re-raise the event.
    /// </summary>
    public class PropertyChangedAggregate : NotifyPropertyChangedBase
    {
        /// <summary>
        /// Construct the instance.
        /// </summary>
        /// <param name="instancesToMonitor">Sources to monitor</param>
        public PropertyChangedAggregate(params INotifyPropertyChanged[] instancesToMonitor)
        {
            if (instancesToMonitor.Length == 0)
                throw new ArgumentException(@"Instances to monitor cannot be empty", "instancesToMonitor");

            foreach (var pc in instancesToMonitor)
            {
                pc.PropertyChanged += RaisePropertyChanged;
            }
        }

        private void RaisePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(sender, e);
        }
    }
}