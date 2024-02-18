using UnityEngine;

public class StackAttackMonster : MonoBehaviour
{
    private Monster monster;
    private int curStack = 0;
    [SerializeField] private readonly int maxStack = 0;
    [SerializeField] [Range(0,100)] private readonly float addDamage_Percent = 0;

    private void Start()
    {
        monster = GetComponent<Monster>();  
    }

    public void StackAttack()
    {
        if(curStack >= maxStack)
        {
            curStack = 0;
            //monster.DealDamage(monster.attackMount * (addDamage_Percent / 100));
        }
    }
}
