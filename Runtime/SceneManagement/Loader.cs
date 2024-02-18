using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneManagement
{
    /// <summary>
    /// Loading screen and scene transitions
    /// </summary>
    public class Loader : MonoBehaviour
    {
        public static Loader Instance { get; private set; }


        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
                Destroy(gameObject);
        }

        /// <summary>
        /// Asynchronously load a scene and display a loading screen until it's fully loaded
        /// </summary>
        /// <param name="sceneName">Name of the scene to load</param>
        public void ASync(string sceneName)
        {
            StartCoroutine(LoadSceneAsyncWithLoadingScreen(sceneName));
        }

        private IEnumerator LoadSceneAsyncWithLoadingScreen(string sceneName)
        {
            SceneManager.LoadScene("Loading");
            yield return new WaitForSeconds(0.1f);

            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
            asyncOperation.allowSceneActivation = false;

            while (asyncOperation.progress < 0.9f)
            {
                yield return null;
            }

            asyncOperation.allowSceneActivation = true;
        }
    }
}
