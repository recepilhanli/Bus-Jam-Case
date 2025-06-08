using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game;
using Game.Player;
using TMPro;
using UnityEngine;


namespace Game.UI
{
    public class RemainingTimeUI : MonoSingleton<RemainingTimeUI>
    {

        [SerializeField] private TextMeshProUGUI _remainingTimeText;

        private async UniTaskVoid Start()
        {
            await UniTask.DelayFrame(3); // Wait for the first frame to ensure PlayerStats is initialized
            if (!this) return;
            PlayerStats.onRemainingLivesChanged += UpdateLifeTMP;
            UpdateLifeTMP(PlayerStats.remainingLives);
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