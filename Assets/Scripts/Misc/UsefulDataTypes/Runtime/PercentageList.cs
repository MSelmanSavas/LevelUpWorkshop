using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class PercentageList : IReadOnlyList<float>
{
    private List<float> _values;

    public float Min { get; }
    public float Max { get; } = 1;

    public PercentageList(int count, float min = 0, float max = 1)
    {
        _values = Enumerable.Repeat(1f / count, count).ToList();
        Min = min;
        Max = max;
    }

    private int ClampIndex(int index)
    {
        int clampedIndex = Mathf.Clamp(index, 0, _values.Count - 1);
        // if (clampedIndex != index)
        // {
        //     Debug.LogError($"Index Out Of Range: {index}, size: {_values.Count}");
        // }

        return clampedIndex;
    }

    public IEnumerator<float> GetEnumerator()
    {
        return _values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable) _values).GetEnumerator();
    }

    public int Count => _values.Count;

    public float this[int index]
    {
        get
        {
            int clampedIndex = ClampIndex(index);
            return _values[clampedIndex];
        }
        set
        {
            if (Count < 2) return;

            int clampedIndex = ClampIndex(index);

            float clampedValue = Mathf.Clamp(value, Min, Max);
            float difference = clampedValue - _values[clampedIndex];
            float differencePerValue = difference / (Count - 1);

            for (var i = 0; i < Count; i++)
            {
                _values[i] += i == clampedIndex ? difference : -differencePerValue;
            }
        }
    }
}
