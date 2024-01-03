using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    [SerializeField] private PlayerSkillsCanUseUI playerSkillsCanUseUI;
    [SerializeField] private Skill[] skillPrefabs = new Skill[21];
    public event System.Action OnSkillsCanUseNumsChanged;
    public event System.Action OnNextSkillChanged;
    public IReadOnlyList<int?> SkillSlotIds => skillSlotIds;
    public int? NextSkillId { get; private set; }
    private List<int?> skillSlotIds = new List<int?>(4);

    public float NextSkillOfCurTime { get; private set; }
    public bool isNextCardReFilling = false;

    // Start 함수인데, Nullreferece 에러를 피하기 위함.
    public void CustomStart()
    {
        skillSlotIds = new List<int?>()
        {
            RandCanUseSkillID(), RandCanUseSkillID(), RandCanUseSkillID(), RandCanUseSkillID()
        };
        NextSkillId = RandCanUseSkillID();

        OnSkillsCanUseNumsChanged?.Invoke();
        OnNextSkillChanged?.Invoke();
    }

    private void Update()
    {
        // 다음 카드대기에 스킬이 비어있는가
        if (IsEmptyNextSKill() && !isNextCardReFilling)
        {
            OnNextSkillChanged?.Invoke();
            isNextCardReFilling = true;
            playerSkillsCanUseUI.NextCardCountUniTask();
        }

        // 슬롯에 스킬이 비어있는가
        for (int i = 0; i < 4; i++)
        {
            if (!IsEmptySlotSkill(i))
                continue;

            if (IsEmptyNextSKill())
                continue;

            // 스킬 채우기
            skillSlotIds[i] = NextSkillId;
            NextSkillId = null;

            OnSkillsCanUseNumsChanged?.Invoke();
        }
    }

    private int RandCanUseSkillID()
    {
        var canUseCardIDLIst = GameManager.Instance.skillTreeManager.CanUseCardIDList();
        int randCardID = canUseCardIDLIst[Random.Range(0, canUseCardIDLIst.Count)];

        return randCardID;
    }

    public void ReFillNextSkill()
    {
        int ID = RandCanUseSkillID();
        NextSkillId = ID;

        OnNextSkillChanged?.Invoke();
    }


    public void ReFillSlotSkill(int _slotIndex, int _skillID)
    {
        skillSlotIds[_slotIndex] = _skillID;

        OnSkillsCanUseNumsChanged?.Invoke();
    }

    public bool IsEmptySlotSkill(int _slotIndex)
    {
        if (skillSlotIds[_slotIndex] == null)
            return true;
        else
            return false;
    }

    public bool IsEmptyNextSKill()
    {
        if (NextSkillId == null)
            return true;
        else
            return false;
    }

    public void UseSkill(int _slotIndex, Vector3 _targetPosition)
    {
        int _id = skillSlotIds[_slotIndex].Value;
        var _cardData = GameManager.Instance.skillTreeManager.CardDataContainer.cards[_id];


        // 마나 부족함
        if (!GameManager.Instance.playerScript.IsCanUseMana(_cardData.cardCost))
        {
            GameManager.Instance.commonUI.ToastMessage("마나가 부족합니다.");
            return;
        }


        skillSlotIds[_slotIndex] = null;
        GameManager.Instance.playerScript.Mana.ApplyModifier(-_cardData.cardCost);
        Vector3 spawnPosition = GameManager.Instance.player.transform.position;
        Skill skill = Instantiate(skillPrefabs[_id], spawnPosition, Quaternion.identity);

        int level = GameManager.Instance.skillTreeManager.CurCardStates[_id].cardLevel;
        int attackAmount = (int)GameManager.Instance.skillTreeManager.CardDataContainer.cards[_id].AttackAmount(level);
        float percent = GameManager.Instance.skillTreeManager.CardDataContainer.cards[_id].PercentAmount(level);
        float percent_two = GameManager.Instance.skillTreeManager.CardDataContainer.cards[_id].PercentAmount_Two(level);
        float duration = GameManager.Instance.skillTreeManager.CardDataContainer.cards[_id].duration;
        float seconds = GameManager.Instance.skillTreeManager.CardDataContainer.cards[_id].SecondsAmount(level);

        float angle;
        Vector2 angleDirection;
        Rigidbody2D? rb = skill.GetComponent<Rigidbody2D>();
        Friend? friend = skill.GetComponent<Friend>();

        OnSkillsCanUseNumsChanged?.Invoke();

        // Vector3 buttonPosition = skillsetButton[pickBtn_Num].transform.position;
        switch (_id)
        {
            case 0:
                skill.SkillInitialize(attackAmount, _id, duration);
                break;

            case 1:
                skill.SkillInitialize(0, _id, duration);
                break;

            case 2:
                Debug.Log(_targetPosition);

                angleDirection = (_targetPosition - GameManager.Instance.player.transform.position).normalized;
                angle = Mathf.Atan2(angleDirection.y, angleDirection.x) * Mathf.Rad2Deg;
                skill.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
                rb.velocity = angleDirection * 5;


                skill.SkillInitialize((int)(GameManager.Instance.playerScript.Attack.CurrentValue * (percent / 100)),
                    _id, duration);
                break;

            case 3:
                skill.transform.position = _targetPosition;
                friend = skill.GetComponent<Friend>();
                percent = GameManager.Instance.skillTreeManager.CardDataContainer.cards[_id].PercentAmount(level);

                friend.attackAmount = (GameManager.Instance.playerScript.Attack.CurrentValue * (percent / 100));
                friend.maxHealth = (float)(GameManager.Instance.playerScript.Health.CurrentValue * 0.1);

                skill.SkillInitialize((int)(GameManager.Instance.playerScript.Attack.CurrentValue * (percent / 100)),
                    _id, duration);
                break;


            case 4:
                skill.GetComponent<Shield>().shieldAmount =
                    GameManager.Instance.skillTreeManager.CardDataContainer.cards[_id].ShieldAmount(level);

                skill.SkillInitialize(0, _id, duration);
                break;

            case 5:
                angleDirection = (_targetPosition - GameManager.Instance.player.transform.position).normalized;
                angle = Mathf.Atan2(angleDirection.y, angleDirection.x) * Mathf.Rad2Deg;
                skill.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
                // GameManager.Instance.objectPoolManager.PlayParticle("Prefab/Particle/5", targetWide_skill5.transform, targetWide_skill5.transform, 4);
                skill.SkillInitialize((int)(GameManager.Instance.playerScript.Attack.CurrentValue * (percent / 100)),
                    _id, duration);
                break;

            case 6:
                skill.transform.position = _targetPosition;
                skill.SkillInitialize(attackAmount, _id, 0.1f);
                break;

            case 7:
                skill.transform.position = _targetPosition;
                skill.SkillInitialize(attackAmount, _id, 4);
                break;

            case 8:
                skill.transform.position = _targetPosition;
                skill.SkillInitialize(0, _id, duration);
                break;

            case 9:
                GameManager.Instance.playerScript.Temporary_invinicibleTurnOn(duration);
                skill.SkillInitialize(attackAmount, _id, seconds);
                break;

            case 10:
                GameManager.Instance.playerScript.ManaReFill(+1);
                skill.SkillInitialize(attackAmount, _id, 0.1f);
                break;

            case 11:
                skill.GetComponent<Bullet>().exptionAttackAmount =
                    (int)(GameManager.Instance.playerScript.Attack.CurrentValue * percent);

                angleDirection = (_targetPosition - GameManager.Instance.player.transform.position).normalized;
                angle = Mathf.Atan2(angleDirection.y, angleDirection.x) * Mathf.Rad2Deg;
                skill.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
                rb.velocity = angleDirection * 5;

                for (int i = 0; i < 2; i++)
                {
                    var temp = Instantiate(skill);
                    temp.GetComponent<Bullet>().exptionAttackAmount =
                        (int)(GameManager.Instance.playerScript.Attack.CurrentValue * percent);

                    Vector3 m_bulletDirection =
                        (new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)));
                    Debug.Log(m_bulletDirection);
                    angle = Mathf.Atan2(m_bulletDirection.y, m_bulletDirection.x) * Mathf.Rad2Deg + 180;
                    temp.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
                    rb = temp.GetComponent<Rigidbody2D>();
                    rb.velocity = m_bulletDirection * 5;
                }

                break;

            case 12:
                skill.SkillInitialize((int)(GameManager.Instance.playerScript.Attack.CurrentValue * percent), _id,
                    0.1f);
                break;

            case 13:
                friend.attackAmount = GameManager.Instance.playerScript.Attack.CurrentValue * (float)(percent / 100);
                friend.maxHealth = (float)GameManager.Instance.playerScript.Health.Cap * (float)(percent_two / 100);
                friend.currentHealth = skill.GetComponent<Friend>().maxHealth;
                skill.SkillInitialize(0, _id, duration);
                break;

            case 14:
                friend.attackAmount = 0;
                friend.maxHealth = (float)GameManager.Instance.playerScript.Health.Cap * (float)(percent / 100);
                friend.currentHealth = friend.maxHealth;
                skill.SkillInitialize(0, _id, duration);
                break;

            case 15:
                friend.attackAmount = 2;
                friend.maxHealth = (float)GameManager.Instance.playerScript.Health.Cap;
                friend.currentHealth = friend.maxHealth;

                for (int i = 0; i < friend.transform.childCount; i++)
                {
                    if (friend.transform.GetChild(i).GetComponent<TimeIntervalAttackSpeed>() != null)
                    {
                        friend.transform.GetChild(i).GetComponent<TimeIntervalAttackSpeed>().duration = seconds;
                        break;
                    }
                }

                break;

            case 16:
                skill.transform.position = _targetPosition;

                friend.maxHealth = (float)GameManager.Instance.playerScript.Health.Cap * (percent / 100);
                friend.currentHealth = friend.maxHealth;
                friend.attackAmount = GameManager.Instance.playerScript.Attack.CurrentValue * (percent_two / 100);

                skill.SkillInitialize((int)friend.attackAmount, _id, duration);
                break;

            case 17:
                skill.transform.position = _targetPosition;

                friend.maxHealth = (float)GameManager.Instance.playerScript.Health.Cap * (percent / 100);
                friend.currentHealth = friend.maxHealth;
                friend.attackAmount = GameManager.Instance.playerScript.Attack.CurrentValue * (percent_two / 100);

                skill.SkillInitialize((int)friend.attackAmount, _id, duration);
                break;

            case 18:
                skill.transform.position = _targetPosition;

                skill.SkillInitialize
                    ((int)(GameManager.Instance.playerScript.Attack.CurrentValue * (percent / 100)), _id, duration);
                break;

            case 19:
                skill.transform.position = _targetPosition;

                skill.SkillInitialize(attackAmount, _id, duration);
                break;

            case 20:
                skill.SkillInitialize(attackAmount, _id, duration);
                break;

            case 21:
                skill.SkillInitialize(100, _id, duration);
                break;

            case 22:
                skill.SkillInitialize(attackAmount, _id, duration);
                break;

            case 23:
                //GameManager.Instance.playerScript.attack
                skill.SkillInitialize(0, _id, duration);
                break;
        }
    }
}