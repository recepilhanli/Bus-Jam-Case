using System.Collections;
using System.Collections.Generic;
using Game.Level;
using UnityEngine;



namespace Game.Utils
{

    public enum ColorList
    {
        White,
        Red,
        Green,
        Blue,
        Yellow,
        Orange,
        Pink,
        Purple,
        Cyan,
    }


    public static class ColorHelper
    {
        private static readonly MaterialPropertyBlock _propertyBlock = new MaterialPropertyBlock();

        public static Color GetColor(ColorList color)
        {
            switch (color)
            {
                case ColorList.White: return Color.white;
                case ColorList.Red: return Color.red;
                case ColorList.Green: return Color.green;
                case ColorList.Blue: return Color.blue;
                case ColorList.Yellow: return Color.yellow;
                case ColorList.Orange: return new Color(1.0f, 0.5f, 0.0f);
                case ColorList.Pink: return new Color(1.0f, 0.75f, 0.8f);
                case ColorList.Purple: return new Color(0.5f, 0.0f, 0.5f);
                case ColorList.Cyan: return new Color(0.0f, 1.0f, 1.0f);
                default: return Color.white;
            }
        }

        public static void SetRendererColor(Renderer renderer, ColorList color)
        {
            _propertyBlock.SetColor(ShaderProperties.baseColor, GetColor(color));
            renderer.SetPropertyBlock(_propertyBlock);
        }

        public static void SetRendererColor(SpriteRenderer renderer, ColorList color)
        {
            renderer.color = GetColor(color);
        }

        public static ColorList GetRandomColor()
        {
            int randomIndex = Random.Range(0, System.Enum.GetValues(typeof(ColorList)).Length);
            return (ColorList)randomIndex;
        }


        public static Color ToColor(this ColorList color) => GetColor(color);

    }

}