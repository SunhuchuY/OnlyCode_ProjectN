using Newtonsoft.Json;

public class SpecialsCommand
{
    public readonly SpecialAbility Ability;
    public readonly string[] Parameters;

    [JsonConstructor]
    public SpecialsCommand(SpecialAbility ability, string parameters)
    {
        Ability = ability;
        
        if (!string.IsNullOrEmpty(parameters))
        {
            Parameters = parameters.Split(',');
        }
    }
}