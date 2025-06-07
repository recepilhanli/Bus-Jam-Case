using System.Collections;
using System.Collections.Generic;
using Game.Level;
using Game.Utils;
using UnityEngine;


namespace Game.Data
{
    public class SaveManager //TO DO: Convert this to binary save system
    {

        private const string FILENAME_CURRENTLEVEL = "currentLevel";
        private const string FILENAME_STATS = "playerStats";

        private static string path = Application.persistentDataPath + "/save{0}.json";
        public static LevelSaveData currentLevelData { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            LoadCurrentGame();
            _ = LevelLoader.InitAsync();
            Debug.Log("SaveManager initialized. Current level data: " + (currentLevelData != null));
        }


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

#if UNITY_EDITOR
        [UnityEditor.MenuItem("Debug/Save/Save Current Game")]
#endif
        public static void SaveCurrentGame()
        {

#if UNITY_EDITOR
            if (Application.isPlaying == false)
            {
                Debug.LogWarning("Cannot save current game. Application is not playing.");
                return;
            }
#endif

            if (GameManager.instance != null)
            {
                LevelSaveData saveData = LevelSaveData.GetCurrentLevel();
                Save(saveData, FILENAME_CURRENTLEVEL);
            }
            else DeleteSaveFile(FILENAME_CURRENTLEVEL);

            PlayerStateData playerStateData = PlayerStateData.GetCurrentData();
            Save(playerStateData, FILENAME_STATS);
        }

#if UNITY_EDITOR
        [UnityEditor.MenuItem("Debug/Save/Load Current Game")]
#endif
        public static void LoadCurrentGame()
        {

#if UNITY_EDITOR
            if (Application.isPlaying == false)
            {
                Debug.LogWarning("Cannot save current game. Application is not playing.");
                return;
            }
#endif

            currentLevelData = Load<LevelSaveData>(FILENAME_CURRENTLEVEL);
            PlayerStateData playerStateData = Load<PlayerStateData>(FILENAME_STATS);
            playerStateData?.Apply();
            return;
        }

        public static void DeleteCurrentLevel()
        {
            DeleteSaveFile(FILENAME_CURRENTLEVEL);
        }

    }

}