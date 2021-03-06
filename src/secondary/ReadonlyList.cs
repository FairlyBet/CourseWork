#pragma warning disable CS8600
#pragma warning disable CS8602
#pragma warning disable CS8604
#pragma warning disable CS8618
#pragma warning disable CS8625
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace CourseWork
{
    public class ReadOnlyList<T> : IReadOnlyList<T>, ICloneable
    {
        private readonly T[] _innerArray;

        public int Count => _innerArray.Length;

        public T this[int index] => _innerArray[index];


        public ReadOnlyList(params T[] collection)
        {
            _innerArray = new T[collection.Length];
            collection.CopyTo(_innerArray, 0);
        }

        public ReadOnlyList(in IEnumerable<T> collection)
        {
            _innerArray = new T[collection.Count()];
            int i = 0;
            foreach (var item in collection)
            {
                _innerArray[i++] = item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var item in _innerArray)
            {
                yield return item;
            }
        }

        public object Clone()
        {
            var result = new T[_innerArray.Length];
            if (typeof(T) is ICloneable)
            {
                for (int i = 0; i < _innerArray.Length; i++)
                {
                    result[i] = (T)((ICloneable)_innerArray[i]).Clone();
                }
                return new ReadOnlyList<T>(result);
            }
            _innerArray.CopyTo(result, 0);
            return new ReadOnlyList<T>(result);
        }

        public T[] ToArray()
        {
            var result = new T[_innerArray.Length];
            _innerArray.CopyTo(result, 0);
            return result;
        }
    }
}
