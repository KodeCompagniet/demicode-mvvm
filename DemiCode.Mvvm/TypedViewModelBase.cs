using System;
using System.Globalization;

namespace DemiCode.Mvvm
{
    /// <summary>
    /// <see cref="TypedViewModelBase{TModel}"/> is the base class for viewmodels with a specific model type
    /// (ie. most viewmodels). A typed <see cref="Initialize(TModel)"/> is introduced, and and the
    /// untyped <see cref="Initialize(object)"/> of <see cref="ViewModelBase"/> is set to call
    /// the typed version and sealed. A typed <see cref="Model"/> property is also introduced.
    /// </summary>
    /// <typeparam name="TModel">The type of the model going into this view</typeparam>
    public abstract class TypedViewModelBase<TModel> : ViewModelBase
    {
        /// <summary>
        /// Constructs a new <see cref="TypedViewModelBase{TModel}"/>.
        /// </summary>
        protected TypedViewModelBase()
            : base() { }

        /// <summary>
        /// Constructs a new <see cref="TypedViewModelBase{TModel}"/> in design-mode.
        /// </summary>
        /// <param name="context">The design time context.</param>
        protected TypedViewModelBase(IDesignTimeContext context)
            : base(context) { }

        /// <summary>
        /// Gets the model for this viewmodel.
        /// </summary>
        protected TModel Model { get; private set; }

        /// <summary>
        /// Initializes the viewmodel to use the specified model. The <paramref name="model"/>
        /// argument may be null.
        /// </summary>
        /// <param name="model">The model for the viewmodel.</param>
        public void Initialize(TModel model)
        {
            Model = model;
            OnInitialize();
        }

        /// <summary>
        /// Called after the view has loaded and the viewmodel has been initialized with the
        /// view's <c>DataContext</c> (placed in <see cref="Model"/>). The base method does nothing. Override this to
        /// initialize your viewmodel.
        /// </summary>
        protected virtual void OnInitialize()
        {
        }

        /// <summary>
        /// Seals the <see cref="Initialize(object)"/> method and calls the typed
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
                var message = String.Format(CultureInfo.InvariantCulture,
                    "The viewmodel of type '{0}' was initialized with a model of type '{1}'. The model should be of type {2}."
                    + " Check the ViewModelType attribute of the view and the type of its data context.",
                    GetType().FullName,
                    model.GetType().FullName,
                    typeof(TModel).FullName);
                throw new ArgumentException(message, "model");
            }

            if (model == null)
                Initialize(default(TModel));
            else
                Initialize((TModel)model);
        }
    }
}
