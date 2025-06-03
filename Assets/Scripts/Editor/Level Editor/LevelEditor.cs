using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Game.OnlyEditor
{


    public class LevelEditor : EditorWindow
    {

        [MenuItem("Window/Level Editor")]
        public static void Open()
        {
            LevelEditor window = GetWindow<LevelEditor>("Level Editor");
            window.minSize = new Vector2(400, 300);
            window.Show();
        }

    }

}