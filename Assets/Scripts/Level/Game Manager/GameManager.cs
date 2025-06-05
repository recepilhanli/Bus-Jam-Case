using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.UI;

using UnityEngine;

namespace Game.Level
{

    public partial class GameManager : MonoSingleton<GameManager>
    {

        public const int SCENE_HOME_INDEX = 0;

        private async UniTaskVoid Start()
        {
            InitTapping();
            InitBuses();
            await InitLevels();
            InitGrids();
        }


        private void Update()
        {

        }


        public void Reset()
        {
            _currentColorIndex = 0;
            
        }

        [ContextMenu("Return to Home")]
        public void ReturnToHome()
        {
            Reset();
            SceneHelper.LoadScene(SCENE_HOME_INDEX);
        }


    }

}