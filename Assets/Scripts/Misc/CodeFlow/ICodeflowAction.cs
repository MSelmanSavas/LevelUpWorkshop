namespace Codeflow.Actions
{
    public interface ICodeflowAction : ICodeflowNode
    {
        public CodeflowState Execute();
    }
}
