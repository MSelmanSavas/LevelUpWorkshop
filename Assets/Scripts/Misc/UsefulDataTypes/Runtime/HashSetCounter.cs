namespace UsefulDataTypes
{
    [System.Serializable]
    public class HashSetCounter<T>
    {
        [System.Serializable]
        public class CounterDictionary : SerializableDictionary<T, int>
        {
            public CounterDictionary() : base() { }
            public CounterDictionary(int capacity) : base(capacity) { }
        }

        public CounterDictionary Counter = new();

        public HashSetCounter()
        {
            Counter = new();
        }

        public HashSetCounter(int capacity)
        {
            Counter = new(capacity);
        }

        public bool Add(T item)
        {
            if (!Counter.ContainsKey(item))
                Counter.Add(item, 0);

            Counter[item]++;

            return true;
        }

        public bool Add(T item, int amount)
        {
            if (!Counter.ContainsKey(item))
                Counter.Add(item, 0);

            Counter[item] += amount;

            return true;
        }

        public bool Update(T item, int amount)
        {
            if (!Counter.ContainsKey(item))
                return false;

            Counter[item] = amount;

            return true;
        }

        public bool Remove(T item)
        {
            if (!Counter.ContainsKey(item))
                return false;

            Counter[item]--;

            if (Counter[item] == 0)
                Counter.Remove(item);

            return true;
        }

        public bool Remove(T item, int amount)
        {
            if (!Counter.ContainsKey(item))
                return false;

            Counter[item] -= amount;

            if (Counter[item] <= 0)
                Counter.Remove(item);

            return true;
        }

        public bool TryGetValue(T item, out int value)
        {
            value = 0;
            if (!Contains(item))
                return false;

            value = Counter[item];
            return true;
        }

        public bool Contains(T item) => Counter.ContainsKey(item);
        public void Clear() => Counter.Clear();
    }
}

