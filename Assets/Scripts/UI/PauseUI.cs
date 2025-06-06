using System.Collections;
using System.Collections.Generic;
using Game.Level;
using Game.Player;
using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace Game.UI
{
    public class PauseUI : MonoSingleton<PauseUI>
    {

        #region Fields
        [SerializeField] private GameObject _root;
        [Tooltip("General Canvas Group"), SerializeField] private CanvasGroup _canvasGroup;

        [Header("Return Home")]
        [Tooltip("Disabled In Main Menu"), SerializeField] private Button _returnHomeButton;
        [SerializeField] private GameObject _returnHomeWarnPanel;
        [SerializeField] private CanvasGroup _returnHomeWarnCanvasGroup;

        [Header("Settings")]
        [SerializeField] private TextMeshProUGUI _soundTMP;
        [SerializeField] private TextMeshProUGUI _vibrationTMP;
        [SerializeField] private TextMeshProUGUI _colorBlindTMP;
        #endregion

        public static bool enable
        {
            set
            {
                instance._root.SetActive(value);
                if (GameManager.instance) GameManager.instance.SetEnableInput(!value);

                if (value)
                {
                    if (SceneManager.GetActiveScene().buildIndex == SceneHelper.HOME_SCENE_INDEX) instance._returnHomeButton.gameObject.SetActive(false);
                    else instance._returnHomeButton.gameObject.SetActive(true);
                    instance._returnHomeWarnPanel.SetActive(false);
                    Tween.Alpha(instance._canvasGroup, .2f, 1f, .2f);
                }
            }
        }

        private void Start() => DontDestroyOnLoad(gameObject);


        #region UI Actions

        public void ToggleSound()
        {
            Settings.soundEnabled = !Settings.soundEnabled;
            _soundTMP.text = Settings.soundEnabled ? "Disable Sound" : "Enable Sound";
        }

        public void ToggleVibration()
        {
            Settings.vibrationEnabled = !Settings.vibrationEnabled;
            _vibrationTMP.text = Settings.vibrationEnabled ? "Disable Vibration" : "Enable Vibration";
        }

        public void ToggleColorBlind()
        {
            Settings.colorBlindEnabled = !Settings.colorBlindEnabled;
            _colorBlindTMP.text = Settings.colorBlindEnabled ? "Disable Color Blind" : "Enable Color Blind";
        }


        public void ShowReturnHomePanel()
        {
            _returnHomeWarnPanel.SetActive(true);
            Tween.Alpha(_returnHomeWarnCanvasGroup, 0f, 1f, .2f);
        }

        public void ReturnHome()
        {
            _returnHomeWarnPanel.SetActive(false);
            GameManager.instance.ReturnToHome();
        }

        public void Close() => enable = false;
        public void Open() => enable = true;


        #endregion



    }
}