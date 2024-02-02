using System.Collections.Generic;

namespace UsefulExtensions.Dictionary
{
    public static class DictionaryExtensions
    {
        public static T2 AddWithReturn<T1, T2>(this Dictionary<T1, T2> dictionary, T1 key, T2 value)
        {
            dictionary.Add(key, value);
            return value;
        }

        public static (T1, T2) AddWithReturnTuple<T1, T2>(this Dictionary<T1, T2> dictionary, T1 key, T2 value)
        {
            dictionary.Add(key, value);
            return (key, value);
        }
    }
}
