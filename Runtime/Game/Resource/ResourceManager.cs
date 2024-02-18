using Cysharp.Threading.Tasks;
using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using TemplateTable;

public class ResourceManager
{
    public async UniTask Initialize()
    {
        await LoadDataTableAsync(false);
    }

    public void Uninitialize()
    {
        UnloadDataTable();
    }

    private async UniTask<bool> LoadDataTableAsync(bool _delayLoad)
    {
        var _hasError = false;
        var _dataLoader = new DataTableLoader
        {
            onLoadAsync = ResourceLoader.LoadTextDataFromResourcesAsync,
            onError = delegate(string _name, Exception _e)
            {
                _hasError = true;
                Debug.LogError($"Error: {_name}\n{_e}");
            }
        };

        var _targetFields = typeof(DataTable)
            .GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
            .Where(x => x.FieldType.IsGenericType &&
                        x.FieldType.GetGenericTypeDefinition() == typeof(TemplateTable<,>))
            .Select(x => x)
            .ToList();

        foreach (var x in _targetFields)
        {
            string _tableAddress = $"Table/{x.Name}";
            await _dataLoader.LoadTableAsync(x, _tableAddress, _delayLoad);
        }

        if (_hasError)
            return false;
        Debug.Log("Table Data Load Complete!");
        return true;
    }

    private void UnloadDataTable()
    {
        var _targetFields = typeof(DataTable)
            .GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
            .Where(x => x.FieldType.IsGenericType &&
                        x.FieldType.GetGenericTypeDefinition() == typeof(TemplateTable<,>))
            .Select(x => x)
            .ToList();

        foreach (var x in _targetFields)
            x.SetValue(null, null);
    }
}