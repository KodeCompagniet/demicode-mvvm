using System.ComponentModel;

namespace DemiCode.Mvvm
{
    public interface IViewValidation : INotifyPropertyChanged
    {
        /// <summary>
        /// Return true if all view validation returns successfully.
        /// </summary>
        bool IsValid { get; }
    }
}
