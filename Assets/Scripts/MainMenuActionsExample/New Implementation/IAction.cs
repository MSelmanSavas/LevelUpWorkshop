
namespace LevelUp.UZCY.CodeAbstraction.Examples.MainMenu.New
{
    public interface IAction
    {
        bool Initialize();
        bool CanStart();
        bool OneTimeExecute();
        bool Update();
        bool IsFinished();
    }
}