using System;

namespace CourseWork
{
    public static class Extensions
    {
        public static uint NextUint(this Random random, in uint minValue, in uint maxValue)
        {
            return (uint)random.NextInt64(minValue, maxValue);
        }

        public static void ForEach<T>(this ReadOnlyList<T> list, in Action<T> action)
        {
            foreach (var item in list)
            {
                action.Invoke(item);
            }
        }
    }
}
