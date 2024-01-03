using DG.Tweening;
using UnityEngine;

public class mon8Script : MonoBehaviour
{
    public int stackCount = 0;
    public GameObject trailRendererPrefeb;

    private TrailRenderer trailRenderer;
    private GameObject emptyObj;
    private Monster monster;

    readonly private float moveDistance_Interval = 1f;
    readonly private float moveDuration = 1f;

    private void Start()
    {
        monster = GetComponent<Monster>();
        trailRenderer = GetComponent<TrailRenderer>();

        trailRenderer.sortingLayerName = "Background";
        trailRenderer.sortingOrder = 1000;

        emptyObj = Instantiate(trailRendererPrefeb);
        
    }

    private void Update()
    {
        emptyObj.transform.position = transform.position;
    }

    void StackAttack() 
    {

        if(transform.position.x < monster.targetObj.transform.position.x && transform.position.y > monster.targetObj.transform.position.y) // 왼쪽 상단
        {
            Vector3 move = new Vector3(moveDistance_Interval, -1 *  moveDistance_Interval, 0);
            transform.DOMove(transform.position + move, moveDuration);

            stackCount++;
        }
        else if (transform.position.x > monster.targetObj.transform.position.x && transform.position.y > monster.targetObj.transform.position.y) // 오른쪽 상단
        {
            Vector3 move = new Vector3(-1 * moveDistance_Interval, -1 * moveDistance_Interval, 0);
            transform.DOMove(transform.position + move, moveDuration);

            stackCount++;
        }
        else if (transform.position.x > monster.targetObj.transform.position.x && transform.position.y < monster.targetObj.transform.position.y) // 오른쪽 하단
        {
            Vector3 move = new Vector3(-1 * moveDistance_Interval, moveDistance_Interval, 0);
            transform.DOMove(transform.position + move, moveDuration);

            stackCount++;
        }
        else if (transform.position.x < monster.targetObj.transform.position.x && transform.position.y < monster.targetObj.transform.position.y) // 왼쪽 하단
        {
            Vector3 move = new Vector3(moveDistance_Interval,moveDistance_Interval, 0);
            transform.DOMove(transform.position + move, moveDuration);
            
            stackCount++;
        }

        if(stackCount >= 3)
        {
            Debug.Log("Stack Comobo");
            stackCount = 0;
        }

    }


}
