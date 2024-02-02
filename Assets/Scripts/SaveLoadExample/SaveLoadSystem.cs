using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace LevelUp.UZCY.CodeAbstraction.Examples.SaveLoad
{
    public class SaveLoadSystem
    {
        const string SAVEFILENAME = "/Save.txt";
        [Sirenix.OdinInspector.ShowInInspector] public Dictionary<System.Type, SaveData> SaveDatas = new Dictionary<System.Type, SaveData>();
        bool isSaveSystemInitialized = false;
        public Action OnBeforeSaveLoadShutdown;
        public Action OnAfterSaveLoadShutdown;

        [SerializeField] bool _shouldAutoSave = true;

        protected void Initialize()
        {
            SaveDatas = new Dictionary<System.Type, SaveData>();

            if (!isSaveSystemInitialized)
                TryLoadSaveFile();
        }

        void TryLoadSaveFile()
        {
            isSaveSystemInitialized = LoadFromDisk();
        }

        public bool TryFindSaveData<T>(out SaveData saveData, T data)
        {
            if (!isSaveSystemInitialized)
                TryLoadSaveFile();

            if (!typeof(ISaveable).IsAssignableFrom(typeof(T)))
            {
                saveData = default;
                return false;
            }

            if (!SaveDatas.ContainsKey(typeof(T)))
            {
                ISaveable dataSaveable = data as ISaveable;
                SaveData newSaveData = dataSaveable.SaveData();
                SaveDatas.Add(typeof(T), newSaveData);
            }

            saveData = SaveDatas[typeof(T)];
            return true;
        }

        public bool TryFindSaveData(System.Type saveDataType, out SaveData saveData, object data)
        {
            if (!isSaveSystemInitialized)
                TryLoadSaveFile();

            if (!typeof(ISaveable).IsAssignableFrom(saveDataType))
            {
                saveData = default;
                return false;
            }

            if (!SaveDatas.ContainsKey(saveDataType))
            {
                ISaveable dataSaveable = data as ISaveable;
                SaveData newSaveData = dataSaveable.SaveData();
                SaveDatas.Add(saveDataType, newSaveData);
            }

            saveData = SaveDatas[saveDataType];
            return true;
        }

        public bool CheckSaveDataExistance<T>()
        {
            if (!typeof(ISaveable).IsAssignableFrom(typeof(T)))
            {
                return false;
            }

            if (!SaveDatas.ContainsKey(typeof(T)))
            {
                return false;
            }

            return true;
        }

        public bool TrySaveData(ISaveable saveable)
        {
            SaveData saveData = saveable.SaveData();

            try
            {
                if (!SaveDatas.ContainsKey(saveData.Type))
                {
                    SaveDatas.Add(saveData.Type, saveData);
                }

                SaveDatas[saveData.Type] = saveData;
                SaveDatasToList();

                if (_shouldAutoSave)
                    SaveToDisk();
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error while trying to save : {saveData.Type}! Cannot save... Error : {e}");
            }

            return true;
        }

        SaveDataForFile ConvertSaveDataToFileFormat(SaveData data)
        {
            SaveDataForFile fileData = new SaveDataForFile();
            fileData.ID = data?.ID;
            fileData.SaveDataType = data?.Type?.AssemblyQualifiedName;
            fileData.Data = data?.Data;
            return fileData;
        }

        SaveData ConvertSaveDataForFileToSaveDataFormat(SaveDataForFile fileData)
        {
            SaveData data = new SaveData();
            data.ID = fileData.ID;
            data.Type = System.Type.GetType(fileData.SaveDataType);
            data.Data = fileData.Data;
            return data;
        }

        /// <summary>
        /// Loads save data from disk.
        /// </summary>
        /// <returns></returns>
        [Sirenix.OdinInspector.Button]
        public bool LoadFromDisk()
        {
            try
            {
                string saveFilePath = Application.persistentDataPath + SAVEFILENAME;

                if (!System.IO.File.Exists(saveFilePath))
                    throw new FileNotFoundException("No save file found!");

                using (System.IO.FileStream fileStream = new System.IO.FileStream(saveFilePath, System.IO.FileMode.OpenOrCreate))
                using (System.IO.StreamReader sr = new System.IO.StreamReader(fileStream))
                {
                    try
                    {
                        string loadedJsonFile = sr.ReadToEnd();
                        SaveFileData loadedData = JsonUtility.FromJson<SaveFileData>(loadedJsonFile);
                        LoadFromList(loadedData.SaveDatasForFiles);
                    }
                    catch
                    {
                        sr.Dispose();
                        SaveToDisk();
                        throw new System.Exception("Cannot find save file or cannot read with JSON");
                    }
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error while trying to load datas from disk! Creating new save file! Error : {e}");
                SaveToDisk();
            }

            return true;
        }

        /// <summary>
        /// Loads save data from disk.
        /// </summary>
        /// <returns></returns>
        [Sirenix.OdinInspector.Button]
        public bool SaveToDisk()
        {
            string saveFilePath = Application.persistentDataPath + SAVEFILENAME;

            if (!System.IO.File.Exists(saveFilePath))
                System.IO.File.Create(saveFilePath);


            SaveFileData fileData = new SaveFileData
            {
                SaveDatasForFiles = SaveDatasToList()
            };

            string saveData = UnityEngine.JsonUtility.ToJson(fileData, true);

            using (System.IO.FileStream fileStream = new System.IO.FileStream(saveFilePath, System.IO.FileMode.Create))
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(fileStream))
            {
                sw.Write(saveData);
            }

            return true;
        }

        /// <summary>
        /// Loads SaveDatasForFiles list to SaveDatas dictionary for faster and precise access.
        /// </summary>
        [Sirenix.OdinInspector.Button]
        public bool LoadFromList(List<SaveDataForFile> dataList)
        {

            SaveDatas.Clear();
            SaveData currentSaveData;

            for (int i = 0; i < dataList.Count; i++)
            {
                try
                {
                    currentSaveData = ConvertSaveDataForFileToSaveDataFormat(dataList[i]);
                    SaveDatas.Add(currentSaveData.Type, currentSaveData);
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"There was an error while loading save datas from SaveDatasForFile to SaveDatas dictionary! SaveDataForFile : {dataList[i]} Error : {e}");
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Saves SaveDatas dictionary to SaveDatasForFiles list for saving all user data.
        /// </summary>
        [Sirenix.OdinInspector.Button]
        public List<SaveDataForFile> SaveDatasToList()
        {
            List<SaveDataForFile> dataList = new List<SaveDataForFile>();


            foreach (KeyValuePair<Type, SaveData> saveData in SaveDatas)
            {
                try
                {
                    dataList.Add(ConvertSaveDataToFileFormat(saveData.Value));
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"There was an error while saving SaveDatas to SaveDatasForFile list! SaveData : {saveData.Value} Error : {e}");
                    return null;
                }
            }

            return dataList;
        }

        [System.Serializable]
        public class SaveFileData
        {
            public List<SaveDataForFile> SaveDatasForFiles = new List<SaveDataForFile>();
        }

        public string GetSaveDataAsJsonString()
        {
            SaveFileData fileData = new SaveFileData
            {
                SaveDatasForFiles = SaveDatasToList()
            };

            return UnityEngine.JsonUtility.ToJson(fileData, false);
        }

        public void SaveJsonDataToDisk(string loadedJsonFile)
        {
            try
            {
                SaveFileData loadedData = JsonUtility.FromJson<SaveFileData>(loadedJsonFile);
                LoadFromList(loadedData.SaveDatasForFiles);
                SaveToDisk();
            }
            catch (Exception e)
            {
                Debug.LogError("Error: Data could not be saved in to disk! " + e.ToString());
            }
        }
    }
}


