using System;
using System.Globalization;

namespace DemiCode.Mvvm
{
    /// <summary>
    /// A viewmodel base class for typical editor-style views
    /// </summary>
    /// <typeparam name="TModel">The type of <see cref="ViewModelBase"/> to use.</typeparam>
    public abstract class EditViewModelBase<TModel> : ViewModelBase
        where TModel : class
    {
        /// <summary>
        /// Gets the model.
        /// </summary>
        protected TModel Model { get; private set; }

        /// <summary>
        /// Initializes the viewmodel to use the specified model. The <paramref name="model"/>
        /// argument may be null.
        /// </summary>
        /// <param name="model">The model for the viewmodel.</param>
        public void Initialize(TModel model)
        {
            if (model != null)
            {
                Model = model;
            }
            else
            {
                Model = CreateNewModel();
            }

            OnInitialize();
        }

        protected abstract TModel CreateNewModel();

        protected abstract void RefreshModel();

        protected abstract void UpdateModel();

        protected abstract bool IsModified { get; }

        public virtual bool CanSave
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Called after the view has loaded and the viewmodel has been initialized with the
        /// view's <c>DataContext</c>. The base method does nothing. Override this to
        /// initialize your viewmodel.
        /// </summary>
        protected virtual void OnInitialize()
        {
        }

        /// <summary>
        /// Seals the <see cref="base.Initialize(object)"/> method and calls the typed
        /// <see cref="Initialize(TModel)"/> if the <paramref name="model"/> argument is
        /// non-null and of type <typeparamref name="TModel"/>.
        /// </summary>
        /// <param name="model">The model for the viewmodel.</param>
        /// <exception cref="ArgumentException">The <paramref name="model"/> argument
        /// is non-null and not of type <typeparamref name="TModel"/>.</exception>
        public sealed override void Initialize(object model)
        {
            if (model != null && !(model is TModel))
            {
                var message = string.Format(CultureInfo.InvariantCulture,
                    "The viewmodel of type '{0}' was initialized with a model of type '{1}'. The model should be of type {2}."
                    + " Check the ViewModelType attribute of the view and the type of its data context.",
                    this.GetType().FullName,
                    model.GetType().FullName,
                    typeof(TModel).FullName);
                throw new ArgumentException(message, "model");
            }

            Initialize((TModel)model);
        }
    }
}
