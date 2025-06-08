using System.Collections;
using System.Collections.Generic;
using Game.Level;
using Game.Player;
using TMPro;
using UnityEngine;


namespace Game.UI
{
    public class MainMenuUI : MonoSingleton<MainMenuUI>
    {

        public void StartGame()
        {
            if(PlayerStats.remainingLives <= 0)
            {
                NotificationUI.ShowNotification("You have no lives left. Please wait for life regain timer.");
                return;
            }
            FadeUI.FadeIn(5f).OnComplete(LoadGameScene);
        }

        private void LoadGameScene() => SceneHelper.LoadScene(SceneHelper.GAME_SCENE_INDEX);

    }

}