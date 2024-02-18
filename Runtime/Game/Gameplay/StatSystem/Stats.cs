using System.Collections.Generic;
using UnityEngine.Assertions;

public class Stats : ICustomIndexer
{
    object ICustomIndexer.this[string _name] => this[_name];

    public Stat this[string _name] =>
        statDict[_name.ToLower()];

    private Dictionary<string, Stat> statDict = new();

    public Stats AddStat(string _name, Stat _stat)
    {
        string _lowerName = _name.ToLower();
        Assert.IsTrue(statDict.ContainsKey(_lowerName) == false);
        statDict.Add(_lowerName, _stat);
        return this;
    }
}