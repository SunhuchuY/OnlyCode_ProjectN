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
    R,
    SR,
    SSR
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

