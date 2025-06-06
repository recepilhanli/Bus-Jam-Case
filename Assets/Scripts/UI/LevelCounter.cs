using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using Game.Player;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class LevelCounterUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private bool _countWithLevelText = true;

        public int levelNumber
        {
            set
            {
                if (!_countWithLevelText) _levelText.text = value.ToString();
                else _levelText.text = $"Level {value}";
            }
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