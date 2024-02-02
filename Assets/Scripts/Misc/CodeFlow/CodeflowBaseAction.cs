namespace Codeflow.Actions
{
    public abstract class CodeflowBaseAction : ICodeflowAction
    {
        public virtual CodeflowState Initialize() { return CodeflowState.Success; }
        public virtual CodeflowState Reset() { return CodeflowState.Success; }
        public virtual CodeflowState Execute() { return CodeflowState.Success; }
        public virtual CodeflowState OneTimeExecute() { return CodeflowState.Success; }
    }
}
