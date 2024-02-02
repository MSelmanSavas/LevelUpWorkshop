namespace LevelUp.UZCY.CodeAbstraction.Examples.SaveLoad
{
    [System.Serializable]
    public class SaveData
    {
        public SaveDataIdentifier ID = new SaveDataIdentifier();
        public System.Type Type;
        public string Data;
    }
}