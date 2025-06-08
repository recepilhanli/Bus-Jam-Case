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

        [SerializeField, Space] private bool _editMode = true;

        private async UniTask InitLevels()
        {
            if (!LevelLoader.isInitialized) await UniTask.WaitUntil(() => LevelLoader.isInitialized);

            LevelContainer currentLevel = LevelLoader.currentLevel;
            if (currentLevel == null)
            {
                Debug.LogError("Current level is null. Make sure levels are loaded correctly.");
                return;
            }

            onLevelCompleted += () => CompleteLevelUI.enable = true;
            onLevelFailed += HandleLevelLoss;


            InitLevelContainer(currentLevel);
        }


        public void GetNextLevel()
        {
            PoolManager.ResetAllPools();
            SaveManager.DeleteCurrentLevel();
            Reset();

            LevelContainer nextLevel = LevelLoader.nextLevel;
            if (nextLevel == null)
            {
                Debug.LogError("Next level is null. Returning to home.");
                NotificationUI.ShowNotification("No more levels available, level progression is reset.");
                PlayerStats.currentLevel = 1;
                SaveManager.SavePlayerState();
                ReturnToHome();
                return;
            }

            FadeUI.FadeOut(2.5f, true);
            PlayerStats.currentLevel++;
            SaveManager.SavePlayerState(); //Save player state before loading next level

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



            var obstacles = levelContainer.secondaryGrid.obstacles;
            if (obstacles != null)
            {
                foreach (var obstacleData in obstacles)
                {
                    var pos = obstacleData.gridPosition;
                    if (secondaryGrid.IsValidPosition(pos)) secondaryGrid.cells[pos.x, pos.y].isObstacle = true;
                }
            }

            if (SaveManager.currentLevelData != null && !_editMode)
            {
                LoadPassengers(SaveManager.currentLevelData);
                LoadBuses(SaveManager.currentLevelData);
                InitTimer(SaveManager.currentLevelData.remainingTime);
            }
            else
            {
                LoadPassengers(levelContainer);
                LoadBuses(in levelContainer.busData);
                InitTimer(levelContainer.timeConstraint);
            }


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
                NotificationUI.ShowNotification("No lives left. Returning to home.");
                ReturnToHome();
            }

            SaveManager.SavePlayerState();
            SaveManager.DeleteCurrentLevel();
        }

        [ContextMenu("Return to Home")]
        public void ReturnToHome()
        {
            Reset();
            FadeUI.FadeIn(5f).OnComplete(ReturnHome);
        }

        private void ReturnHome()
        {
            SaveManager.SavePlayerState();
            SaveManager.DeleteCurrentLevel();
            SceneHelper.LoadScene(SceneHelper.HOME_SCENE_INDEX);
        }

        public void GiveUp()
        {
            PlayerStats.remainingLives--;
            ReturnToHome();
        }

        private void HandleLevelLoss()
        {
            LevelLossUI.enable = true;
            StopTimer();
            SaveManager.SavePlayerState();
            SaveManager.DeleteCurrentLevel();
        }



    }
}