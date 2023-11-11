using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;
using TMPro;

public class CastleIn : MonoBehaviour
{
    [SerializeField] GameObject goldShop_Tap,skillTree_Tap;
    [SerializeField ] Transform startPo, endPo;
    [SerializeField] TMP_Text reqSpeed_Text, reqAttack_Text, reqRange_Text, reqCounter_Text, reqMaxhp_Text;

    [SerializeField] TMP_Text Speed_Text, Attack_Text, Range_Text, Counter_Text, Maxhp_Text;
    int reqGold_Speed = 30, reqGold_Attack = 30, reqGold_Range = 30, reqGold_Counter = 30, reqGold_Maxhp = 30;
    float upAmount_Speed = 0.1f, upAmount_Attack = 20, upAmount_Range = 1, upAmount_Counter = 20, upAmount_Maxhp = 100;
    private const float moveDuration = 0.3f;

    private void Start()
    {
        AllTextUpdate();
    }

    public void GoldShop_Btn()
    {
        goldShop_Tap.transform.position = startPo.position;
        goldShop_Tap.SetActive(true);

        goldShop_Tap.transform.DOMoveY(endPo.position.y, moveDuration);
    }

    void AllTextUpdate()
    {
        ReqText_Update();
        NumText_Update();
    }

    void ReqText_Update()
    {
        reqSpeed_Text.text = $"{reqGold_Speed}개가 필요";
        reqAttack_Text.text = $"{reqGold_Attack}개가 필요";
        reqRange_Text.text = $"{reqGold_Range}개가 필요";
        reqCounter_Text.text = $"{reqGold_Counter}개가 필요";
        reqMaxhp_Text.text = $"{reqGold_Maxhp}개가 필요";
    }

    void NumText_Update()
    {
        Speed_Text.text = $"현재 : {GameManager.playerScript.per_speed}";
        Attack_Text.text = $"현재 : {GameManager.playerScript.per_attack}";
        Range_Text.text = $"현재 : {GameManager.playerScript.per_range}";
        Counter_Text.text = $"현재 : {GameManager.playerScript.per_counter}";
        Maxhp_Text.text = $"현재 : {GameManager.playerScript.per_maxhp}";
    }

    public void upAttack_btn()
    {
        if (GameManager.uIManager.GetGold() >= reqGold_Attack)
        {
            GameManager.playerScript.per_attack += upAmount_Attack;
            GameManager.uIManager.SetGold(-1 * reqGold_Attack);
            NumText_Update();
        }
    }

    public void upRange_btn()
    {
        if (GameManager.uIManager.GetGold() >= reqGold_Range)
        {
            GameManager.playerScript.per_range += upAmount_Range;
            GameManager.uIManager.SetGold(-1 * reqGold_Range);
            NumText_Update();
        }
    }

    public void upCounter_btn()
    {
        if (GameManager.uIManager.GetGold() >= reqGold_Counter)
        {
            GameManager.playerScript.per_counter += upAmount_Counter;
            GameManager.uIManager.SetGold(-1 * reqGold_Counter);
            NumText_Update();
        }
    }

    public void upSpeed_btn()
    {
        if (GameManager.uIManager.GetGold() >= reqGold_Speed)
        {
            GameManager.playerScript.per_speed += upAmount_Speed;
            GameManager.uIManager.SetGold(-1 * reqGold_Speed);
            NumText_Update();
        }
    }
    public void upMaxhp_btn()
    {
        if (GameManager.uIManager.GetGold() >= reqGold_Maxhp)
        {
            GameManager.playerScript.per_counter += upAmount_Maxhp;
            GameManager.uIManager.SetGold(-1 * reqGold_Maxhp);
            NumText_Update();
        }
    }

    public void GoldShop_Exit()
    {
        AllTextUpdate();
        goldShop_Tap.SetActive(false);

    }

    public void SkillTree_In()
    {
        skillTree_Tap.SetActive(true);
    }

    public void SkillTree_Exit()
    {
        skillTree_Tap.SetActive(false);

    }

}
