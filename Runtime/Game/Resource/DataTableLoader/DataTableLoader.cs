using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;
using TemplateTable;

public class DataTableLoader
{
    public Func<string, UniTask<object>> onLoadAsync;
    public Action<string, Exception> onError;

    private string lastDataName = string.Empty;

    private static readonly MethodInfo LoadTableGenericAsyncMethodInfo =
        typeof(DataTableLoader).GetMethod(nameof(LoadTableGenericAsync), BindingFlags.Instance | BindingFlags.NonPublic);

    public UniTask LoadTableAsync(FieldInfo _tableField, string _tableAddress, bool _delayLoad)
    {
        var _method =
            LoadTableGenericAsyncMethodInfo.MakeGenericMethod(_tableField.FieldType.GetGenericArguments()[1]);
        return (UniTask)_method.Invoke(this, new object[] { _tableField, _tableAddress, _delayLoad });
    }

    private async UniTask LoadTableGenericAsync<TValue>(
        FieldInfo _tableField,
        string _tableAddress,
        bool _delayLoad)
        where TValue : class, new()
    {
        try
        {
            var _data = await LoadDataAsync(_tableAddress);

            ITemplateTableLoader<int, TValue> _loader = null;

            if (_data is string)
            {
                _loader = new TemplateTableJsonLoader<int, TValue>(
                    new JsonTextReader(new StringReader((string)_data)),
                    JsonSerializer.Create(JsonUnityHelper.DeserializerSettings),
                    _delayLoad);
            }
            else if (_data is Stream)
            {
                _loader = new TemplateTableBsonPackLoader<int, TValue>(
                    (Stream)_data,
                    JsonSerializer.Create(JsonUnityHelper.DeserializerSettings),
                    _delayLoad);
            }

            if (_loader == null)
                throw new Exception($"LoadData('{_tableAddress}') failed");

            var _table = (TemplateTable<int, TValue>)_tableField.GetValue(null); // static 멤버를 가져옵니다.
            if (_table == null)
            {
                _table = new TemplateTable<int, TValue>();
                _table.Load(_loader);
                _tableField.SetValue(null, _table);
            }
            else
            {
                _table.Update(_loader);
            }
        }
        catch (Exception _e)
        {
            if (onError != null)
                onError(lastDataName, _e);
        }
    }

    private async UniTask<object> LoadDataAsync(string _name)
    {
        lastDataName = _name;
        if (onLoadAsync != null)
            return await onLoadAsync(_name);
        return default;
    }
}