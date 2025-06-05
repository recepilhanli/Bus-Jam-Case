using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



namespace Game.Level
{
    //A wrapper class that helps to manage scene loading and scene change events.
    public static class SceneHelper
    {
        public static event Action<Scene, Scene> onSceneChanged;
        public static event Action<Scene> onRequestSceneLoad;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Initialize()
        {
            SceneManager.activeSceneChanged += OnSceneChanged;
        }

        private static void OnSceneChanged(Scene oldScene, Scene newScene) => onSceneChanged?.Invoke(oldScene, newScene);

        public static void LoadScene(int sceneIndex)
        {
            onRequestSceneLoad?.Invoke(SceneManager.GetSceneByBuildIndex(sceneIndex));
            SceneManager.LoadScene(sceneIndex);
        }

        public static void LoadScene(string sceneName)
        {
            onRequestSceneLoad?.Invoke(SceneManager.GetSceneByName(sceneName));
            SceneManager.LoadScene(sceneName);
        }


    }

}