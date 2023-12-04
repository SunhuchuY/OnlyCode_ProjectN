using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class SkillManager : MonoBehaviour
{
    readonly private int MaxNumIndex = 1000000;
    static public int[] dragTargetSkill_Num = {2 };

    const int skillMount = 5;
    const int skillSetMount = 4;
    const float btnMax_scale = 1f, btnMin_scale = 0.3f;

    public int pickSkill_Num = 0;
    public int pickBtn_Num = 0;

    [SerializeField] private GameObject[] skillObj = new GameObject[skillMount];
    [HideInInspector] public int[] skillLevel = new int[skillMount];

    [SerializeField] private GameObject targetWide_skill5 , targetWide_skill6 , targetWide_skill2, targetWide_skill17;
    [SerializeField] private SpriteRenderer useSkillBound;

    public Transform[] skillsetButton = new Transform[skillSetMount];
    
    private Vector2 targetPosition;
    private Vector2 dragOriginalPosition;
    private Vector2 spawnPosition = Vector2.zero;

    // Next Card
    public Transform NextCard;
    public TMP_Text NextCard_Instantiate_lastTime_Text;
    // Next Card

    private bool isUse = false;
    private bool isTurn = false;
    private float angle;

    private async void Start()
    {
        for (int i = 0; i < skillLevel.Length; i++)
        {
            skillLevel[i] = 1;
        }

        await Task.Delay(2);

        for (int i = 0; i < GameManager.Instance.skillTreeManager.pickbutton_ObjList.Count; i++)
        {
            GameObject obj = GameManager.Instance.skillTreeManager.pickbutton_ObjList[i];
        }

        for (int i = 0; i < skillsetButton.Length; i++)
        {
            int randNum = RandSkillNum();

            skillsetButton[i].GetComponent<SkillButton>().pickSkillNum = randNum;
        }
    }

    private int RandSkillNum()
    {
        List<int> randCard_List = new List<int>();

        for (int j = 0; j < GameManager.Instance.skillTreeManager.pickbutton_Parent.childCount; j++)
        {
            int temp = GameManager.Instance.skillTreeManager.pickbutton_Parent.GetChild(j).GetComponent<PickButtonScript>().skillpickNum;

            if (temp < 0)
                continue;

            randCard_List.Add(temp);
        }


        return randCard_List
                [Random.Range(0, randCard_List.Count)];
    }

    private void Update()
    {
        if(NextCard.GetComponent<SkillButton>().pickSkillNum < 0 && !isTurn)
        {
            StartCoroutine(NextCard_Setting());
        }

        for (int i = 0; i < skillsetButton.Length; i++)
        {
            if (skillsetButton[i].GetComponent<SkillButton>().pickSkillNum >= 0)
                continue;

            if (NextCard.GetComponent<SkillButton>().pickSkillNum < 0)
                continue;

            skillsetButton[i].GetComponent<SkillButton>().pickSkillNum = NextCard.GetComponent<SkillButton>().pickSkillNum;
            NextCard.GetComponent<SkillButton>().pickSkillNum = -1;

            isTurn = false;
        }
    }

    private IEnumerator NextCard_Setting()
    {
        isTurn = true;

        NextCard_Instantiate_lastTime_Text.text = $"2초";
        yield return new WaitForSeconds(1f);
        NextCard_Instantiate_lastTime_Text.text = $"1초";
        yield return new WaitForSeconds(1f);
        NextCard_Instantiate_lastTime_Text.text = $"";


        int randNum = RandSkillNum();

        NextCard.GetComponent<SkillButton>().pickSkillNum = randNum;
        NextCard.GetComponent<Image>().sprite = GameManager.Instance.skillTreeManager.cardDatas[randNum].cardSprite;

        isTurn = false;
    }

    public void BeginDrag()
    {
        if (pickSkill_Num >= 0)
        {
            skillsetButton[pickBtn_Num].transform.localScale = new Vector2
            (btnMin_scale, btnMin_scale);
        }

        isUse = false;
        dragOriginalPosition = skillsetButton[pickBtn_Num].position;
    }

    public void OnDrag(BaseEventData data)
    {
        if (pickSkill_Num < 0)
            return;

        PointerEventData pointer_data = (PointerEventData)data;
        Vector2 tempVector = new Vector2(Camera.main.ScreenToWorldPoint(pointer_data.position).x, Camera.main.ScreenToWorldPoint(pointer_data.position).y);
        skillsetButton[pickBtn_Num].position = tempVector;

        isUse = true;

        Vector3 temp;
        switch (pickSkill_Num)
        {
            case 5:
                targetWide_skill5.SetActive(true);

                skillsetButton[pickBtn_Num].GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
                temp = Camera.main.ScreenToWorldPoint(skillsetButton[pickBtn_Num].GetComponent<RectTransform>().position);

                // 총알의 방향을 설정합니다.
                Vector2 wideDirection =
                    (temp - GameManager.Instance.player.transform.position).normalized;

                // 각도 계산을 위해 방향 벡터를 각도로 변환합니다.
                angle = Mathf.Atan2(wideDirection.y, wideDirection.x) * Mathf.Rad2Deg;

                // 총알의 회전을 설정합니다.
                targetWide_skill5.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
                break;

            case 12:
                skillsetButton[pickBtn_Num].gameObject.SetActive(false);
                targetWide_skill2.gameObject.SetActive(true);
                break;
        }
    }

    public void EndDrag() 
    {
        Vector2 tempPosition = skillsetButton[pickBtn_Num].position;
        int mana = GameManager.Instance.skillTreeManager.cardDatas[pickSkill_Num].GetCost();

        targetWide_skill2.SetActive(false);
        targetWide_skill5.SetActive(false);
        targetWide_skill6.SetActive(false);
        targetWide_skill17.SetActive(false);

        skillsetButton[pickBtn_Num].GetComponent<Image>().color = new Color(1, 1, 1, 1);
        skillsetButton[pickBtn_Num].transform.localScale = new Vector2
            (btnMax_scale, btnMax_scale);
        skillsetButton[pickBtn_Num].position = dragOriginalPosition;

        if (pickSkill_Num < 0)
            return;

        if (GameManager.Instance.playerScript.GetMana() < mana)
            return;

        if (isUse == false)
            return;

        // Use Card
        GameManager.Instance.playerScript.SetMana(-1 * mana);
        skillsetButton[pickBtn_Num].GetComponent<SkillButton>().pickSkillNum = -1; // 초기화

        spawnPosition = GameManager.Instance.player.transform.position;
        GameObject obj = Instantiate(skillObj[pickSkill_Num], spawnPosition, Quaternion.identity);
        GameManager.Instance.playerScript.StatePlayer(PlayerStateEnum.Skill);

        int level = GameManager.Instance.skillTreeManager.CurCardStates[pickSkill_Num].cardLevel;
        int attackAmount = GameManager.Instance.skillTreeManager.cardDatas[pickSkill_Num].
                    AttackAmount(level);
        float percent = GameManager.Instance.skillTreeManager.cardDatas[pickSkill_Num].
                    PercentAmount(level);
        float percent_two = GameManager.Instance.skillTreeManager.cardDatas[pickSkill_Num].
                    PercentAmount_Two(level);
        float seconds = GameManager.Instance.skillTreeManager.cardDatas[pickSkill_Num].
                    SecondsAmount(level);


        Skill skill = obj.GetComponent<Skill>();
        Friend friend;
        Vector3 buttonPosition = skillsetButton[pickBtn_Num].transform.position;

        switch (pickSkill_Num)
        {
            case 0:
                skill.SkillStartAsync(attackAmount,pickSkill_Num ,4);
                break;
            case 1:
                skill.SkillStartAsync(0, pickSkill_Num, 2f);
                break;
            case 2:
                Vector3 targetPosition = tempPosition;

                // 총알의 방향을 설정합니다.
                Vector2 bulletDirection = (targetPosition - GameManager.Instance.player.transform.position).normalized;

                // 각도 계산을 위해 방향 벡터를 각도로 변환합니다.
                float angle = Mathf.Atan2(bulletDirection.y, bulletDirection.x) * Mathf.Rad2Deg;

                // 총알의 회전을 설정합니다.
                obj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

                // Rigidbody2D 컴포넌트를 가져옵니다.
                Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();

                // 발사 방향으로 총알을 이동시킵니다.
                rb.velocity = bulletDirection * 5;


                skill.SkillStartAsync((int)(attackAmount * percent), pickSkill_Num, 5f);
                break;
            case 3:
                obj.transform.position = buttonPosition;
                friend = obj.GetComponent<Friend>();
                percent = GameManager.Instance.skillTreeManager.cardDatas[pickSkill_Num].
                    PercentAmount(level);

                friend.attackAmount = (GameManager.Instance.playerScript.attack * (percent / 100));
                friend.maxHealth = (float)(GameManager.Instance.playerScript.GetMaxHealth() * 0.1);
                break;


            case 4:
                obj.GetComponent<Shield>().shieldAmount = GameManager.Instance.skillTreeManager.cardDatas[pickSkill_Num].
                    ShieldAmount(level); ;

                skill.SkillStartAsync(0,pickSkill_Num,10);
                break;

            case 5:
                obj.transform.rotation = targetWide_skill5.transform.rotation;
                GameManager.Instance.particleManager.PlayParticle(targetWide_skill5.transform, targetWide_skill5.transform, 4);
                skill.SkillStartAsync(attackAmount, pickSkill_Num, 10);
                break;

            case 6:
                obj.transform.position = targetWide_skill6.transform.position;
                GameManager.Instance.particleManager.PlayParticle(targetWide_skill6.transform, targetWide_skill6.transform, 5, 2);
                skill.SkillStartAsync(attackAmount, pickSkill_Num, 0.1f);
                break;

            case 7:
                skill.SkillStartAsync(attackAmount, pickSkill_Num, 4);
                break;
            case 9:
                GameManager.Instance.playerScript.Temporary_invinicibleTurnOn(seconds);
                skill.SkillStartAsync(attackAmount, pickSkill_Num, seconds);
                break;
            case 10:
                GameManager.Instance.playerScript.SetMana(+1);
                skill.SkillStartAsync(attackAmount, pickSkill_Num, 0.1f);
                break;
            case 11:
                obj.GetComponent<Bullet>().exptionAttackAmount = 
                    (int)(GameManager.Instance.playerScript.attack * percent);

                for (int i = 0; i < 2; i++)
                    Instantiate(obj);
                break;
            case 12:    
                skill.SkillStartAsync( (int)(GameManager.Instance.playerScript.attack * percent), pickSkill_Num, 0.1f);
                break;
            case 13:
                obj.GetComponent<Friend>().attackAmount = GameManager.Instance.playerScript.attack * (float)(percent / 100);
                obj.GetComponent<Friend>().maxHealth = (float)GameManager.Instance.playerScript.GetMaxHealth() * (float)(percent_two / 100);
                obj.GetComponent<Friend>().currentHealth = obj.GetComponent<Friend>().maxHealth;
                break;
            case 14:
                obj.GetComponent<Friend>().attackAmount = 0;
                obj.GetComponent<Friend>().maxHealth = (float)GameManager.Instance.playerScript.GetMaxHealth() * (float)(percent/ 100);
                obj.GetComponent<Friend>().currentHealth = obj.GetComponent<Friend>().maxHealth;
                break;

        }
    }
}
