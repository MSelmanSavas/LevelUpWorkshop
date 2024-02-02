using UnityEngine;

namespace Codeflow.Execution
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "ScriptableCodeflowExecutor", menuName = "Scriptable Datas/Codeflow/Scriptable Codeflow Executor", order = 0)]
    public class ScriptableCodeflowExecutor : ScriptableObject
    {
        [field: SerializeReference]
        public ICodeflowExecutor Executor { get; private set; }
    }
}