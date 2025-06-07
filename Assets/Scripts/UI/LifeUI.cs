using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using Game.Player;
using TMPro;
using UnityEngine;


namespace Game.UI
{
    public class RemainingTimeUI : MonoSingleton<RemainingTimeUI>
    {

        [SerializeField] private TextMeshProUGUI _remainingTimeText;

        private void Start()
        {
            PlayerStats.onRemainingLivesChanged += UpdateLifeTMP;

        }

        private void UpdateLifeTMP(int remainingLives)
        {
            _remainingTimeText.text = remainingLives.ToString();
        }

        private void OnDestroy()
        {
            PlayerStats.onRemainingLivesChanged -= UpdateLifeTMP;
        }

    }

}