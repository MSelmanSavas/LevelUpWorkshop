
using System.Collections.Generic;

namespace Codeflow.Conditions
{
    public interface ICodeflowConditionsEvaluator
    {
        public CodeflowState Reset();
        public CodeflowState CheckConditions(ICollection<ICodeflowCondition> conditions);
    }
}