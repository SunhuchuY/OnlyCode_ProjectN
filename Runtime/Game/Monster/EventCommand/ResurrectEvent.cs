using UniRx;
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
        Owner.Stats["Hp"].OnChangesCurrentValueIntAsObservable
            .Where(x => x <= 0)
            .Subscribe(_ => Resurrect());
    }

    void Resurrect()
    {
        Owner.Stats["Hp"].Initialize();
        Owner.Resets();
    }
}