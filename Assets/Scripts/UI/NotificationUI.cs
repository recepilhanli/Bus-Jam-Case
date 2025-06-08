using System.Collections;
using System.Collections.Generic;
using Game;
using PrimeTween;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class NotificationUI : MonoSingleton<NotificationUI>
    {
        [SerializeField] private TextMeshProUGUI _notificationTmp;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Transform _panelTransform;
        [SerializeField] private ShakeSettings _shakeSettings;
        [SerializeField] private TweenSettings _fadeSettings;

        public static void ShowNotification(string message)
        {
            if (instance == null)
            {
                Debug.LogWarning("NotificationUI instance is not initialized.");
                return;
            }

            if (string.IsNullOrEmpty(message)) return;

            instance._notificationTmp.text = message;
            instance.gameObject.SetActive(true);
            Tween.ShakeLocalPosition(instance._panelTransform, instance._shakeSettings);
            Tween.Alpha(instance._canvasGroup, 0, .5f, instance._fadeSettings);
        }
    }

}