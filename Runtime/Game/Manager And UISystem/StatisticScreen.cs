using System.Collections;
using UnityEngine;
using DG.Tweening;

public class StatisticScreen : MonoBehaviour
{
    private float slideDuration = 2f; 
    
    private int requiredBloodStone = 1000; // 공격력 업글 필요골드
    private int goldRange_Start = 0 , goldRange_End = 10;

    [SerializeField] private Transform startPosition, EndPosition;

    [SerializeField] private GameObject Panel;

    private void Start()
    {
        Panel.transform.position = startPosition.position;
    }

    public void SlidePanel()
    {
        Panel.SetActive(true);  
        Panel.transform.DOLocalMoveY(EndPosition.localPosition.y, slideDuration);
    }

    public void ExitPanel() // button
    {
        Panel.transform.DOLocalMoveY(EndPosition.localPosition.y, slideDuration);
        StartCoroutine(ExitOff());
    }

    IEnumerator ExitOff()
    {
        yield return new WaitForSeconds(slideDuration);
        Panel.SetActive(false);
    }

    public void ChangeGold() // button
    {
        
    }
}
