using System.Collections;
using System.Collections.Generic;
using System.Windows.Data;
using Autofac.Util;

namespace DemiCode.Mvvm.Data
{
    ///<summary>
    /// A strongly typed implementation of <see cref="CollectionView"/>.
    ///</summary>
    ///<typeparam name="T">Type of the items contained in the collection</typeparam>
    public class TypedCollectionView<T> : CollectionView, IEnumerable<T>
    {
        ///<summary>
        /// Construct a new instance and pass in the collection to view.
        ///</summary>
        public TypedCollectionView(IEnumerable<T> collection)
            : base(collection)
        {
        }

        #region Implementation of IEnumerable<out T>

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return new NonGenericEnumerator(base.GetEnumerator());
        }

        #endregion

        private class NonGenericEnumerator : Disposable, IEnumerator<T>
        {
            private readonly IEnumerator _nonGenericEnumerator;

            public NonGenericEnumerator(IEnumerator nonGenericEnumerator)
            {
                _nonGenericEnumerator = nonGenericEnumerator;
            }

            #region Implementation of IEnumerator

            public bool MoveNext()
            {
                return _nonGenericEnumerator.MoveNext();
            }

            public void Reset()
            {
                _nonGenericEnumerator.Reset();
            }

            public T Current
            {
                get { return (T)_nonGenericEnumerator.Current; }
            }

            object IEnumerator.Current
            {
                get { return _nonGenericEnumerator.Current; }
            }

            #endregion
        }

    }
}