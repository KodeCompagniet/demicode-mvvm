using System;

namespace DemiCode.Mvvm.Helpers
{
    /// <summary>
    /// Stores a Predicate without causing a hard reference
    /// to be created to the Predicate's owner. The owner can be garbage collected at any time.
    /// </summary>
    /// <typeparam name="T">The type of the Predicate's parameter.</typeparam>
    public class WeakPredicate<T> : IFilterWithObject
    {
        private readonly Predicate<T> _predicate;
        private WeakReference _reference;

        /// <summary>
        /// Initializes a new instance of the <see cref="WeakPredicate{T}" /> class.
        /// </summary>
        /// <param name="target">The action's owner.</param>
        /// <param name="predicate">The action that will be associated to this instance.</param>
        public WeakPredicate(object target, Predicate<T> predicate)
        {
            if (target == null)
                throw new ArgumentNullException("target");
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            _reference = new WeakReference(target);
            _predicate = predicate;
        }

        /// <summary>
        /// Gets the Action associated to this instance.
        /// </summary>
        public Predicate<T> Predicate
        {
            get
            {
                return _predicate;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the Action's owner is still alive, or if it was collected
        /// by the Garbage Collector already.
        /// </summary>
        public bool IsAlive
        {
            get
            {
                return _reference != null && _reference.IsAlive;
            }
        }

        /// <summary>
        /// Gets the Action's owner. This object is stored as a <see cref="WeakReference" />.
        /// </summary>
        public object Target
        {
            get
            {
                return _reference == null ? null : _reference.Target;
            }
        }

        /// <summary>
        /// Sets the reference that this instance stores to null.
        /// </summary>
        public void MarkForDeletion()
        {
            _reference = null;
        }

        /// <summary>
        /// Executes a predicate.
        /// </summary>
        /// <param name="parameter">A parameter passed as an object,
        /// to be casted to the appropriate type.</param>
        public bool FilterWithObject(T parameter)
        {
            if (_predicate != null && IsAlive)
            {
                return _predicate(parameter);
            }
            return false;
        }

        public bool FilterWithObject(object parameter)
        {
            var parameterCasted = (T)parameter;
            return FilterWithObject(parameterCasted);
        }

        public override bool Equals(object obj)
        {
            var other = obj as WeakPredicate<T>;
            return Equals(this, other);
        }

        /// <summary>
        /// Compare this instance to <paramref name="right"/> instance.
        /// </summary>
        protected bool Equals(WeakPredicate<T> right)
        {
            return Equals(this, right);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                // ReSharper disable once NonReadonlyMemberInGetHashCode
                return ((_predicate != null ? _predicate.GetHashCode() : 0)*397) ^ (_reference != null ? _reference.GetHashCode() : 0);
            }
        }

        /// <summary>
        /// Is <paramref name="left"/> equal to <paramref name="right"/>?
        /// </summary>
        public static bool operator ==(WeakPredicate<T> left, WeakPredicate<T> right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Is <paramref name="left"/> different from <paramref name="right"/>?
        /// </summary>
        public static bool operator !=(WeakPredicate<T> left, WeakPredicate<T> right)
        {
            return !Equals(left, right);
        }

        private static bool Equals(WeakPredicate<T> left, WeakPredicate<T> right)
        {
            if (ReferenceEquals(left, right))
                return true;
            if (!ReferenceEquals(left, null) && !ReferenceEquals(right, null))
                return left._predicate.Equals(right._predicate) && left._reference.Target.Equals(right._reference.Target);
            return false;
        }
    }
}