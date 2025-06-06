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
        public const int HOME_SCENE_INDEX = 0;
        public const int GAME_SCENE_INDEX = 1;
        public const int LEVEL_EDITOR_SCENE_INDEX = 2;

        public static event Action<Scene, Scene> onSceneChanged;
        public static event Action<Scene> onRequestSceneLoad;

        public static int currentSceneIndex => SceneManager.GetActiveScene().buildIndex;
        public static bool isGameScene => currentSceneIndex == GAME_SCENE_INDEX;
        public static bool isHomeScene => currentSceneIndex == HOME_SCENE_INDEX;


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