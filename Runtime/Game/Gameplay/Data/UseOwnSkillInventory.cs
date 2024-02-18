using UnityEngine;

public class UseOwnSkillInventory : MonoBehaviour
{
    public const int USEOWNSKILL_MAX_COUNT = 15;
    public const int NULL = -1;

    public int[] useOwnSkillID { get; private set; } = new int[USEOWNSKILL_MAX_COUNT];

    private void Awake()    
    {
        // temp: 임시로 모두 NULL 처리합니다.
        for (int i = 0; i < USEOWNSKILL_MAX_COUNT; i++)
        {
            useOwnSkillID[i] = -1;
        }
    }
}
