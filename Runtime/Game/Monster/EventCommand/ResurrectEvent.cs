using UnityEngine;

public class ResurrectEvent : MonoBehaviour
{
    private Monster Owner;

    void Awake()
    {
        Owner = GetComponent<Monster>();
    }

    void Start()
    {
        Owner.attributes.HP.OnCurrentValueIsZero += Resurrect;
    }

    void Resurrect()
    {
        Owner.attributes.HP.Initialize();
        Owner.Resets();
    }
}