using System;
using Autofac;

namespace DemiCode.Mvvm
{
    public interface IView : IViewValidation
    {
        string Title { get; }
    }
}
