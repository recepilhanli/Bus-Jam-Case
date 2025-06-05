using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Game.Data;
using Game.Level.Pooling;
using Game.UI;
using UnityEngine;

namespace Game.Level
{
    public partial class GameManager
    {

        private async UniTask InitLevels()
        {
            await LevelLoader.InitAsync(0); //Temp Will be changed with WaitUntil LevelLoader is ready
            LevelContainer currentLevel = LevelLoader.currentLevel;
            if (currentLevel == null)
            {
                Debug.LogError("Current level is null. Make sure levels are loaded correctly.");
                return;
            }

            onLevelCompleted += () => CompleteLevelUI.enable = true;
            InitLevel(currentLevel);
        }

        public void GetNextLevel()
        {
            PoolManager.ResetAllPools();
            Reset();
            InitBuses();

            LevelContainer nextLevel = LevelLoader.nextLevel;
            if (nextLevel == null)
            {
                Debug.LogError("Next level is null. Cannot initialize next level.");
                return;
            }

            _ = LevelLoader.LoadNextLevelAsync(); //Lazy loading next level

            InitLevel(nextLevel);
        }

        private void InitLevel(LevelContainer levelContainer)
        {
            if (levelContainer == null)
            {
                Debug.LogError("Level container is null. Cannot initialize level.");
                return;
            }

            primaryGrid.Init(levelContainer.primaryGrid);
            secondaryGrid.Init(levelContainer.secondaryGrid);
        }

    }
}