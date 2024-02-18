using UnityEngine;

public class TimeIntervalAttackSpeed : TimeIntervalFriend
{
    [SerializeField]
    float attackSpeedAmount = 0.2f;

    [SerializeField]
    float amount = 0.2f;

    protected override void Action()
    {

        foreach (Friend friend in friends)
        {
            friend.ChangeAttackSpeed(amount, duration);
        }
    }
}
