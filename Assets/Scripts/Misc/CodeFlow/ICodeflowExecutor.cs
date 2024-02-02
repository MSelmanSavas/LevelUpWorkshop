namespace Codeflow.Execution
{
    public interface ICodeflowExecutor
    {
        public CodeflowState Initialize();
        public CodeflowState TryStart();
        public CodeflowState TryProcess();
        public CodeflowState Reset();
    }
}