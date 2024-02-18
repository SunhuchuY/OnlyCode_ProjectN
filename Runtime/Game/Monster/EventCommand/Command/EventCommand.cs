using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.U2D;

public class EventCommand
{
    public readonly EventCommandType Type;
    public readonly string[] Parameters;

    [JsonConstructor]
    public EventCommand(EventCommandType type, string parameters)
    {
        Type = type;

        if (!string.IsNullOrEmpty(parameters))
        {
            Parameters = parameters.Split(',');
        }

    }
}
