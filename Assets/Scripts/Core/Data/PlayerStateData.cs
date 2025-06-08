using System.Collections;
using System.Collections.Generic;
using Game.Level;
using Game.Player;
using UnityEngine;

namespace Game.Data
{
    public class PlayerStateData
    {
        public int currentLevelIndex;
        public int coins;
        public int remainingLives;
        public int remainingTimeForRegainLife;


        public void Apply()
        {
            PlayerStats.currentLevel = currentLevelIndex;
            PlayerStats.coins = coins;
            PlayerStats.remainingLives = remainingLives;
            PlayerStats.remainingTimeForRegainLife = remainingTimeForRegainLife;
        }

        public static PlayerStateData GetCurrentData()
        {
            PlayerStateData data = new PlayerStateData();
            data.currentLevelIndex = PlayerStats.currentLevel;
            data.coins = PlayerStats.coins;
            data.remainingLives = PlayerStats.remainingLives;
            data.remainingTimeForRegainLife = PlayerStats.remainingTimeForRegainLife;
            return data;
        }
    }

}