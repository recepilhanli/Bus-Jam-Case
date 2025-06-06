using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Game.Data;
using Game.Level.Pooling;
using Game.Player;
using Game.UI;
using UnityEngine;

namespace Game.Level
{
    public partial class GameManager
    {


        private async UniTask InitLevels()
        {
            if (SceneHelper.isGameScene) await LevelLoader.InitAsync(); //Temp

            LevelContainer currentLevel = LevelLoader.currentLevel;
            if (currentLevel == null)
            {
                Debug.LogError("Current level is null. Make sure levels are loaded correctly.");
                return;
            }

            onLevelCompleted += () => CompleteLevelUI.enable = true;
            onLevelFailed += () => LevelLossUI.enable = true;

            InitLevelContainer(currentLevel);
        }

        public void GetNextLevel()
        {
            PoolManager.ResetAllPools();
            Reset();

            LevelContainer nextLevel = LevelLoader.nextLevel;
            if (nextLevel == null)
            {
                Debug.LogError("Next level is null. Returning to home.");
                ReturnToHome();
                return;
            }

            FadeUI.FadeOut(2.5f, true);

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

        public void RestartLevel()
        {
            PlayerStats.remainingLives--;
            //TO DO: Implenent remaining lives system
            if (PlayerStats.remainingLives > 0) LoadLevel(LevelLoader.currentLevel);
            else
            {
                Debug.Log("No remaining lives. Returning to home.");
                ReturnToHome();
            }
        }

        [ContextMenu("Return to Home")]
        public void ReturnToHome()
        {
            Reset();
            FadeUI.FadeIn(5f).OnComplete(ReturnHome);
        }

        private void ReturnHome() => SceneHelper.LoadScene(SceneHelper.HOME_SCENE_INDEX);



    }
}