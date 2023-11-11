using TMPro;
using UnityEngine;
using DG.Tweening;
using System.Collections;
using UnityEngine.UIElements;

public class CastleManager : MonoBehaviour
{
    [SerializeField] GameObject canvasCastle, canvasInGame, castleBackGround, canvasLoading, loadingManager;
    [SerializeField] TMP_Text goldText, soulFragmentText;
    [SerializeField] TMP_Text loadingText;
    [SerializeField] SpriteRenderer loadingBar;
    private int loadingPercent = 0;
    const float loadingMaxPercent = 100;




    private void FixedUpdate()
    {
        goldText.text = GameManager.uIManager.GetGold().ToString();
        soulFragmentText.text = GameManager.uIManager.GetSoulFragment().ToString();
    }
    public void Castle_In_Btn()
    {
        GameManager.monsterControll.gameObject.SetActive(false);
        canvasCastle.SetActive(true);
        canvasInGame.SetActive(false);
        castleBackGround.SetActive(true);
    }

    public void Castle_Exit_Btn()
    {
        GameManager.monsterControll.gameObject.SetActive(true);
        canvasCastle.SetActive(false);
        canvasInGame.SetActive(true);
        castleBackGround.SetActive(false);
    }

    public void GameFirst_Start()
    {
        StartCoroutine(Loading());
    }

    IEnumerator Loading()
    {
        float delay = 0.01f;

        canvasCastle.SetActive(false);
        castleBackGround.SetActive(false);
        loadingManager.SetActive(true);
        canvasLoading.SetActive(true);

        for (float i = 1; i <= loadingMaxPercent; i++ )
        {
            loadingText.text = $"{i}%";
            loadingBar.transform.DOScaleX(i/ loadingMaxPercent, delay);
            yield return new WaitForSeconds(delay);
        }

        canvasInGame.SetActive(true);
        loadingManager.SetActive(false);
        canvasLoading.SetActive(false);

        yield return new WaitForSeconds((delay * loadingMaxPercent));

        GameManager.monsterControll.gameObject.SetActive(true);

    }


}
