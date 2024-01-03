using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneManagement
{
    public class Access : MonoBehaviour
    {
        public static Access Instance { get; private set; }

        private Dictionary<string, Scene> Scenes = new Dictionary<string, Scene>();


        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);
            Initialize();
        }

        private void Initialize()
        {
            try
            {
                int length = SceneManager.sceneCountInBuildSettings;

                for (int i = 0; i < length; i++)
                {
                    Scene currentScene = SceneManager.GetSceneByBuildIndex(i);
                    Scenes.Add(currentScene.name, currentScene);
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError("Scene Access dose not Initialize" + e);
            }
        }

        public Scene GetScene(string sceneName)
        {
            if (Scenes.TryGetValue(sceneName, out Scene scene))
            {
                return scene;
            }
            else
            {
                Debug.LogError("Scene not found: " + sceneName);
                return default; 
            }
        }
    }
}