using System.Collections;
using System.Collections.Generic;
using Game.Level;
using Game.Player;
using UnityEngine;

namespace Game.Data
{
    public struct PlayerStateData
    {
        public int currentLevelIndex;
        public int coins;
        public int remainingLives;


        public void Assign()
        {
            PlayerStats.currentLevel = currentLevelIndex;
            PlayerStats.coins = coins;
            PlayerStats.remainingLives = remainingLives;
        }

        public static PlayerStateData GetCurrentData()
        {
            PlayerStateData data = new PlayerStateData();
            data.currentLevelIndex = PlayerStats.currentLevel;
            data.coins = PlayerStats.coins;
            data.remainingLives = PlayerStats.remainingLives;
            return data;
        }
    }

}