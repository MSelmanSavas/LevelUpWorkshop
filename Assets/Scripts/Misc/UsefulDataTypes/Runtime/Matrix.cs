using System.Collections.Generic;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.Utilities;
#endif

namespace UsefulDataTypes
{
    [System.Serializable]
    public class Matrix<T> : ISerializationCallbackReceiver
    {
        public T NULL_VALUE;

#if ODIN_INSPECTOR && UNITY_EDITOR
        [Sirenix.OdinInspector.TableMatrix(DrawElementMethod = "DrawCustomMatrix", ResizableColumns = false)]
        [Sirenix.OdinInspector.ShowInInspector]
#endif
        public T[,] Values;

        [SerializeField]
        [HideInInspector]
        protected Vector2Int _matrixSize = Vector2Int.zero;

        [SerializeField]
        [HideInInspector]
        protected Vector2Int _assignedValuesSize = Vector2Int.zero;

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ShowInInspector]
#endif

        public Vector2Int MatrixSize
        {
            get
            {
                return _matrixSize;
            }
            set
            {
                if (value.x < 0 || value.y < 0)
                    return;


                T[,] newArray = new T[value.x, value.y];

                Vector2Int iterationSize = Vector2Int.Min(value, _matrixSize);

                for (int i = 0; i < iterationSize.x; i++)
                    for (int j = 0; j < iterationSize.y; j++)
                    {
                        newArray[i, j] = Values[i, j];
                    }

                _matrixSize = value;
                Values = newArray;
            }
        }

        public Vector2Int AssignedValuesSize
        {
            get
            {
                return _assignedValuesSize;
            }
            set
            {
                if (value.x < 0 || value.y < 0)
                    return;

                _assignedValuesSize = value;
            }
        }

        public T this[int x, int y]
        {
            get
            {
                return Values[x, y];
            }
            set
            {
                Values[x, y] = value;
            }
        }

        public T this[Vector2Int Index]
        {
            get
            {
                return Values[Index.x, Index.y];
            }
            set
            {
                Values[Index.x, Index.y] = value;
            }
        }

        public Matrix(int xSize, int ySize, T defaultValue = default)
        {
            NULL_VALUE = defaultValue;

            MatrixSize = new Vector2Int(xSize, ySize);
            AssignedValuesSize = MatrixSize;

            Values = new T[MatrixSize.x, MatrixSize.y];

            for (int i = 0; i < MatrixSize.x; i++)
            {
                for (int j = 0; j < MatrixSize.y; j++)
                {
                    Values[i, j] = NULL_VALUE;
                }
            }
        }

        public Matrix(Vector2Int size, T defaultValue = default)
        {
            NULL_VALUE = defaultValue;

            MatrixSize = size;
            AssignedValuesSize = MatrixSize;

            Values = new T[MatrixSize.x, MatrixSize.y];

            for (int i = 0; i < MatrixSize.x; i++)
            {
                for (int j = 0; j < MatrixSize.x; j++)
                {
                    Values[i, j] = NULL_VALUE;
                }
            }
        }

        public Matrix(T[,] Array)
        {
            MatrixSize = new Vector2Int(Array.GetLength(0), Array.GetLength(1));
            AssignedValuesSize = MatrixSize;

            Values = new T[MatrixSize.x, MatrixSize.y];

            for (int i = 0; i < MatrixSize.x; i++)
            {
                for (int j = 0; j < MatrixSize.y; j++)
                {
                    Values[i, j] = Array[i, j];
                }
            }
        }

        public void Reset()
        {
            MatrixSize = new Vector2Int(Values.GetLength(0), Values.GetLength(1));
            AssignedValuesSize = MatrixSize;

            for (int i = 0; i < MatrixSize.x; i++)
            {
                for (int j = 0; j < MatrixSize.y; j++)
                {
                    Values[i, j] = NULL_VALUE;
                }
            }
        }

        public void ResetOnlyValues(Vector2Int IndexesToReset)
        {
            Vector2Int indexes = new Vector2Int(Mathf.Min(IndexesToReset.x, MatrixSize.x), Mathf.Min(IndexesToReset.y, MatrixSize.y));

            for (int i = 0; i < indexes.x; i++)
            {
                for (int j = 0; j < indexes.y; j++)
                {
                    Values[i, j] = NULL_VALUE;
                }
            }
        }

        /// <summary>
        /// Check matrix index for given NULL_VALUE equality. DO NOT USE FOR OBJECT BASED MATRIX!
        /// </summary>
        /// <param name="xIndex"></param>
        /// <param name="yIndex"></param>
        /// <returns></returns>
        public bool IsIndexNullValue(int xIndex, int yIndex)
        {
            if (xIndex >= _matrixSize.x || xIndex < 0 || yIndex >= _matrixSize.y || yIndex < 0)
                return true;

            if (Values[xIndex, yIndex].Equals(NULL_VALUE))
                return true;

            return false;
        }

        public bool IsIndexNullValue(Vector2Int Index)
        {
            return IsIndexNullValue(Index.x, Index.y);
        }

        [SerializeField]
        [HideInInspector]
        List<T> SerializedValues = new List<T>();

        public void OnBeforeSerialize()
        {
            this.SerializedValues = new List<T>();

            for (int i = 0; i < MatrixSize.x; i++)
                for (int j = 0; j < MatrixSize.y; j++)
                {
                    SerializedValues.Add(Values[i, j]);
                }
        }

        public void OnAfterDeserialize()
        {
            Values = new T[MatrixSize.x, MatrixSize.y];

            for (int i = 0; i < MatrixSize.x; i++)
            {
                for (int j = 0; j < MatrixSize.y; j++)
                {
                    Values[i, j] = this.SerializedValues[i * MatrixSize.y + j];
                }
            }
        }

#if ODIN_INSPECTOR && UNITY_EDITOR
        public static T DrawCustomMatrix(Rect rect, T value)
        {
            if (value == null)
            {
                UnityEditor.EditorGUI.LabelField(rect, "null");
                return default;
            }

            switch (typeof(T))
            {
                case System.Type boolType when boolType == typeof(bool):
                    {
                        rect = rect.MaxWidth(40);

                        bool boolValue = (bool)System.Convert.ChangeType(value, typeof(bool));

                        if (Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition))
                        {
                            boolValue = !boolValue;
                            GUI.changed = true;
                            Event.current.Use();
                        }

                        UnityEditor.EditorGUI.DrawRect(rect.Padding(1), boolValue ? new Color(0.1f, 0.8f, 0.2f) : new Color(0, 0, 0, 0.5f));
                        value = (T)System.Convert.ChangeType(boolValue, typeof(T));

                        break;
                    }
                default:
                    {
                        UnityEditor.EditorGUI.LabelField(rect, value.ToString());
                        break;
                    }
            }

            return value;
        }
#endif
    }

}

