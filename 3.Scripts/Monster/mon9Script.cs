using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mon9Script : MonoBehaviour
{
    [SerializeField] GameObject InstantiateEachMonster_Prefeb;

    Vector3 addPosition = new Vector3(-3f, -3f, 0 );

    private void InstantiateEachMonster() // animation event funtion
    {
        for (int i = 0; i < 1; i++)
        {
            Instantiate(InstantiateEachMonster_Prefeb , transform.position + addPosition, Quaternion.identity);
        }


    }
}
