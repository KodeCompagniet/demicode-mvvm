using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace DemiCode.Mvvm
{
    /// <summary>
    /// The <see cref="ViewModelBase"/> class is the base class for all viewmodels.
    /// </summary>
    public abstract class ViewModelBase : NotifyPropertyChangedBase, IDataErrorInfo
    {
        private readonly List<ValidationRule> _validationRules = new List<ValidationRule>();

        /// <summary>
        /// Constructs a new <see cref="ViewModelBase"/>
        /// </summary>
        protected ViewModelBase()
        {
            CloseView = () => { };
        }

        /// <summary>
        /// Constructs a new <see cref="ViewModelBase"/> in design-mode.
        /// </summary>
        /// <param name="context">The design time context.</param>
        protected ViewModelBase(IDesignTimeContext context)
            : this()
        {
            IsInDesignMode = true;
            DesignTimeContext = context;
        }

        /// <summary>
        /// Gets a value indicating whether this viewmodel is currently in design mode.
        /// </summary>
        protected bool IsInDesignMode { get; private set; }

        /// <summary>
        /// Gets the design time context for the view model, when in design mode.
        /// </summary>
        protected IDesignTimeContext DesignTimeContext { get; private set; }

        ///<summary>
        /// View validation service.
        ///</summary>
        public IViewValidation ViewValidation { get; set; }

        /// <summary>
        /// Initializes the ViewModel with the specified model.
        /// </summary>
        /// <param name="model">The model (data).</param>
        public virtual void Initialize(object model)
        {
        }

        /// <summary>
        /// Registers command bindings for the view model.
        /// </summary>
        public virtual void RegisterCommandBindings(ICommandBindingRegistry registry)
        {
        }


        /// <summary>
        /// Gets the list of validation rules for this viewmodel.
        /// </summary>
        public IList<ValidationRule> ValidationRules
        {
            get
            {
                // return _validationRules.AsReadOnly();
                return _validationRules;
            }
        }

        /// <summary>
        /// Validates the property with the specified name. If the validation fails,
        /// an error message is returned, otherwise an empty string. Override this
        /// method to provide custom validation, but remember to call the base
        /// implementation for the validation rules in the <see cref="ValidationRules"/>
        /// collection to be evaluated correctly.
        /// </summary>
        /// <param name="propertyName">The name of the property to validate.</param>
        /// <returns>An error message or an empty string, indicating no error.</returns>
        protected virtual string ValidateProperty(string propertyName)
        {
            // Find the first matching validation rule where a condition is not met
            var rule = _validationRules
                .Where(r => r.PropertyName == propertyName && !r.Condition())
                .FirstOrDefault();
            
            // Evaluate it
            return rule != null ? rule.ErrorMessage : String.Empty;
        }

        /// <summary>
        /// Validates the viewmodel by iterating over all <see cref="ValidationRules"/>. Override
        /// this method to perform custom validation.
        /// </summary>
        /// <remarks>This property first evaluates any rules added to the <see cref="ValidationRules"/> list.
        /// If all rules are satisfied, the <see cref="ViewValidation"/> service is consulted.</remarks>
        /// <returns><c>false</c> if any validation errors exist, otherwise <c>true</c>. Also returns true if <see cref="ViewValidation"/> is null</returns>
        public virtual bool IsValid
        {
            get
            {
                if (_validationRules.Any(rule => !rule.Condition()))
                {
                    return false;
                }
                return ViewValidation == null || ViewValidation.IsValid;
            }
        }

        /// <summary>
        /// Instructs the view to close itself. Ignored by all views that do not derive
        /// from <see cref="WindowView"/>.
        /// </summary>
        public Action CloseView { get; set; }

        #region IDataErrorInfo Members

        string IDataErrorInfo.Error
        {
            get { return string.Empty; }
        }

        string IDataErrorInfo.this[string propertyName]
        {
            get
            {
                string error = ValidateProperty(propertyName);
                return error ?? string.Empty;
            }
        }

        #endregion
    }
}
