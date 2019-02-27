using System.Collections.Generic;
using System.Linq;

namespace DemiCode.Mvvm.Messaging
{
    internal static class ListExtensions
    {
        public static List<T> Clone<T>(this IEnumerable<T> list)
        {
            // ReSharper disable once PossibleMultipleEnumeration
            return list.Take(list.Count()).ToList();
        }
    }
}