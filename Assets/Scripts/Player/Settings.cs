using System;
using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;


namespace Game.Player
{

    public static class Settings
    {
        #region  Events
        public static event Action<bool> onSoundEnabledChanged;
        public static event Action<bool> onVibrationEnabledChanged;
        public static event Action<bool> onColorBlindEnabledChanged;
        #endregion

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Init()
        {
#if !UNITY_EDITOR
            Application.targetFrameRate = 60;
            PrimeTweenConfig.warnEndValueEqualsCurrent = false; 
            PrimeTweenConfig.warnTweenOnDisabledTarget = false;
#endif
        }


        public static bool soundEnabled
        {
            get => _soundEnabled;
            set
            {
                _soundEnabled = value;
                if (value)
                {
                    AudioListener.volume = 1.0f;
                }
                else
                {
                    AudioListener.volume = 0.0f;
                }
                onSoundEnabledChanged?.Invoke(value);
            }
        }

        public static bool vibrationEnabled
        {
            get => _vibrationEnabled;
            set
            {
                _vibrationEnabled = value;
                if (value) Handheld.Vibrate();
                onVibrationEnabledChanged?.Invoke(value);
            }
        }

        public static bool colorBlindEnabled
        {
            get => _colorBlindEnabled;

            set
            {
                _colorBlindEnabled = value;
                Debug.Log(value ? "Color Blind Mode Enabled" : "Color Blind Mode Disabled");
                onColorBlindEnabledChanged?.Invoke(value);
            }
        }


        private static bool _soundEnabled = true;
        private static bool _vibrationEnabled = true;
        private static bool _colorBlindEnabled = false;
    }

}