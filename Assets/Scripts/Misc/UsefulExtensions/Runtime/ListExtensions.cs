using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using Random = System.Random;

namespace UsefulExtensions.List
{
    public static class ListExtensions
    {
        private static Random rng = new Random(System.DateTime.UtcNow.Second);

        public static void SwapWithLast<T>(this IList<T> list, int i)
        {
            SwapElements(list, i, list.Count - 1);
        }

        public static void ShuffleList<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                SwapElements(list, n, k);
            }
        }

        public static bool IsInRange<T>(this List<T> list, int index)
        {
            return (index >= 0) && (index < list.Count);
        }

        public static void ShuffleList<T>(this List<T> list, int startIndex, int endIndex)
        {
            var rng = new Random(UnityEngine.Random.Range(0, 1000));

            for (int i = startIndex; i < endIndex; i++)
            {
                int k = rng.Next(startIndex, endIndex);
                SwapElements(list, i, k);
            }
        }

        public static void SwapElements<T>(this IList<T> list, int i, int j)
        {
            var temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }

        public static int CarryToEndPredicateMatch<T>(this IList<T> list, System.Predicate<T> predicate)
        {
            if (list.Count == 0)
                return 0;

            int lastIndex = list.Count - 1;
            int itemCount = list.Count;

            for (int i = 0; i < itemCount; i++)
            {
                if (i == lastIndex)
                    break;

                if (predicate.Invoke(list[i]))
                {
                    SwapElements(list, i, lastIndex);
                    lastIndex--;
                }
            }

            return lastIndex;
        }

        public static T RandomItemFromList<T>(this IList<T> list)
        {
            if (list.Count == 0)
            {
                throw new System.IndexOutOfRangeException("Cannot select a random item from an empty list");
            }

            var rnd = new System.Random(UnityEngine.Random.Range(0, 1000));
            var index = rnd.Next(0, list.Count);
            return list[index];
        }

        public static void RemoveLast<T>(this IList<T> list)
        {
            list.RemoveAt(list.Count - 1);
        }

        public static T GetLast<T>(this IList<T> list)
        {
            return list[list.Count - 1];
        }

        public static T SetLast<T>(this IList<T> list, T value)
        {
            return list[list.Count - 1] = value;
        }

        public static T GetFirst<T>(this IList<T> list)
        {
            return list[0];
        }

        public static T SetFirst<T>(this IList<T> list, T value)
        {
            return list[0] = value;
        }

        public static string ToPrettyString<T>(this IEnumerable<T> source) => $"[{string.Join(", ", source)}]";

        public static T Random<T>(this IList<T> list)
        {
            int randomIndex = UnityEngine.Random.Range(0, list.Count);
            return list[randomIndex];
        }

        public static T RandomElement<T>(this IReadOnlyList<T> list)
        {
            int randomIndex = UnityEngine.Random.Range(0, list.Count);
            return list[randomIndex];
        }

        public static bool IsEmpty<T>(this IReadOnlyList<T> source)
        {
            return !source.Any();
        }

        public static IReadOnlyList<T> UseIfEmpty<T>(this IReadOnlyList<T> source, IReadOnlyList<T> ifEmpty)
        {
            return source.IsEmpty() ? ifEmpty : source;
        }

        public static T AddWithReturn<T>(this List<T> list, T item)
        {
            list.Add(item);
            return item;
        }

        public static int IndexOf<T>(this List<T> source, Func<T, bool> predicate)
        {
            for (var i = 0; i < source.Count; i++)
            {
                var element = source[i];
                if (predicate(element))
                {
                    return i;
                }
            }

            return -1;
        }

        public static int IndexOf<T, TParam>(this List<T> source, Func<T, TParam, bool> predicate, TParam param)
        {
            for (var i = 0; i < source.Count; i++)
            {
                var element = source[i];
                if (predicate(element, param))
                {
                    return i;
                }
            }

            return -1;
        }

        public static T LastElement<T>(this List<T> sourceList)
        {
            if (sourceList == null)
                return default;

            if (sourceList.Count == 0)
                return default;

            return sourceList[sourceList.Count - 1];
        }

        public static bool IsHasAnyElement<T>(this List<T> sourceList)
        {
            if (sourceList == null || sourceList.Count == 0)
                return false;

            return true;
        }

        public static void RemoveNulls<T>(this List<T> sourceList)
        {
            try
            {
                int currentIndex = 0;
                int maxIndex = sourceList.Count;

                for (currentIndex = 0; currentIndex < maxIndex;)
                {

                    if (sourceList[currentIndex] is null || sourceList[currentIndex].Equals(null))
                    {
                        sourceList.RemoveAt(currentIndex);
                        maxIndex--;
                        continue;
                    }

                    currentIndex++;
                }
            }
            catch (System.Exception e)
            {
                UnityEngine.Debug.LogError($"Error while trying to remove nulls from a List! This operation was not safe, list may become corrupted! Error : {e}");
            }
        }

        public static void RemoveNullsSafe<T>(this List<T> sourceList)
        {
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                MemoryStream stream = new MemoryStream();
                formatter.Serialize(stream, sourceList);
                stream.Position = 0;

                List<T> copyList = (List<T>)formatter.Deserialize(stream);

                int currentIndex = 0;
                int maxIndex = sourceList.Count;

                for (currentIndex = 0; currentIndex < maxIndex;)
                {
                    if (copyList[currentIndex] is null)
                    {
                        copyList.RemoveAt(currentIndex);
                        maxIndex--;
                        continue;
                    }

                    currentIndex++;
                }

                sourceList = copyList;
            }
            catch (System.Exception e)
            {
                UnityEngine.Debug.LogError($"Error while trying to remove nulls from a List! Reverting changes on list!  Error : {e}");
            }
        }

        public static T Get<T>(this List<T> list, UnityEngine.Vector2Int index, int xMax)
        {
            return list[index.y * xMax + index.x];
        }

        public static bool TryGet<T>(this List<T> list, UnityEngine.Vector2Int index, int xMax, out T value)
        {
            value = default;

            int arrayIndex = index.y * xMax + index.x;

            if (arrayIndex < 0 || arrayIndex >= list.Count)
                return false;

            value = list[arrayIndex];
            return true;
        }

        public static void Set<T>(this List<T> list, UnityEngine.Vector2Int index, int xMax, T value)
        {
            list[index.y * xMax + index.x] = value;
        }
    }

}
