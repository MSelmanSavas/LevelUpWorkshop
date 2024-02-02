
namespace Codeflow.Conditions
{
    public interface ICodeflowCondition : ICodeflowNode
    {
        public CodeflowState CheckCondition();
    }
}
