using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using Game.Player;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class LevelCounterUI : MonoSingleton<LevelCounterUI>
    {
        [SerializeField] private TextMeshProUGUI _levelText;

        public static int levelNumber
        {
            set => instance._levelText.text = value.ToString();
        }

        private void Start()
        {
            levelNumber = PlayerStats.currentLevel;
            PlayerStats.onLevelNumberChanged += UpdateLevelText;
        }


        private void OnDestroy()
        {
            PlayerStats.onLevelNumberChanged -= UpdateLevelText;
        }

        private void UpdateLevelText(int number)
        {
            levelNumber = number;
        }
    }

}