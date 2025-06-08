using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Cysharp.Threading.Tasks;
using Game.Data;
using UnityEngine;


namespace Game.Player
{

    public static class PlayerStats
    {
        public const int DEFAULT_LIFE_COUNT = 3;
        public const int REGAIN_LIFE_TIME = 60; // seconds

        public static event Action<int> onLevelNumberChanged;
        public static event Action<int> onCoinsChanged;
        public static event Action<int> onRemainingLivesChanged;
        public static event Action<int> onRegainLifeTimerChanged;
        public static event Action onRegainLifeEnd;

        private static int _currentLevel = 1;
        private static int _coins = 0;
        private static int _remainingLives = 3;

        private static bool _regainingLife = false;
        private static int _remainingTimeForRegainLife = 0;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void CheckAndRegainLife()
        {
            if (remainingLives < DEFAULT_LIFE_COUNT && !_regainingLife)
            {
                _regainingLife = true;
                RegainLifeTimer().Forget();
            }
        }

        private static async UniTaskVoid RegainLifeTimer()
        {
            Debug.Log("Regaining life timer started, remaining lives: " + remainingLives);

            if (_remainingTimeForRegainLife == 0) _remainingTimeForRegainLife = REGAIN_LIFE_TIME;

            while (_regainingLife && _remainingLives < DEFAULT_LIFE_COUNT)
            {
                _remainingTimeForRegainLife--;
                onRegainLifeTimerChanged?.Invoke(_remainingTimeForRegainLife);

                if (_remainingTimeForRegainLife <= 0)
                {
                    remainingLives++;
                    _remainingTimeForRegainLife = REGAIN_LIFE_TIME;
                    Debug.Log("Life regained! Remaining lives: " + remainingLives);
                    SaveManager.SavePlayerState();
                }

                await UniTask.WaitForSeconds(1f);

                Debug.Log("Tick");
            }

            Debug.Log("Regaining life ended, remaining lives: " + remainingLives);
            onRegainLifeEnd?.Invoke();
            _regainingLife = false;
        }


        public static int remainingLives
        {
            get => _remainingLives;
            set
            {
                _remainingLives = value;
                if (_remainingLives < 0) _remainingLives = 0;
                onRemainingLivesChanged?.Invoke(value);
            }
        }

        public static int currentLevel
        {
            get => _currentLevel;
            set
            {
                _currentLevel = value;
                if (_currentLevel < 0) _currentLevel = 0;
                onLevelNumberChanged?.Invoke(value);
            }
        }


        public static int coins
        {
            get => _coins;
            set
            {
                _coins = value;
                if (_coins < 0) _coins = 0;
                onCoinsChanged?.Invoke(value);
            }
        }

        public static int remainingTimeForRegainLife
        {
            get => _remainingTimeForRegainLife;
            set
            {
                _remainingTimeForRegainLife = value;
                if (_remainingTimeForRegainLife < 0) _remainingTimeForRegainLife = 0;
            }
        }

        public static bool isRegainingLife
        {
            get => _regainingLife;
            set
            {
                if (_regainingLife == value) return; // No change, do nothing
                else if (value) CheckAndRegainLife();
            }
        }




    }

}