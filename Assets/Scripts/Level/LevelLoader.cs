using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Game.Data;
using UnityEngine;
using UnityEngine.AddressableAssets;


namespace Game.Level
{

    /// <summary>
    /// Manages loading and accessing levels in the game. (Lazy loading)
    /// </summary>
    public static class LevelLoader
    {

        public const string LEVEL_CONTAINER_ADDRESS = "Levels/";
        public const int MAX_LEVEL_COUNT = 5; //Maximum number of levels in ram


        private static int _currentLevel = 0;
        private static int _currentLevelIndex = 0;
        private static bool _isInitialized = false;

        public static LevelContainer[] levelContainers => _levelContainers;
        private static LevelContainer[] _levelContainers;

        public static LevelContainer currentLevel => GetCurrentLevel();
        public static LevelContainer nextLevel => GetNextLevel();

        public static bool isInitialized => _isInitialized;


        public static async UniTask InitAsync(int startLevel)
        {
            if (isInitialized) return;
            _currentLevel = startLevel;
            _levelContainers = new LevelContainer[MAX_LEVEL_COUNT];
            await LoadLevelsAsync();
        }

        private static async UniTask LoadLevelsAsync()
        {

            for (int i = 0; i < MAX_LEVEL_COUNT; i++)
            {
                var levelAddress = $"{LEVEL_CONTAINER_ADDRESS}Level_{i}.asset";
                var levelContainer = await Addressables.LoadAssetAsync<LevelContainer>(levelAddress).Task;
                if (levelContainer != null)
                {
                    _levelContainers[i] = levelContainer;
                    Debug.Log($"Level {i} loaded from address: {levelAddress}");
                }
                else Debug.LogWarning($"Level {i} not found at address: {levelAddress}");
            }

            _isInitialized = true;
        }

        public static LevelContainer GetCurrentLevel()
        {
            if (!isInitialized)
            {
                Debug.LogError("LevelManager is not initialized. Call Init() before accessing levels.");
                return null;
            }

            return _levelContainers[_currentLevelIndex];
        }

        public static LevelContainer GetNextLevel()
        {
            if (!isInitialized)
            {
                Debug.LogError("LevelManager is not initialized. Call Init() before accessing levels.");
                return null;
            }
            int index = (_currentLevelIndex + 1 < _levelContainers.Length) ? _currentLevelIndex + 1 : 0;
            return _levelContainers[index];
        }

        public static async UniTask LoadNextLevelAsync()
        {
            if (!isInitialized)
            {
                Debug.LogError("LevelManager is not initialized. Call Init() before accessing levels.");
                return;
            }

            _currentLevel++;

            // Release the previous level container
            Addressables.Release(currentLevel);
            _levelContainers[_currentLevelIndex] = null;
            _currentLevelIndex++;

            if (_currentLevelIndex >= _levelContainers.Length) _currentLevelIndex = 0;

            var nextLevelAddress = $"{LEVEL_CONTAINER_ADDRESS}Level_{_currentLevel}";
            var nextLevelContainer = await Addressables.LoadAssetAsync<LevelContainer>(nextLevelAddress).Task;
            if (nextLevelContainer != null) _levelContainers[_currentLevelIndex - 1] = nextLevelContainer;
            else Debug.LogWarning($"Next level not found at address: {nextLevelAddress}");
        }

    }

}