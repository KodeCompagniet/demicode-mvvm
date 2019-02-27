using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace DemiCode.Mvvm.Wpf
{
    /// <summary>
    /// An observable list wrapper which will update its underlying list.
    /// </summary>
    /// <typeparam name="T">The type of elements contained by the underlying list.</typeparam>
    public class ObservableListWrapper<T> : ObservableCollectionWrapper<T>, IList<T>
    {
        /// <summary>
        /// Constructs a new <see cref="ObservableListWrapper{T}"/>.
        /// </summary>
        /// <param name="wrappedList">The list to wrap.</param>
        public ObservableListWrapper(IList<T> wrappedList)
            : base(wrappedList)
        {
            WrappedList = wrappedList;
        }

        /// <summary>
        /// Constructs a new <see cref="ObservableListWrapper{T}"/>.
        /// </summary>
        /// <param name="wrappedList">The list to wrap.</param>
        /// <param name="filter">A filter to determine which items in the underlying
        /// collection that will be visible.</param>
        public ObservableListWrapper(IList<T> wrappedList, Func<T, bool> filter)
            : base(wrappedList, filter)
        {
            WrappedList = wrappedList;
        }

        /// <summary>
        /// Gets the wrapped list.
        /// </summary>
        protected IList<T> WrappedList { get; private set; }

        #region Implementation of IList<T>

        /// <summary>
        /// Determines the index of a specific item in the <see cref="T:System.Collections.Generic.IList`1"/>.
        /// </summary>
        /// <returns>
        /// The index of <paramref name="item"/> if found in the list; otherwise, -1.
        /// </returns>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.IList`1"/>.</param>
        public int IndexOf(T item)
        {
            return WrappedList.IndexOf(item);
        }

        /// <summary>
        /// Inserts an item to the <see cref="T:System.Collections.Generic.IList`1"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted.</param><param name="item">The object to insert into the <see cref="T:System.Collections.Generic.IList`1"/>.</param><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.</exception><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1"/> is read-only.</exception>
        public void Insert(int index, T item)
        {
            WrappedList.Insert(index, item);

            RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
        }

        /// <summary>
        /// Removes the <see cref="T:System.Collections.Generic.IList`1"/> item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.</exception><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1"/> is read-only.</exception>
        public void RemoveAt(int index)
        {
            T item = default(T);
            if (WrappedList.Count > index)
                item = WrappedList[index];

            WrappedList.RemoveAt(index);

            RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, index));
        }

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <returns>
        /// The element at the specified index.
        /// </returns>
        /// <param name="index">The zero-based index of the element to get or set.</param><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.</exception><exception cref="T:System.NotSupportedException">The property is set and the <see cref="T:System.Collections.Generic.IList`1"/> is read-only.</exception>
        public T this[int index]
        {
            get { return WrappedList[index]; }
            set
            {
                T oldItem = default(T);
                if (WrappedList.Count > index)
                    oldItem = WrappedList[index];

                WrappedList[index] = value;

                RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, value, oldItem, index));
            }
        }

        #endregion
    }
}
