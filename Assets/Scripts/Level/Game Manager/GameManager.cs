using System.Collections;
using System.Collections.Generic;
using Game.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Level
{

    public partial class GameManager : MonoSingleton<GameManager>
    {

        public const int SCENE_HOME_INDEX = 0;

        private void Start()
        {
            InitTapping();
            InitBuses();
            InitGrids();

            //Temp
            onLevelCompleted += () => CompleteLevelUI.enable = true;
        }


        private void Update()
        {

        }


        public void Reset()
        {
            _currentColorIndex = 0;
        }

        public void ReturnToHome()
        {
            Reset();
            SceneManager.LoadScene(SCENE_HOME_INDEX);
        }


    }

}