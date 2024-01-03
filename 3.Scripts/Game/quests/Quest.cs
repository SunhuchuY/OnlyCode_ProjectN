using UnityEngine;

public class Quest : MonoBehaviour
{
    [SerializeField] private GameObject completedObject;
    [SerializeField] private GameObject notcompletedObject;

    public void Completed()
    {
        completedObject.SetActive(true);
        notcompletedObject.SetActive(false);
    }

    public void NotCompleted()
    {
        completedObject.SetActive(false);
        notcompletedObject.SetActive(true);
    }
}
