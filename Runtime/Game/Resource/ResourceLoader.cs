using Cysharp.Threading.Tasks;
using UnityEngine;

public static class ResourceLoader
{
    public static async UniTask<object> LoadTextDataFromResourcesAsync(string _addressableName)
    {
        var _resource = await Resources.LoadAsync<TextAsset>(_addressableName);
        return _resource.ToString();
    }
}