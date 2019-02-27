using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Windows;

namespace DemiCode.Mvvm.Wpf
{
	/// <summary>
	/// Type-safe way of registering dependency properties.
	/// </summary>
	/// <typeparam name="TParent">Type of the class that contains the related property</typeparam>
	public static class DependencyHelper<TParent>
	{
		/// <summary>
		/// Type-safe way of registering dependency properties.
		/// </summary>
		/// <typeparam name="TProperty">Type of the property. Inferred through expression.</typeparam>
        /// <param name="property">The related <see cref="DependencyProperty"/>.</param>
		public static DependencyProperty Register<TProperty>(Expression<Func<TParent, TProperty>> property)
		{
			var member = ExpressionHelper.GetPropertyFromExpression(property);
			return DependencyProperty.Register(member.Name, member.PropertyType, member.DeclaringType);
		}

        /// <summary>
        /// Type-safe way of registering read-only dependency properties.
        /// </summary>
        /// <typeparam name="TProperty">Type of the property. Inferred through expression.</typeparam>
        /// <param name="property">The related <see cref="DependencyPropertyKey"/>.</param>
        public static DependencyPropertyKey RegisterReadOnly<TProperty>(Expression<Func<TParent, TProperty>> property)
        {
            var member = ExpressionHelper.GetPropertyFromExpression(property);
            return DependencyProperty.RegisterReadOnly(member.Name, member.PropertyType, member.DeclaringType, new FrameworkPropertyMetadata());
        }

        //public static DependencyProperty RegisterAttachedInherited<TProperty>(Expression<Func<TParent, TProperty>> property)
        //{
        //    var member = ExpressionHelper.GetPropertyFromExpression(property);
        //    var metadata = new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits);
        //    return DependencyProperty.RegisterAttached(member.Name, member.PropertyType, member.DeclaringType, metadata);
        //}

        /// <summary>
        /// Type-safe way of registering an attached dependency property.
        /// </summary>
        /// <typeparam name="TPropertyType">Type of the property. Inferred through expression.</typeparam>
        /// <param name="property">The related <see cref="DependencyPropertyKey"/>.</param>
        public static DependencyProperty RegisterAttached<TPropertyType>(Expression<Func<TParent, TPropertyType>> property)
        {
            var member = ExpressionHelper.GetPropertyFromExpression(property).Name;
            if (member.EndsWith("Property"))
                member = member.Substring(0, member.IndexOf("Property"));
            return RegisterAttached(member, typeof (TPropertyType));
        }

        /// <summary>
        /// Register an attached dependency property.
        /// </summary>
        public static DependencyProperty RegisterAttached(string name, Type propertyType)
        {
            return DependencyProperty.RegisterAttached(name, propertyType, typeof(TParent));
        }

        /// <summary>
        /// Create a dependency property with a <see cref="FrameworkPropertyMetadata"/>-based <see cref="PropertyChangedCallback"/>.
        /// </summary>
        public static DependencyProperty RegisterAttachedWithPropertyChangedCallback<TPropertyType>(string name, PropertyChangedCallback callback)
        {
            var metadata = new FrameworkPropertyMetadata(null, callback);
            return RegisterAttachedWithPropertyMetadata<TPropertyType>(name, metadata);
        }

        /// <summary>
        /// Create a dependency property with a <see cref="UIPropertyMetadata"/>-based <see cref="PropertyChangedCallback"/>.
        /// </summary>
        public static DependencyProperty RegisterAttachedWithUIPropertyChangedCallback<TPropertyType>(string name, PropertyChangedCallback callback)
        {
            var metadata = new UIPropertyMetadata(null, callback);
            return RegisterAttachedWithPropertyMetadata<TPropertyType>(name, metadata);
        }

        /// <summary>
        /// Create a dependency property with a <see cref="FrameworkPropertyMetadata"/> and the <see cref="FrameworkPropertyMetadataOptions.Inherits"/> option.
        /// </summary>
        public static DependencyProperty RegisterAttachedInherited<TPropertyType>(string name)
	    {
	        var metadata = new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits);
            return RegisterAttachedWithPropertyMetadata<TPropertyType>(name, metadata);
	    }

        /// <summary>
        /// Create a dependency property with a <see cref="FrameworkPropertyMetadata"/> and the <see cref="FrameworkPropertyMetadataOptions.Inherits"/> option.
        /// </summary>
        public static DependencyProperty RegisterAttachedInherited(string name, Type propertyType)
	    {
	        var metadata = new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits);
	        return DependencyProperty.RegisterAttached(name, propertyType, typeof(TParent), metadata);
	    }

	    /// <summary>
        /// Create a dependency property with a property metadata.
        /// </summary>
        public static DependencyProperty RegisterAttachedWithPropertyMetadata<TPropertyType>(string name, PropertyMetadata propertyMetadata)
        {
            return DependencyProperty.RegisterAttached(name, typeof(TPropertyType), typeof(TParent), propertyMetadata);
        }

	    /// <summary>
        /// Add a "<see cref="DependencyPropertyDescriptor.AddValueChanged"/> event handler to <paramref name="dependencyProperty"/>.
	    /// </summary>
	    public static void AddValueChangedEventHandler(DependencyProperty dependencyProperty, TParent parent, EventHandler handler)
        {
            var dpDescriptor = DependencyPropertyDescriptor.FromProperty(dependencyProperty, typeof(TParent));
            dpDescriptor.AddValueChanged(parent, handler);
        }
	}
}