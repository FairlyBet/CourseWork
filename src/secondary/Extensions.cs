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

        public static Performance ReverseToString(this Performance performance, in string value)
        {
            var val = value.Trim().ToLowerInvariant().Replace("performance", string.Empty).Replace(" ", string.Empty);
            return val switch
            {
                "low" => Performance.Low,
                "medium" => Performance.Medium,
                "high" => Performance.High,
                _ => throw new ArgumentException("Invalid value"),
            };
        }
    }
}
