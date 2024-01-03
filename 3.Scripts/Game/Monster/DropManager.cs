using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DropManager : MonoBehaviour
{ // when monster died, Trigger On
  // 몬스터 죽었을때 나타나는 폭발 이펙트.
    const int dropMount = 1;
    const int Mount = 50;

    public Dictionary<DropAnim, int> enumToGameObjectMap = new Dictionary<DropAnim, int>()
    {
        { DropAnim.Basic,0 }
    };

    [SerializeField] GameObject emptyObject;
    [SerializeField] private GameObject[] gameObjects = new GameObject[dropMount];
    private List<GameObject> parents = new List<GameObject>();
        
    private void Start()
    {
        for (int k = 0; k < dropMount; k++)
        {

            GameObject _parent = Instantiate(emptyObject );
            parents.Add(_parent);
            _parent.transform.parent = transform;

            for (int i = 0; i < Mount; i++)
            {
                GameObject instant = Instantiate(gameObjects[k]);
                instant.transform.parent = _parent.transform;
                instant.SetActive(false);
            }
        }
    }

    public void DropObject(DropAnim dropType, Transform toPosition) 
     {
        for (int i = 0; i < Mount; i++)
        {
            GameObject temp = parents[enumToGameObjectMap[dropType]].transform.GetChild(i).gameObject;

            if (temp.activeSelf == false) 
            {
                temp.transform.position = toPosition.position;
                temp.SetActive(true);
                break;
            }
        }

     }
}

