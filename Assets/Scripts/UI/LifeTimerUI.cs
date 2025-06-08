
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

    public class LifeTimerUI : MonoSingleton<LifeTimerUI>
    {

        [SerializeField] private TextMeshProUGUI _timerText;

        private async UniTaskVoid Start()
        {
            await UniTask.DelayFrame(3); // Wait for the first frame to ensure PlayerStats is initialized

            if (_timerText == null)
            {
                Debug.LogError("Timer Text is not assigned in LifeTimerUI.");
                return;
            }

            if (!PlayerStats.isRegainingLife)
            {
                if (PlayerStats.remainingLives < PlayerStats.DEFAULT_LIFE_COUNT)
                {
                    PlayerStats.isRegainingLife = true;
                    Debug.Log("Starting life regain timer.");
                }
                else
                {
                    gameObject.SetActive(false);
                    return;
                }
            }

            if (!this) return;

            PlayerStats.onRegainLifeEnd += Disable;
            PlayerStats.onRegainLifeTimerChanged += UpdateTimerText;
            UpdateTimerText(PlayerStats.remainingTimeForRegainLife);
        }

        private void Disable() => gameObject.SetActive(false);

        private void UpdateTimerText(int timerSeconds)
        {
            if (_timerText == null)
            {
                Debug.LogError("Timer Text is not assigned in LifeTimerUI.");
                return;
            }

            if (timerSeconds < 0)
            {
                _timerText.text = "00:00";
                return;
            }

            int minutes = Mathf.FloorToInt((float)timerSeconds / 60);
            int seconds = Mathf.FloorToInt((float)timerSeconds % 60);
            _timerText.text = $"{minutes:D2}:{seconds:D2}";
        }


        private void OnDestroy()
        {
            PlayerStats.onRegainLifeTimerChanged -= UpdateTimerText;
            PlayerStats.onRegainLifeEnd -= Disable;
        }

    }
}
