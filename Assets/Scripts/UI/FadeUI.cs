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
    public class FadeUI : MonoSingleton<FadeUI>
    {
        [SerializeField] private Image _fadeImage;
        [SerializeField] private TweenSettings _fadeSettings;
        [SerializeField] private bool _fadeInOnAwake = true;

        protected override void Awake()
        {
            FindType();
            if (_fadeInOnAwake) FadeOut(1f, true);
        }

        public static Tween FadeIn(float timeScale = 1f)
        {
            Tween.StopAll(instance._fadeImage);
            instance._fadeImage.gameObject.SetActive(true);
            Tween tween = Tween.Alpha(instance._fadeImage, 0f, 1f, instance._fadeSettings);
            tween.timeScale = timeScale;
            return tween;
        }

        public static Tween FadeOut(float timeScale = 1f, bool disableAfterFade = false)
        {
            Tween.StopAll(instance._fadeImage);
            Tween tween = Tween.Alpha(instance._fadeImage, 1f, 0f, instance._fadeSettings);
            tween.timeScale = timeScale;

            if (disableAfterFade) tween.OnComplete(instance.Disable);
            return tween;
        }

        private void Disable() => _fadeImage.gameObject.SetActive(false);

    }
}