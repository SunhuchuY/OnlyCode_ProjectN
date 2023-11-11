using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static UnityEngine.Rendering.DebugUI;

public class StatisticScreen : MonoBehaviour
{

    float slideDuration = 2f; 

    int requiredBloodStone = 1000; // 공격력 업글 필요골드
    int goldRange_Start = 0 , goldRange_End = 10;

    [SerializeField]
    Transform startPosition, EndPosition;

    [SerializeField]
    GameObject Panel;

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

        GameManager.playerScript.StatePlayer(PlayerStateEnum.Restart);
        GameManager.monsterControll.BigWaveSet(PlayerStateEnum.Restart);
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
        if(GameManager.uIManager.GetBloodStone() >= requiredBloodStone)
        {
            GameManager.uIManager.SetBloodStone(-1 * requiredBloodStone);

            if (GameManager.RandomRange(goldRange_Start, goldRange_End))
            {
                GameManager.uIManager.SetGold(100);
            }
        }
    }
}
