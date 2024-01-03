using UnityEngine;

public class Boss : MonoBehaviour
{
    private bool firstStart = true;
    private Monster monster;

    private void Start()
    {
        monster = GetComponent<Monster>();  
    }

    private void OnDisable()
    {
        if (firstStart == true)
            return;

        if (monster.Health.CurrentValue > 0)
            return;

        // GameManager.Instance.monsterControl.BossExit();
        
        firstStart = false;
    }


}

