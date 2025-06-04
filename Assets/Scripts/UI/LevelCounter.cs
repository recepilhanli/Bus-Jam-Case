using System.Collections;
using System.Collections.Generic;
using Game;
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
    }

}