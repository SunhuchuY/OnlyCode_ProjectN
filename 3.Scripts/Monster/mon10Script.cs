using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using System.Collections;

public class mon10Script : MonoBehaviour
{
    [SerializeField] mon10ScriptAssist mon10ScriptAssists;
    const float effectDuration = 1f;

    void AlternativeFuntionHeal()
    {
        mon10ScriptAssists.healRange();
    }
}
