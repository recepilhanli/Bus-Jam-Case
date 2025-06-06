using System.Collections;
using System.Collections.Generic;
using Game.Level;
using TMPro;
using UnityEngine;


namespace Game.UI
{
    public class MainMenuUI : MonoSingleton<MainMenuUI>
    {

        public void StartGame()
        {
            FadeUI.FadeIn(5f).OnComplete(LoadGameScene);
        }

        private void LoadGameScene() => SceneHelper.LoadScene(SceneHelper.GAME_SCENE_INDEX);

    }

}