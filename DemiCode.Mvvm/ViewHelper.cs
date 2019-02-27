using System;
using System.Reflection;
using System.Windows;
using Autofac;
using DemiCode.Mvvm.Messaging;

namespace DemiCode.Mvvm
{
    ///<summary>
    /// Helper methods for view implementations.
    ///</summary>
    public static class ViewHelper
    {
        ///<summary>
        /// Return the viewmodel type for the specified view type by convention. 
        /// If <paramref name="explicitViewModelType"/> is not null, this type is returned.
        ///</summary>
        public static Type GetViewModelType(Type viewType, Type explicitViewModelType)
        {
            if (explicitViewModelType != null)
                return explicitViewModelType;

            if (viewType == null)
                throw new ArgumentNullException("viewType", "View type must be specified");

            // Find type by convention
            var viewTypeFullyQualifiedName = viewType.AssemblyQualifiedName;
            if (viewTypeFullyQualifiedName == null)
                throw new InvalidOperationException("Unable to get fully qualified name for view type");

            var viewModelType = FindViewModelType(viewType.FullName, viewTypeFullyQualifiedName, "Model");

            return viewModelType ?? FindViewModelType(viewType.FullName, viewTypeFullyQualifiedName, "ViewModel");
        }

        ///<summary>
        /// Call the InitializeComponent method (if it exist) for the specified view.
        ///</summary>
        public static void InitializeComponent(UIElement view)
        {
            var initializeComponentMethod = view.GetType().GetMethod("InitializeComponent", BindingFlags.Public | BindingFlags.Instance);
            if (initializeComponentMethod != null)
                initializeComponentMethod.Invoke(view, new object[0]);
        }

        /// <summary>
        /// Given a messenger <paramref name="recipient"/>, optionally get a <see cref="IMessenger"/> instance from <paramref name="lifetimeScope"/> and unregister the recipient object.
        /// </summary>
        /// <remarks>
        /// If <paramref name="recipient"/> is null, no operation is performed.
        /// If <paramref name="lifetimeScope"/> does not contain a <see cref="IMessenger"/> instance, no operation is performed.
        /// </remarks>
        public static void UnregisterFromMessenger(object recipient, ILifetimeScope lifetimeScope)
        {
            UnregisterFromMessenger(recipient, (IComponentContext)lifetimeScope);
        }

        /// <summary>
        /// Given a messenger <paramref name="recipient"/>, optionally get a <see cref="IMessenger"/> instance from <paramref name="componentContext"/> and unregister the recipient object.
        /// </summary>
        /// <remarks>
        /// If <paramref name="recipient"/> is null, no operation is performed.
        /// If <paramref name="componentContext"/> does not contain a <see cref="IMessenger"/> instance, no operation is performed.
        /// </remarks>
        public static void UnregisterFromMessenger(object recipient, IComponentContext componentContext)
        {
            if (recipient == null || componentContext == null)
                return;

            var messenger = componentContext.ResolveOptional<IMessenger>();
            if (messenger != null)
                messenger.Unregister(recipient);
        }

        /// <summary>
        /// Unregister <paramref name="recipient"/> from <paramref name="messenger"/>.
        /// </summary>
        public static void UnregisterFromMessenger(object recipient, IMessenger messenger)
        {
            if (recipient == null || messenger == null)
                return;
            messenger.Unregister(recipient);
        }

        /// <summary>
        /// Return true if <paramref name="viewModelType"/> inherits <see cref="TypedViewModelBase{TModel}"/> at some level.
        /// </summary>
        public static bool IsTypedViewModel(Type viewModelType)
        {
            if (viewModelType == null)
                throw new ArgumentNullException("viewModelType");
            return IsSubclassOfRawGeneric(typeof (TypedViewModelBase<>), viewModelType);
        }

        private static bool IsSubclassOfRawGeneric(Type generic, Type toCheck)
        {
            while (toCheck != null && toCheck != typeof(object))
            {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == cur)
                {
                    return true;
                }
                toCheck = toCheck.BaseType;
            }
            return false;
        }

        private static Type FindViewModelType(string viewTypeFullName, string viewTypeFullyQualifiedName, string postFix)
        {
            var viewModelTypeName = viewTypeFullyQualifiedName.Replace(viewTypeFullName, viewTypeFullName + postFix);

            return Type.GetType(viewModelTypeName);
        }
    }
}