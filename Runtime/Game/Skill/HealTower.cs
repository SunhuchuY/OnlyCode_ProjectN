using UnityEngine;

public class HealTower : MonoBehaviour
{
    [SerializeField] Friend friend;

    [SerializeField] float coolTime = 1f;
    float curTime;

    void OnEnable()
    {
        curTime = coolTime;
    }

    void FixedUpdate()
    {
        curTime += Time.fixedTime;

        if (coolTime > curTime)
            return;

        Heal();
    }

    void Heal()
    {
        GameManager.Instance.playerScript.Stats["Hp"].ApplyModifier(new StatModifier() { Magnitude = friend.attackAmount });
    }
}