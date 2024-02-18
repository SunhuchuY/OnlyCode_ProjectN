using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public class StageParser : MonoBehaviour
{
    public static StageParser Instance;

    public const string BackGroundPath = "Stage/BackGround";
    public const string MonsterPrefabPath = "Monster";

    public Dictionary<int, WaveProfile> bigWaveProfiles { get; private set; }
    public Dictionary<int, MonsterProfile> monsterProfiles { get; private set; }

    private Dictionary<int, object> UniqueAbilityObject { get; set; }
    private Dictionary<int, SpecialsCommand> SpecialAbilityCommand { get; set; }
    private Dictionary<int, EventCommand> EventCommand { get; set; }

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;    
        }
        else
        {
            Destroy(gameObject);
        }

        WaveRead();
        MonsterRead();

        UniqueAbilityObject = new Dictionary<int, object>();
        SpecialAbilityCommand = new Dictionary<int, SpecialsCommand>();
        EventCommand = new Dictionary<int, EventCommand>();

#if UNITY_EDITOR
        Debug.Log("monsterProfiles를 성공적으로 불러왔습니다.");
        Debug.Log("bigWaveProfiles를 성공적으로 불러왔습니다.");
#endif
    }

    private void WaveRead()
    {
        ITableReader<Dictionary<int, WaveProfile>> reader = new WaveTableReader();
        bigWaveProfiles = reader.ReadTable(WaveTableReader.JsonPath);
    }

    private void MonsterRead()
    {
        ITableReader<Dictionary<int, MonsterProfile>> reader = new MonsterTableReader();
        monsterProfiles = reader.ReadTable(MonsterTableReader.JsonPath);
    }

    public object GetUniqueAbilityObject(int id, MonsterProfile profile)
    {
        if (UniqueAbilityObject.ContainsKey(id))
        {
            return UniqueAbilityObject[id];
        }
        else
        {
            if (profile.AttackType == 0)
            {
                MeleeCommands commands = JsonConvert.DeserializeObject<MeleeCommands>(profile.UniqueAbilityJsonText);
                UniqueAbilityObject.Add(id, commands);
                return commands;
            }
            else if (profile.AttackType == 1)
            {
                RangedCommand commands = JsonConvert.DeserializeObject<RangedCommand>(profile.UniqueAbilityJsonText);
                UniqueAbilityObject.Add(id, commands);
                return commands;
            }
            else
            {
                return null;
            }
        }
    }

    /// <summary>
    /// 존재하지 않는다면 NULL을 반환합니다.
    /// </summary>
    /// <returns></returns>
    public SpecialsCommand GetSpecialAbilityCommand(int id)
    {
        if (SpecialAbilityCommand.ContainsKey(id))
        {
            return SpecialAbilityCommand[id];
        }
        else
        {
            SpecialsCommand command = JsonConvert.DeserializeObject<SpecialsCommand>(monsterProfiles[id].SpecialAbilityJsonText);

            if(command.Ability == SpecialAbility.Null)
            {
                SpecialAbilityCommand.Add(id, null);
                return null;
            }
            else
            {
                SpecialAbilityCommand.Add(id, command);
                return command;
            }

        }
    }    

    /// <summary>
    /// 존재하지 않는다면 NULL을 반환합니다.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="profile"></param>
    /// <returns></returns>
    public EventCommand GetEventCommand(int id)
    {
        if (EventCommand.ContainsKey(id))
        {
            return EventCommand[id];    
        }
        else
        {
            EventCommand command = JsonConvert.DeserializeObject<EventCommand>(monsterProfiles[id].EventJsonText);

            if (command.Type == EventCommandType.Null)
            {
                EventCommand.Add(id, null);
                return null;
            }
            else
            {
                EventCommand.Add(id, command);
                return command;
            }
        }
    }
}

