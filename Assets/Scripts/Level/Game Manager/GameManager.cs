using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Level
{

    public partial class GameManager : MonoSingleton<GameManager>
    {

        private void Start()
        {
            InitTapping();
            InitBuses();
            InitGrids();
        }


        private void Update()
        {

        }


        public void Reset()
        {
            _currentColorIndex = 0;
        }

    }

}