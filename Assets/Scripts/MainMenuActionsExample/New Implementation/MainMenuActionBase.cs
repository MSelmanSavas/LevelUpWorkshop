namespace LevelUp.UZCY.CodeAbstraction.Examples.MainMenu.New
{
    public abstract class MainMenuActionBase : IAction
    {
        public virtual bool CanStart() => true;
        public virtual bool Initialize() => true;
        public virtual bool IsFinished() => true;
        public virtual bool OneTimeExecute() => true;
        public virtual bool Update() => true;
    }
}