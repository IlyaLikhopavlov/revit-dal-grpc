namespace Revit.DAL.Utils
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Immediately executes the given action on each element in the source sequence.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the sequence</typeparam>
        /// <param name="source">The sequence of elements</param>
        /// <param name="action">The action to execute on each element</param>

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (action == null) throw new ArgumentNullException(nameof(action));

            foreach (var element in source)
                action(element);
        }

        public static void For<T>(this T[] source, Action<int, T> action, int startIndex = 0, int endIndex = 0)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (action == null) throw new ArgumentNullException(nameof(action));

            if (source.Length == 0)
                return;

            if (endIndex <= 0 || endIndex > source.Length)
                endIndex = source.Length;

            if (startIndex < 0 || startIndex > endIndex)
                startIndex = 0;

            for (var i = startIndex; i <= endIndex; i++)
                action(i, source[i]);
        }
    }
}
