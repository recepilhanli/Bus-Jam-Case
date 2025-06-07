using System.Collections;
using System.Collections.Generic;
using Game.Level;
using Game.Utils;
using UnityEditor;
using UnityEngine;


namespace Game.Data
{
    public class SaveManager //TO DO: Convert this to binary save system
    {

        private const string FILENAME_CURRENTLEVEL = "currentLevel";

        private static string path = Application.persistentDataPath + "/save{0}.json";


        public static void Save<T>(T data, string fileName) where T : class
        {
            string filePath = string.Format(path, fileName);
            string json = JsonUtility.ToJson(data, true);

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            System.IO.File.WriteAllTextAsync(filePath, json);
        }

        public static T Load<T>(string fileName) where T : class
        {
            string filePath = string.Format(path, fileName);

            if (!System.IO.File.Exists(filePath))
            {
                Debug.LogWarning($"Save file not found: {filePath}");
                return null;
            }

            string json = System.IO.File.ReadAllText(filePath);
            return JsonUtility.FromJson<T>(json);
        }


        public static void DeleteSaveFile(string fileName)
        {
            string filePath = string.Format(path, fileName);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }

        public static void SaveCurrentGame()
        {
            if (GameManager.instance != null)
            {
                LevelSaveData saveData = LevelSaveData.GetCurrentLevel();
                Save(saveData, FILENAME_CURRENTLEVEL);
            }
            else DeleteSaveFile(FILENAME_CURRENTLEVEL);
        }


        public LevelSaveData LoadCurrentGame()
        {
            LevelSaveData saveData = Load<LevelSaveData>(FILENAME_CURRENTLEVEL);
            return saveData;
        }

        public static void DeleteCurrentGame()
        {
            DeleteSaveFile(FILENAME_CURRENTLEVEL);
        }



    }

}