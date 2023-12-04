using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static UnityEngine.Rendering.DebugUI;

public class StatisticScreen : MonoBehaviour
{
    private float slideDuration = 2f; 
    
    private int requiredBloodStone = 1000; // ���ݷ� ���� �ʿ���
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

    // if player is dead, return big stage 
    public void RestartBigStage()
    {
        Panel.gameObject.SetActive(false);

        GameManager.Instance.playerScript.StatePlayer(PlayerStateEnum.Restart);
        GameManager.Instance.monsterControll.BigWaveSet(PlayerStateEnum.Restart);
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
        if(GameManager.Instance.uIManager.GetBloodStone() >= requiredBloodStone)
        {
            GameManager.Instance.uIManager.SetBloodStone(-1 * requiredBloodStone);

            if (GameManager.RandomRange(goldRange_Start, goldRange_End))
            {
                GameManager.Instance.uIManager.SetGold(100);
            }
        }
    }
}