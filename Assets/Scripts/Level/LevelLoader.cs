using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Game.Data;
using Game.Player;
using UnityEngine;
using UnityEngine.AddressableAssets;


namespace Game.Level
{

    /// <summary>
    /// Manages loading and accessing levels in the game. (Lazy loading)
    /// </summary>
    public static class LevelLoader
    {
        public const string LEVEL_PREFIX = "Level_";
        public const string LEVEL_FOLDER = "Assets/Scenes/Levels/";

        public const string LEVEL_CONTAINER_ADDRESS = "Levels/";
        public const int MAX_LEVEL_COUNT = 5; //Maximum number of levels in ram

        private static int _currentLevelIndex = 0;
        private static bool _isInitialized = false;

        public static LevelContainer[] levelContainers => _levelContainers;
        private static LevelContainer[] _levelContainers;

        public static LevelContainer currentLevel => GetCurrentLevel();
        public static LevelContainer nextLevel => GetNextLevel();

        public static bool isInitialized => _isInitialized;


        public static async UniTask InitAsync()
        {
            if (isInitialized) return;
            _levelContainers = new LevelContainer[MAX_LEVEL_COUNT];
            await LoadLevelsAsync();
        }

        public static async UniTask Reload()
        {
            if (!isInitialized) return;

            foreach (var level in _levelContainers)
            {
                if (level != null)
                {
                    Addressables.Release(level);
                }
            }

            _levelContainers = new LevelContainer[MAX_LEVEL_COUNT];
            await LoadLevelsAsync();
        }


        public static void InitPersistent(LevelContainer level)
        {
            if (isInitialized) return;
            Debug.Assert(level != null, "Level cannot be null when initializing LevelLoader.");
            _levelContainers = new LevelContainer[MAX_LEVEL_COUNT];
            _levelContainers[0] = level; // Initialize the first level with the provided level
            _currentLevelIndex = 0; // Set the current level index to 0
            _isInitialized = true;
            PlayerStats.currentLevel = 1;
        }

        private static async UniTask LoadLevelsAsync()
        {

            for (int i = 0; i < MAX_LEVEL_COUNT; i++)
            {
                var levelAddress = $"{LEVEL_CONTAINER_ADDRESS}{LEVEL_PREFIX}{i + PlayerStats.currentLevel}.asset";
                try
                {

                    var levelContainer = await Addressables.LoadAssetAsync<LevelContainer>(levelAddress).Task;
                    if (levelContainer != null)
                    {
                        _levelContainers[i] = levelContainer;
                        Debug.Log($"Level {i} loaded from address: {levelAddress}");
                    }
                }
                catch
                {
                    Debug.LogWarning($"Level {i} not found at address: {levelAddress}");
                }
            }

            _isInitialized = true;
        }

        public static LevelContainer GetCurrentLevel()
        {
            if (!isInitialized)
            {
                Debug.LogError("LevelLoader is not initialized. Call Init() before accessing levels.");
                return null;
            }

            return _levelContainers[_currentLevelIndex];
        }

        public static LevelContainer GetNextLevel()
        {
            if (!isInitialized)
            {
                Debug.LogError("LevelLoader is not initialized. Call Init() before accessing levels.");
                return null;
            }
            int index = (_currentLevelIndex + 1 < _levelContainers.Length) ? _currentLevelIndex + 1 : 0;
            return _levelContainers[index];
        }

        public static async UniTask LoadNextLevelAsync()
        {
            if (!isInitialized)
            {
                Debug.LogError("LevelLoader is not initialized. Call Init() before accessing levels.");
                return;
            }

            // Release the previous level container
            Addressables.Release(currentLevel);
            _levelContainers[_currentLevelIndex] = null;
            _currentLevelIndex++;

            if (_currentLevelIndex >= _levelContainers.Length) _currentLevelIndex = 0;

            var nextLevelAddress = $"{LEVEL_CONTAINER_ADDRESS}Level_{PlayerStats.currentLevel}.asset";
            var nextLevelContainer = await Addressables.LoadAssetAsync<LevelContainer>(nextLevelAddress).Task;
            if (nextLevelContainer != null) _levelContainers[_currentLevelIndex - 1] = nextLevelContainer;
            else Debug.LogWarning($"Next level not found at address: {nextLevelAddress}");
        }



    }

}