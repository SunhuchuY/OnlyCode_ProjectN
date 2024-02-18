using Newtonsoft.Json;
using UnityEngine;

public class MeleeCommands
{
    public readonly MeleeAbility Ability;
    public readonly string AddDamage;
    public readonly string[] Parameters;

    [JsonConstructor]
    public MeleeCommands(MeleeAbility ability, string addDamage, string parameters)
    {
        Ability = ability;
        AddDamage = addDamage;

        if(!string.IsNullOrEmpty(parameters))
        {
            Parameters = parameters.Split(',');

            if(Parameters.Length == 0)
            {
                Parameters = new string[1] { parameters };
            }
        }
    }
}
