using UnityEngine;

public abstract class BaseMeleeAbility : MonoBehaviour
{
    protected Monster Owner { get; private set; }

    protected bool IsParameterInitialized { get; set; }
    public void BasicInitialize(Monster owner)
    {
        Owner = owner;
    }

    public bool IsInitialized()
    {
        if (Owner != null && IsParameterInitialized)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public abstract void Attack();
}
