using System.Collections.Generic;

namespace Dependra.Common
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Returns an empty collection if the sequence is null.
        /// </summary>
        /// <param name="sequence">The sequence to return an empty collection for if it is empty.</param>
        /// <typeparam name="T">The type of the elements of <paramref name="sequence"/>.</typeparam>
        /// <returns>An <see cref="IEnumerable{T}"/> that contains an empty collection if <paramref name="sequence"/> is null; otherwise, <paramref name="sequence"/>.</returns>
        public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> sequence)
        {
            return sequence ?? new List<T>();
        }
    }
}