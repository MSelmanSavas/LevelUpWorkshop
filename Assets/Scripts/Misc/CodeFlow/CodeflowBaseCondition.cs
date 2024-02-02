namespace Codeflow.Conditions
{
    public abstract class CodeflowBaseCondition : ICodeflowCondition
    {
        public virtual CodeflowState Initialize() { return CodeflowState.Success; }
        public virtual CodeflowState Reset() { return CodeflowState.Success; }
        public virtual CodeflowState CheckCondition() { return CodeflowState.Success; }
        public virtual CodeflowState OneTimeExecute() { return CodeflowState.Success; }
    }
}
