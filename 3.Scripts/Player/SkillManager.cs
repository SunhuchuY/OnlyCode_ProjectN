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

    const int MaxNumIndex = 1000000;

    Vector3 avatarPosition = new Vector3(-4.55f, 7.95f, 0);
    const int skillMount = 5;
    const int skillSetMount = 4;
    const float btnMax_scale = 1f, btnMin_scale = 0.3f;
    const float nextCard_CoolTime = 2f;
    float nextCard_CurTime = 0;
    float angle;

    static public int[] dragTargetSkill_Num = {2 };
    public int pickSkill_Num = 0;
    public int pickBtn_Num = 0;

    [SerializeField] GameObject[] skillObj = new GameObject[skillMount];
    [SerializeField] int[] skillMana = new int[skillMount];
    [HideInInspector] public int[] skillLevel = new int[skillMount];
    public Transform[] skillsetButton = new Transform[skillSetMount];

    [SerializeField] GameObject targetWide_skill5 , targetWide_skill6 , targetWide_skill2, targetWide_skill17;
    [SerializeField] SpriteRenderer useSkillBound;

    Vector2 targetPosition;
    Vector2 dragOriginalPosition;
    Vector2 spawnPosition = Vector2.zero;

    // Next Card
    public Transform NextCard;
    public TMP_Text NextCard_Instantiate_lastTime_Text;
    // Next Card

    private bool isUse = false, isTurn = false;

    private async void Start()
    {
        for (int i = 0; i < skillLevel.Length; i++)
        {
            skillLevel[i] = 1;
        }

        await Task.Delay(2);

        for (int i = 0; i < GameManager.skillTreeManager.pickbutton_ObjList.Count; i++)
        {
            GameObject obj = GameManager.skillTreeManager.pickbutton_ObjList[i];
        }

        for (int i = 0; i < skillsetButton.Length; i++)
        {
            int randNum = RandSkillNum();

            skillsetButton[i].GetComponent<SkillButton>().pickSkillNum = randNum;
        }
    }

    int RandSkillNum()
    {
        List<int> randCard_List = new List<int>();

        for (int j = 0; j < GameManager.skillTreeManager.pickbutton_Parent.childCount; j++)
        {
            int temp = GameManager.skillTreeManager.pickbutton_Parent.GetChild(j).GetComponent<PickButtonScript>().skillpickNum;

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

    IEnumerator NextCard_Setting()
    {
        isTurn = true;

        NextCard_Instantiate_lastTime_Text.text = $"2초";
        yield return new WaitForSeconds(1f);
        NextCard_Instantiate_lastTime_Text.text = $"1초";
        yield return new WaitForSeconds(1f);
        NextCard_Instantiate_lastTime_Text.text = $"";


        int randNum = RandSkillNum();

        NextCard.GetComponent<SkillButton>().pickSkillNum = randNum;
        NextCard.GetComponent<Image>().sprite = GameManager.skillTreeManager.cardDatas[randNum].cardSprite;

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



        //if (IsInsideBounds(useSkillBound, tempVector))
            isUse = true;
    }

    public void EndDrag() 
    {
        Vector2 tempPosition = skillsetButton[pickBtn_Num].position;

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

        if (GameManager.playerScript.GetMana() < skillMana[pickSkill_Num])
            return;

        if (isUse == false)
            return;

        // Use Card
        GameManager.playerScript.SetMana(-1 * skillMana[pickSkill_Num]);
        skillsetButton[pickBtn_Num].GetComponent<SkillButton>().pickSkillNum = -1; // 초기화

        GameObject obj;
        spawnPosition = GameManager.player.transform.position;
        obj = Instantiate(skillObj[pickSkill_Num], spawnPosition, Quaternion.identity);
        GameManager.playerScript.StatePlayer(PlayerStateEnum.Skill);

        int level, attackAmount;
        float percent;
        Skill skill;
        Friend friend;

        switch (pickSkill_Num)
        {
            case 0:
                skill = obj.GetComponent<Skill>();
                level = GameManager.skillTreeManager.CurCardStates[pickSkill_Num].cardLevel;
                attackAmount = GameManager.skillTreeManager.cardDatas[pickSkill_Num].
                    AttackAmount(level);

                skill.SkillStart(attackAmount,pickSkill_Num ,4);
                break;
            case 1:
                skill = obj.GetComponent<Skill>();
                level = GameManager.skillTreeManager.CurCardStates[pickSkill_Num].cardLevel;

                skill.SkillStart(0, pickSkill_Num,0.1f);
                break;
            case 2:
                skill = obj.GetComponent<Skill>();
                level = GameManager.skillTreeManager.CurCardStates[pickSkill_Num].cardLevel;
                percent = GameManager.skillTreeManager.cardDatas[pickSkill_Num].
                    PercentAmount(level);
                attackAmount = (int)GameManager.playerScript.attack;

                Vector3 targetPosition = tempPosition;

                // 총알의 방향을 설정합니다.
                Vector2 bulletDirection = (targetPosition - GameManager.player.transform.position).normalized;

                // 각도 계산을 위해 방향 벡터를 각도로 변환합니다.
                float angle = Mathf.Atan2(bulletDirection.y, bulletDirection.x) * Mathf.Rad2Deg;

                // 총알의 회전을 설정합니다.
                obj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

                // Rigidbody2D 컴포넌트를 가져옵니다.
                Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();

                // 발사 방향으로 총알을 이동시킵니다.
                rb.velocity = bulletDirection * 5;


                skill.SkillStart((int)(attackAmount * percent), pickSkill_Num, 5f);
                break;
            case 3:
                friend = obj.GetComponent<Friend>();
                level = GameManager.skillTreeManager.CurCardStates[pickSkill_Num].cardLevel;
                percent = GameManager.skillTreeManager.cardDatas[pickSkill_Num].
                    PercentAmount(level);

                friend.attackAmount = (GameManager.playerScript.attack * (percent / 100));
                friend.maxHealth = (float)(GameManager.playerScript.GetMaxHealth() * 0.1);
                Debug.Log(friend.attackAmount);
                Debug.Log(percent);
                break;
        }
    }

    void RefillCard()
    {
        int randNum = RandSkillNum();

        skillsetButton[pickBtn_Num].GetComponent<SkillButton>().pickSkillNum = randNum;
    }

    bool IsInsideBounds(SpriteRenderer spriteRenderer, Vector2 point)
    {
        // Sprite의 bound를 가져옴
        Bounds spriteBounds = spriteRenderer.bounds;

        // point가 bound 안에 있는지 확인
        return spriteBounds.Contains(point);
    }
}
