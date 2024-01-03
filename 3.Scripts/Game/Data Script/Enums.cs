using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enums : MonoBehaviour
{
}


public enum monsterType
{
    Basic,
    Unique,
    FirstAttack
}

public enum ArrowType
{
    Basic,
    wideShot
}

public enum HealthTapEnum
{
    upMaxHealth,
    upCurHealth,
    NULL
}

public enum DefenseTapEnum
{
    defenseAmount,
    counterAttack,
    NULL
}

public enum AttackTapEnum
{
    attack,
    attackRange,
    attackSpeed,
    criticalPercent,
    CriticalAddDamagePercent,
    NULL
}

public enum RewardEnum
{
    gold,
    bloodstone,
    soulfragment
}

public enum cardRateEnum
{
    R = 0,
    SR = 1,
    SSR = 2
}

public enum TapEnum
{
    Attack,
    Defense
}

public enum PlayerStateEnum
{
    Dead,
    Attack,
    Skill,
    Restart,
    NULL


}

public enum appearTextEnum
{
    basic,
    critical

}

public enum BoxEnum
{
    gold,
    bloodStone,
    soulFragment,
    exp
}
public enum cardType
{
    spawner,
    attack,
    debuff,
    debiffSecond,
    attackPercent,
    spawnerPercent,
    shield,
    onlyPercent,
    targetNum,
    seconds,
    percentOfSecond,
    buff
}

public enum MultiplyOrPlus
{
    plus,
    multyply
}

public enum HealMonsterType
{
    hpPercent
}

public enum DropAnim
{
    Basic
}

[System.Serializable]
public enum type
{
    player,
    playerSKill,
    tower,
    sequencs
}