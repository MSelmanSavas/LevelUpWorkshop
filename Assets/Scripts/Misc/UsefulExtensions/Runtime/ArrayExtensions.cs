using System.Text;
using UnityEngine;

namespace UsefulExtensions.Array
{
    public static class ArrayExtensions
    {

        public static T RandomItemFromArray<T>(this T[] array)
        {
            if (array.Length == 0)
            {
                throw new System.IndexOutOfRangeException("Cannot select a random item from an empty array");
            }

            var rnd = new System.Random(UnityEngine.Random.Range(0, 1000));
            var index = rnd.Next(0, array.Length);
            return array[index];
        }

        public static T Get<T>(this T[,] array, UnityEngine.Vector2Int index)
        {
            return array[index.x, index.y];
        }

        public static T Last<T>(this T[] array) => array[array.Length - 1];

        public static T Get<T>(this T[] array, UnityEngine.Vector2Int index, int xMax)
        {
            return array[index.y * xMax + index.x];
        }

        public static bool TryGet<T>(this T[] array, UnityEngine.Vector2Int index, int xMax, out T value)
        {
            value = default;

            int arrayIndex = index.y * xMax + index.x;

            if (arrayIndex < 0 || arrayIndex >= array.Length)
                return false;

            value = array[arrayIndex];
            return true;
        }

        public static void Set<T>(this T[] array, UnityEngine.Vector2Int index, int xMax, T value)
        {
            array[index.y * xMax + index.x] = value;
        }

        public static void Set<T>(this T[,] array, UnityEngine.Vector2Int index, T value)
        {
            array[index.x, index.y] = value;
        }

        public static void Set<T>(this (T, T)[,] array, UnityEngine.Vector2Int index, T value1, T value2)
        {
            array[index.x, index.y] = (value1, value2);
        }

        public static bool IsInRange<T>(this T[,] array, UnityEngine.Vector2Int index)
        {
            return !(index.x < 0 || index.y < 0 || index.x >= array.GetLength(0) || index.y >= array.GetLength(1));
        }

        public static string DebugArray<T>(this T[] array)
        {
            StringBuilder builder = new StringBuilder(array.Length * 10);

            foreach (var obj in array)
            {
                builder.AppendLine(obj.ToString());
            }

            return builder.ToString();
        }
    }
}
