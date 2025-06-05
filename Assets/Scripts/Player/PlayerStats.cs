using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Game.Data;
using UnityEngine;


namespace Game.Player
{

    public static class PlayerStats
    {
        private static int _currentLevel = 1;
        private static int _coins = 0;
        private static int _remainingLives = 3;

        public static int remainingLives
        {
            get => _remainingLives;
            set
            {
                _remainingLives = value;
                if (_remainingLives < 0) _remainingLives = 0;
            }
        }

        public static int currentLevel
        {
            get => _currentLevel;
            set
            {
                _currentLevel = value;
                if (_currentLevel < 0) _currentLevel = 0;
            }
        }


        public static int coins
        {
            get => _coins;
            set
            {
                _coins = value;
                if (_coins < 0) _coins = 0;
            }
        }


        //TO DO: Implement a save system

    }

}