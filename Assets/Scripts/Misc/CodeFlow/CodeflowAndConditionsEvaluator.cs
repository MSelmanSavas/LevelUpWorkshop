using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Codeflow.Conditions
{
    public class CodeflowAndConditionsEvaluator : ICodeflowConditionsEvaluator
    {
        public bool IsSucceded { get; private set; }
        [SerializeField] CodeflowState _validationValue;

        public CodeflowState CheckConditions(ICollection<ICodeflowCondition> conditions)
        {
            foreach (var condition in conditions)
            {
                if (condition.CheckCondition() != _validationValue)
                    return CodeflowState.Waiting;
            }

            IsSucceded = true;
            return CodeflowState.Success;
        }

        public CodeflowState Reset()
        {
            IsSucceded = false;
            return CodeflowState.Success;
        }
    }
}
