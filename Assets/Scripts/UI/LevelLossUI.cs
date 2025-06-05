using System.Collections;
using System.Collections.Generic;
using Game.Level;
using PrimeTween;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{

    public class LevelLossUI : MonoSingleton<LevelLossUI>
    {

        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Button _tryAgainButton;

        // [SerializeField] private Button _KeepPlayingButton;

        public static bool enable
        {
            get => instance.gameObject.activeSelf;

            set
            {

                instance.gameObject.SetActive(value);
                if (value)
                {
                    PauseUI.enable = false;
                    instance._canvasGroup.alpha = 0f;
                    Tween.Alpha(instance._canvasGroup, .2f, 1f, .2f, startDelay: 1f);
                }

                GameManager.instance.SetEnableInput(!value);
            }
        }


        public void RestartLevel()
        {
            enable = false;
            GameManager.instance.RestartLevel();
        }

        public void ReturnToHome()
        {
            enable = false;
            GameManager.instance.ReturnToHome();
        }



    }

}