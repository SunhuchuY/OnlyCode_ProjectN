using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public GameObject[] particleObjects;
    Vector2 adjustVector = new Vector2(0.5f, 0.5f);

    [SerializeField] GameObject emptyObject;
    List<GameObject> emptyObject_List = new List<GameObject>();

    const int childCount = 5;

    private void Start()
    {
        for (int i = 0; i < particleObjects.Length; i++)
        {
            emptyObject_List.Add(Instantiate(emptyObject, transform));

            for (int j = 0; j < childCount; j++)
            {
                GameObject obj = Instantiate(particleObjects[i], emptyObject_List[i].transform);
                obj.SetActive(false);
            }
        }
    }







    private Transform particleTransform;
    public Transform GetparticleTransform() {  return particleTransform; }

    public void PlayParticle(Transform enemtTransform, Transform rotationTransform ,int index, float offTime = 3f)
    {
        if (index >= particleObjects.Length)
            return;

        for (int i = 0; i < childCount; i++)
        {
            if (emptyObject_List[index].transform.GetChild(i).gameObject.activeSelf == false)
            {
                emptyObject_List[index].transform.GetChild(i).gameObject.gameObject.SetActive(true);
                emptyObject_List[index].transform.GetChild(i).transform.rotation = rotationTransform.rotation;
                emptyObject_List[index].transform.GetChild(i).gameObject.transform.position = enemtTransform.position;
                emptyObject_List[index].transform.GetChild(i).gameObject.GetComponent<ParticleSystem>().Play();
                StartCoroutine(OffParticle(emptyObject_List[index].transform.GetChild(i).gameObject, offTime));
                particleTransform = emptyObject_List[index].transform.GetChild(i);
                break;
            }
        }
    }

    public void PlayParticle(Vector3 enemtTransform, Transform rotationTransform, int index, float offTime = 3f)
    {
        if (index >= particleObjects.Length)
            return;

        for (int i = 0; i < childCount; i++)
        {
            if (emptyObject_List[index].transform.GetChild(i).gameObject.activeSelf == false)
            {
                emptyObject_List[index].transform.GetChild(i).gameObject.gameObject.SetActive(true);
                emptyObject_List[index].transform.GetChild(i).transform.rotation = rotationTransform.rotation;
                emptyObject_List[index].transform.GetChild(i).gameObject.transform.position = enemtTransform;
                emptyObject_List[index].transform.GetChild(i).gameObject.GetComponent<ParticleSystem>().Play();
                StartCoroutine(OffParticle(emptyObject_List[index].transform.GetChild(i).gameObject, offTime));
                particleTransform = emptyObject_List[index].transform.GetChild(i);
                break;
            }
        }
    }



    IEnumerator OffParticle(GameObject obj, float _offTime =3f)
    {
        yield return new WaitForSeconds(_offTime);
        obj.SetActive(false);
    }
}
