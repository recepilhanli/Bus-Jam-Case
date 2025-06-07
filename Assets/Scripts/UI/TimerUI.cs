using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using Game.Level;
using Game.Player;
using TMPro;
using UnityEngine;


namespace Game.UI
{
    public class TimerUI : MonoSingleton<TimerUI>
    {

        [SerializeField] private TextMeshProUGUI _remainingTimeText;

        private void Start()
        {
            GameManager.instance.onTimerChanged += UpdateRemainingTimeText;
            UpdateRemainingTimeText(GameManager.instance.remainingTime);
        }

        private void UpdateRemainingTimeText(float remaining)
        {
            if (remaining <= 0) _remainingTimeText.text = "-";
            else if (remaining > 60)
            {
                int minutes = Mathf.FloorToInt(remaining / 60);
                int seconds = Mathf.FloorToInt(remaining % 60);
                _remainingTimeText.text = $"{minutes:D2}:{seconds:D2}";
            }
            else _remainingTimeText.text = $"{Mathf.FloorToInt(remaining)}s";
        }

        private void OnDestroy()
        {
            GameManager.instance.onTimerChanged -= UpdateRemainingTimeText;
        }
    }

}