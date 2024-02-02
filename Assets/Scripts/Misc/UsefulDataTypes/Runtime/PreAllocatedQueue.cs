using System;
using UnityEngine;

namespace UsefulDataTypes
{
    public class PreAllocatedQueue<T>
    {
        T DEFAULT_VALUE;

        public int StartIndex;
        public int EndIndex;

        public int Count => Mathf.Abs(EndIndex - StartIndex);

        public Action<int, int> OnCapacityChange;

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ShowInInspector]
        [Sirenix.OdinInspector.ReadOnly]
#endif
        public int Capacity
        {
            get => ValueArray.Length;
            set
            {
                Array.Resize(ref ValueArray, value);
            }
        }

        public T[] ValueArray;

        public T this[int index]
        {
            get => ValueArray[index];
            set => ValueArray[index] = value;
        }

        public PreAllocatedQueue(T DEFAULT_VALUE = default)
        {
            StartIndex = 0;
            EndIndex = 0;
            ValueArray = new T[0];
            this.DEFAULT_VALUE = DEFAULT_VALUE;

            for (int i = 0; i < ValueArray.Length; i++)
            {
                ValueArray[i] = DEFAULT_VALUE;
            }
        }

        public PreAllocatedQueue(Vector2Int Capacity, T DEFAULT_VALUE = default)
        {
            StartIndex = 0;
            EndIndex = 0;
            ValueArray = new T[Capacity.x * Capacity.y];
            this.DEFAULT_VALUE = DEFAULT_VALUE;

            for (int i = 0; i < ValueArray.Length; i++)
            {
                ValueArray[i] = DEFAULT_VALUE;
            }
        }

        public PreAllocatedQueue(int Capacity, T DEFAULT_VALUE = default)
        {
            StartIndex = 0;
            EndIndex = 0;
            ValueArray = new T[Capacity];
            this.DEFAULT_VALUE = DEFAULT_VALUE;

            for (int i = 0; i < ValueArray.Length; i++)
            {
                ValueArray[i] = DEFAULT_VALUE;
            }
        }

        public PreAllocatedQueue(PreAllocatedQueue<T> CopyFromArray, int newCapacity, T DEFAULT_VALUE = default)
        {
            StartIndex = CopyFromArray.StartIndex;
            EndIndex = CopyFromArray.EndIndex;

            ValueArray = new T[newCapacity];
            this.DEFAULT_VALUE = DEFAULT_VALUE;

            for (int i = 0; i < ValueArray.Length; i++)
            {
                if (CopyFromArray.Count > i)
                {
                    ValueArray[i] = CopyFromArray[i];
                }
                else
                {
                    ValueArray[i] = DEFAULT_VALUE;
                }
            }
        }

        public T Dequeue()
        {
            if (StartIndex + 1 % ValueArray.Length == EndIndex)
                throw new System.Exception($"No values stored in {this}! Cannot Pop.");

            return ValueArray[StartIndex++];
        }

        public void Enqueue(T value)
        {
            if (EndIndex + 1 % ValueArray.Length == StartIndex)
            {
                int oldCapacity = Capacity;
                Capacity = Capacity * 2;
                int newCapacity = Capacity;

                OnCapacityChange?.Invoke(oldCapacity, newCapacity);
            }

            ValueArray[EndIndex++] = value;
        }

        public void Clear()
        {
            StartIndex = 0;
            EndIndex = 0;
        }

        public void ClearAndSetDefault()
        {
            StartIndex = 0;
            EndIndex = 0;

            for (int i = 0; i < ValueArray.Length; i++)
            {
                ValueArray[i] = DEFAULT_VALUE;
            }
        }
    }
}
