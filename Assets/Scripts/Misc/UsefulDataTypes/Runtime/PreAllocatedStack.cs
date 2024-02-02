using System;
using UnityEngine;

namespace UsefulDataTypes
{
    [System.Serializable]
    public struct PreAllocatedStack<T>
    {
        T DEFAULT_VALUE;

        T LastPoppedValue;

        public int Count;

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

        public PreAllocatedStack(T DEFAULT_VALUE = default)
        {
            Count = 0;
            ValueArray = new T[0];
            this.DEFAULT_VALUE = DEFAULT_VALUE;
            this.LastPoppedValue = this.DEFAULT_VALUE;

            for (int i = 0; i < ValueArray.Length; i++)
            {
                ValueArray[i] = DEFAULT_VALUE;
            }

            OnCapacityChange = null;
        }

        public PreAllocatedStack(Vector2Int Capacity, T DEFAULT_VALUE = default)
        {
            Count = 0;
            ValueArray = new T[Capacity.x * Capacity.y];
            this.DEFAULT_VALUE = DEFAULT_VALUE;
            this.LastPoppedValue = this.DEFAULT_VALUE;

            for (int i = 0; i < ValueArray.Length; i++)
            {
                ValueArray[i] = DEFAULT_VALUE;
            }

            OnCapacityChange = null;
        }

        public PreAllocatedStack(int Capacity, T DEFAULT_VALUE = default)
        {
            Count = 0;
            ValueArray = new T[Capacity];
            this.DEFAULT_VALUE = DEFAULT_VALUE;
            this.LastPoppedValue = this.DEFAULT_VALUE;

            for (int i = 0; i < ValueArray.Length; i++)
            {
                ValueArray[i] = DEFAULT_VALUE;
            }

            OnCapacityChange = null;
        }

        public PreAllocatedStack(PreAllocatedStack<T> CopyFromArray, int newCapacity, T DEFAULT_VALUE = default)
        {
            Count = 0;
            ValueArray = new T[newCapacity];
            this.DEFAULT_VALUE = DEFAULT_VALUE;
            this.LastPoppedValue = this.DEFAULT_VALUE;

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

            OnCapacityChange = null;
        }

        public T Pop()
        {
            if (Count <= 0)
                throw new System.Exception($"No values stored in {this}! Cannot Pop.");

            LastPoppedValue = ValueArray[--Count];
            ValueArray[Count] = DEFAULT_VALUE;

            return LastPoppedValue;
        }

        public void Push(T value)
        {
            if (Count >= Capacity)
            {
                int oldCapacity = Capacity;
                Capacity = Capacity * 2;
                int newCapacity = Capacity;
                OnCapacityChange?.Invoke(oldCapacity, newCapacity);
            }

            ValueArray[Count++] = value;
        }

        public void Clear()
        {
            Count = 0;
        }

        public void ClearAndSetDefault()
        {
            Count = 0;

            for (int i = 0; i < ValueArray.Length; i++)
            {
                ValueArray[i] = DEFAULT_VALUE;
            }
        }
    }

}
