public enum SkillType
{
    Summon,
    Active
}

public enum SkillGrade
{
    R,
    SR,
    SSR
}

public class ActiveSkillData
{
    public int Id { get; set; }
    public SkillType Type { get; set; }
    public SkillGrade Grade { get; set; }
    public string Name { get; set; }
    public int Cost { get; set; }
    public string LevelPer { get; set; }
    public string Description { get; set; }
    public (float time, int index)[] ActionTimes { get; set; } = { (0f, 0) };
    public ActionData[] Actions { get; set; }
}