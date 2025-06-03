using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game.Level
{

    public static class ShaderProperties
    {
        public static readonly int baseColor = Shader.PropertyToID("_BaseColor");
        public static readonly int thickness = Shader.PropertyToID("_Thickness");

    }
}