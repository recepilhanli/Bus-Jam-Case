using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
            InitLevelContainer(currentLevel);
        }

        public void GetNextLevel()
        {
            PoolManager.ResetAllPools();
            Reset();

            LevelContainer nextLevel = LevelLoader.nextLevel;
            if (nextLevel == null)
            {
                Debug.LogError("Next level is null. Cannot initialize next level.");
                return;
            }

            _ = LevelLoader.LoadNextLevelAsync(); //Lazy loading next level

            InitLevelContainer(nextLevel);
        }

        public void LoadLevel(LevelContainer levelContainer)
        {
            Debug.Assert(levelContainer != null, "Level container cannot be null when loading a level.");

            PoolManager.ResetAllPools();
            Reset();

            InitLevelContainer(levelContainer);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void InitLevelContainer(LevelContainer levelContainer)
        {
            if (levelContainer == null)
            {
                Debug.LogError("Level container is null. Cannot initialize level.");
                return;
            }

            primaryGrid.Init(levelContainer.primaryGrid);
            secondaryGrid.Init(levelContainer.secondaryGrid);
            LoadBuses(in levelContainer.busData);

            CheckSecondaryGridFrontLine();
        }

    }
}