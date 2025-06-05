using System.Collections;
using System.Collections.Generic;
using Game.Level;
using PrimeTween;
using UnityEngine;


namespace Game.UI
{

    public class CompleteLevelUI : MonoSingleton<CompleteLevelUI>
    {
        [SerializeField] private CanvasGroup _canvasGroup;

        public static bool enable
        {
            get => instance.gameObject.activeSelf;

            set
            {
                PauseUI.enable = false;
                instance.gameObject.SetActive(value);
                if (value) Tween.Alpha(instance._canvasGroup, .2f, 1f, .2f);
            }
        }


        private void Start() => DontDestroyOnLoad(gameObject);

        public void NextLevel()
        {
            enable = false;
            GameManager.instance.GetNextLevel();
        }

        public void ReturnToHome()
        {
            enable = false;
            GameManager.instance.ReturnToHome();
        }
    }

}