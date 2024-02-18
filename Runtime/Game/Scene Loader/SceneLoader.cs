using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    public enum SceneType
    {
        Menu,
        Shop,
        mainTest,
        Story
    }

    private static readonly Dictionary<SceneType, string> SceneName = new Dictionary<SceneType, string>()
    {
        { SceneType.Menu, "Menu" },
        { SceneType.Shop, "Shop" },
        { SceneType.mainTest, "main Test" },
        { SceneType.Story, "Story" },
    };

    private static Dictionary<SceneType, AsyncOperation> Scenes = new Dictionary<SceneType, AsyncOperation>();

    public static async UniTask LoadSceneAsync(SceneType type)
    {
        if (GameManager.Instance != null && GameManager.Instance.userDataManager != null)
        {
            GameManager.Instance.userDataManager.Update();
        }

        SceneManager.LoadScene("Loading");
        await UniTask.Delay(System.TimeSpan.FromSeconds(0.1f));

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(SceneName[type]);
        asyncOperation.allowSceneActivation = false;

        await UniTask.WaitUntil(() => asyncOperation.progress >= 0.9f);
        asyncOperation.allowSceneActivation = true; 
    }

    public static async UniTask LoadAdditiveSceneAsync(SceneType newType)
    {
        if (Scenes.ContainsKey(newType))
            Scenes[newType].allowSceneActivation = true;
        else
        {
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(SceneName[newType], LoadSceneMode.Additive);
            await UniTask.WaitUntil(() => asyncOperation.progress >= 0.9f);
            Scenes.Add(newType, asyncOperation);
            asyncOperation.allowSceneActivation = true;
        }
    }

    public static async UniTask UnLoadAdditiveSceneAsync(SceneType type)
    {
        Scenes[type].allowSceneActivation = false;
    }
}