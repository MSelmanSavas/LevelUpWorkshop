namespace UsefulExtensions.Int
{
    public static class IntExtensions
    {
        public static int IncrementUpTo(this int n, int max) => n == max ? max : n + 1;

        public static void IncreaseUpTo(this ref int n, int increase, int max)
        {
            n = UnityEngine.Mathf.Clamp(n + increase, n, max);
        }

        public static bool TryIncreaseUpTo(this ref int n, int increase, int max)
        {
            if (n == max)
                return false;

            n = UnityEngine.Mathf.Clamp(n + increase, n, max);
            return true;
        }

        /// <summary>
        /// Finds the nearest multiple of m that is greater than or equal to n.
        /// </summary>
        /// <param name="n">number</param>
        /// <param name="m">multiple</param>
        /// <returns></returns>
        public static int ToNextMultipleOf(this int n, int m)
        {
            var isMultiple = n % m == 0;
            if (isMultiple)
                return n;

            var nextMultiple = (n / m + 1) * m;
            return nextMultiple;
        }
    }

}
