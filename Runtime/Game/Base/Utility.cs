using UnityEngine;

public static class Utility
{
    public static bool IsNullOrWhiteSpaceEx(this string _value)
    {
        if (string.IsNullOrWhiteSpace(_value))
            return true;

        if (_value == "null")
            return true;

        return false;
    }
    
    public static T GetOrAddComponent<T>(this GameObject _gameObject) where T : Component
    {
        var component = _gameObject.GetComponent<T>();
        if (component == null)
            component = _gameObject.AddComponent<T>();

        return component;
    }
}