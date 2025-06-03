using System.Collections;
using System.Collections.Generic;
using Game.Utils;
using UnityEngine;


namespace Game.Level
{
    public partial class Bus
    {

        [Header("Appearance")]
        public Renderer _renderer;
        private ColorList _color;

        public ColorList Color
        {
            get => _color;
            set
            {
                _color = value;
                ColorHelper.SetRendererColor(_renderer, _color);
            }
        }

    }

}