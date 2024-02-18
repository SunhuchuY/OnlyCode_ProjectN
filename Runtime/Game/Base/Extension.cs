using System.Collections.Generic;

public static class Extension
{
    public static T Random<T>(this IList<T> _list)
    {
        if (_list.Count == 0)
            return default;
        return _list[UnityEngine.Random.Range(0, _list.Count)];
    }
}