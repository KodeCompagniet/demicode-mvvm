using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace DemiCode.Mvvm.Wpf
{
    /// <summary>
    /// An observable collection wrapper which will update its underlying collection.
    /// </summary>
    /// <typeparam name="T">The type of elements contained by the underlying collection.</typeparam>
    public class ObservableCollectionWrapper<T> : ICollection<T>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        /// <summary>
        /// Constructs a new <see cref="ObservableCollectionWrapper{T}"/>.
        /// </summary>
        /// <param name="wrappedCollection">The collection to wrap.</param>
        public ObservableCollectionWrapper(ICollection<T> wrappedCollection)
        {
            WrappedCollection = wrappedCollection;
        }

        /// <summary>
        /// Constructs a new <see cref="ObservableCollectionWrapper{T}"/>.
        /// </summary>
        /// <param name="wrappedCollection">The collection to wrap.</param>
        /// <param name="filter">A filter to determine which items in the underlying
        /// collection that will be visible.</param>
        public ObservableCollectionWrapper(ICollection<T> wrappedCollection, Func<T, bool> filter)
        {
            WrappedCollection = wrappedCollection;
            Filter = filter;
        }

        /// <summary>
        /// Gets the wrapped collection.
        /// </summary>
        protected ICollection<T> WrappedCollection { get; private set; }

        /// <summary>
        /// Gets the wrapped collection as an <see cref="IEnumerable"/>, with the filter, if set, applied.
        /// </summary>
        protected IEnumerable<T> FilteredCollection
        {
            get
            {
                if (Filter != null)
                    return WrappedCollection.Where(Filter);
                else
                    return WrappedCollection;
            }
        }

        /// <summary>
        /// Gets or sets a filter to determine which items in the underlying
        /// collection that will be visible.
        /// </summary>
        public Func<T, bool> Filter { get; private set; }

        #region Implementation of IEnumerable

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator<T> GetEnumerator()
        {
            return FilteredCollection.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Implementation of ICollection<T>

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
        public void Add(T item)
        {
            WrappedCollection.Add(item);

            // If the filter applies to the new item, the collection has changed
            if (Filter == null || Filter(item))
            {
                RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
            }
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only. </exception>
        public void Clear()
        {
            bool changed = FilteredCollection.Count() > 0;

            WrappedCollection.Clear();

            if (changed)
            {
                RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"/> contains a specific value.
        /// </summary>
        /// <returns>
        /// true if <paramref name="item"/> is found in the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false.
        /// </returns>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        public bool Contains(T item)
        {
            return FilteredCollection.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1"/>. The <see cref="T:System.Array"/> must have zero-based indexing.</param><param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param><exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than 0.</exception><exception cref="T:System.ArgumentException"><paramref name="array"/> is multidimensional.-or-The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1"/> is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.-or-Type <paramref name="T"/> cannot be cast automatically to the type of the destination <paramref name="array"/>.</exception>
        public void CopyTo(T[] array, int arrayIndex)
        {
            FilteredCollection.ToArray().CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <returns>
        /// true if <paramref name="item"/> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false. This method also returns false if <paramref name="item"/> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
        public bool Remove(T item)
        {
            // Oops, ugly.. The CollectionChanged event for action = Remove needs the item's index..
            // ICollections are really unordered. Let's just use the enumeration ordering..
            var index = 0;
            foreach (var element in FilteredCollection)
            {
                if (Object.ReferenceEquals(element, item))
                    break;
                index++;
            }
            if (index == FilteredCollection.Count())
                index = -1;

            var removed = WrappedCollection.Remove(item);

            if (removed && (Filter == null || Filter(item)))
            {
                RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, index));
            }
            return removed;
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <returns>
        /// The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        public int Count
        {
            get { return FilteredCollection.Count(); }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
        /// </summary>
        /// <returns>
        /// true if the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only; otherwise, false.
        /// </returns>
        public bool IsReadOnly
        {
            get { return WrappedCollection.IsReadOnly; }
        }

        #endregion

        #region Implementation of INotifyCollectionChanged

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        #endregion

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        /// <summary>
        /// Raises the PropertyChanged event.
        /// </summary>
        /// <param name="args">A <see cref="NotifyCollectionChangedEventArgs"/> that describes the
        /// change to the collection.</param>
        public void RaiseCollectionChanged(NotifyCollectionChangedEventArgs args)
        {
            RaisePropertyChanged("Count");
            RaisePropertyChanged("Items[]");

            var handler = CollectionChanged;
            if (handler != null)
                handler(this, args);
        }

        /// <summary>
        /// Raises the PropertyChanged event.
        /// </summary>
        /// <param name="propertyName"></param>
        private void RaisePropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
