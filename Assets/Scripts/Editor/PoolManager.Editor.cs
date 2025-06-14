using System.Collections;
using System.Collections.Generic;
using Game.Level.Pooling;
using UnityEditor;
using UnityEngine;

namespace Game.OnlyEditor
{

    class PoolManagerEditor : EditorWindow
    {

        private Vector2 _scrollPosition;
        private PoolTypes[] _poolTypes;
        private PoolTypes _selectedPoolType = PoolTypes.Unkown;



        [MenuItem("Debug/Pool Manager")]
        private static void Init()
        {
            PoolManagerEditor window = (PoolManagerEditor)GetWindow(typeof(PoolManagerEditor));
            window.titleContent = new GUIContent("Pool Manager");
            window.Show();
        }

        private void OnEnable()
        {
            _scrollPosition = Vector2.zero;
            _poolTypes = (PoolTypes[])System.Enum.GetValues(typeof(PoolTypes));
        }


        private void OnGUI()
        {
            if (Application.isPlaying == false)
            {
                GUILayout.Label("Pool Manager is only available in play mode.", EditorStyles.boldLabel);
                return;
            }

            EditorGUILayout.HelpBox("This window shows the current state of the object pools in the game.", MessageType.Info);
            EditorGUILayout.Space();
            ListPoolTypes();
        }

        private void ListPoolTypes()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Select Pool Type:", GUILayout.Width(120));
            _selectedPoolType = (PoolTypes)EditorGUILayout.EnumPopup(_selectedPoolType);
            EditorGUILayout.EndHorizontal();
            if (_selectedPoolType != PoolTypes.Unkown) DrawPoolType(_selectedPoolType);
        }

        private void DrawPoolType(PoolTypes poolType)
        {
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
            GUILayout.Label(poolType.ToString(), EditorStyles.boldLabel);
            EditorGUILayout.Space();

            EditorGUILayout.BeginVertical("box");

            var pool = PoolManager.GetPool(poolType);

            if (pool == null)
            {
                GUILayout.Label("Pool is not initialized or does not exist.", EditorStyles.boldLabel);
                EditorGUILayout.Space();
                return;
            }

            GUILayout.Label($"Pooled Objects: {pool.PooledObjects.Count}");
            foreach (var pooledObject in pool.PooledObjects)
            {
                //draw field
                var mono = pooledObject as MonoBehaviour;
                EditorGUILayout.ObjectField(mono, typeof(MonoBehaviour), true);
            }

            GUILayout.Label($"Active Objects: {pool.ActiveObjects.Count}");
            foreach (var activeObject in pool.ActiveObjects)
            {
                var mono = activeObject as MonoBehaviour;
                EditorGUILayout.ObjectField(mono, typeof(MonoBehaviour), true);
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
        }

    }

}