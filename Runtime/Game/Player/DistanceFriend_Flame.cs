using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceFriend_Flame : distanceFriend
{
    [SerializeField] protected GameObject emptyChildObj;

    void OnEnable()
    {
        if (emptyChildObj != null)
            emptyChildObj.SetActive(false);
    }

    void Update()
    {
        if (transform.parent.GetComponent<Friend>().searchObj != null)
            FlamethroweShoot();
        else
            FlamethroweOff();
    }


    private void FlamethroweShoot()
    {
        emptyChildObj.SetActive(true);

        Vector2 FlameDirection = (transform.parent.GetComponent<Friend>().searchObj.transform.position - transform.position).normalized;

        float angle = Mathf.Atan2(FlameDirection.y, FlameDirection.x) * Mathf.Rad2Deg;

        emptyChildObj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
    }
    
    private void FlamethroweOff()
    {
        emptyChildObj.SetActive(false);
    }

}
