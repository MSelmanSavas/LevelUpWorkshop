namespace Codeflow
{
    public interface ICodeflowNode
    {
        public CodeflowState Initialize();
        public CodeflowState Reset();
        public CodeflowState OneTimeExecute();
    }
}
