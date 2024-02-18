using System.Numerics;

public struct PlayerLevelData
{
    public readonly int LevelUpMultiple;
    public readonly BigInteger Experience;

    public PlayerLevelData(int levelUpMultiple, BigInteger experience)
    {
        LevelUpMultiple = levelUpMultiple;
        Experience = experience;
    }
}

public struct PlayerSkillLevelData
{
    public readonly int NeedOfSkillFragments;
    public readonly int NeedOfGold;

    public PlayerSkillLevelData(int needOfSkillFragments, int needOfGold)
    {
        NeedOfSkillFragments = needOfSkillFragments;
        NeedOfGold = needOfGold;
    }
}
