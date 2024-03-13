using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public event System.Action OnSkillsCanUseNumsChanged;
    public event System.Action OnNextSkillChanged;
    public List<int?> SkillSlotIds;
    public int? NextSkillId { get; private set; }
    public float NextSkillCooldown;

    private bool isInit;

    private IEnumerator Start()
    {
        yield return null;
        yield return null;
        yield return null;

        Initialize();
    }

    public void Initialize()
    {
        SkillSlotIds = new() { RandCanUseSkillID(), RandCanUseSkillID(), RandCanUseSkillID(), RandCanUseSkillID() };
        OnSkillsCanUseNumsChanged?.Invoke();
    }

    private void Update()
    {
        if (SkillSlotIds == null)
        {
            return;
        }

        if (NextSkillCooldown > 0)
        {
            NextSkillCooldown -= Time.deltaTime;
        }

        if (NextSkillCooldown <= 0
            && SkillSlotIds.Count < 4)
        {
            // 다음 스킬을 추가합니다.
            SkillSlotIds.Add(NextSkillId);
            NextSkillId = null;
            OnSkillsCanUseNumsChanged?.Invoke();
        }

        if (NextSkillId == null)
        {
            ReFillNextSkill();
        }
    }

    private int RandCanUseSkillID()
    {
        var _skillsCanUse =
            GameManager.Instance.userDataManager.userData.SkillsInUseList.Where(x => DataTable.ActiveSkillDataTable.ContainsKey(x)).ToList();
        return _skillsCanUse.Random();
    }

    public void ReFillNextSkill()
    {
        NextSkillCooldown = 2f;
        NextSkillId = RandCanUseSkillID();
        OnNextSkillChanged?.Invoke();
    }

    public void UseSkill(int _slotIndex, Vector3 _targetPosition)
    {
        int _id = SkillSlotIds[_slotIndex].Value;
        var _skillData = DataTable.ActiveSkillDataTable[_id];

        if (GameManager.Instance.playerScript.Stats["Mp"].CurrentValueInt < _skillData.Cost)
        {
            GameManager.Instance.commonUI.ToastMessage("마나가 부족합니다.");
            return;
        }

        int _level = GameManager.Instance.userDataManager.userData.SkillDict[_id].Level;
        var _player = GameManager.Instance.playerScript;

        _player.SkillController.Perform(
            _player, _skillData, _level, _targetPosition);

        GameManager.Instance.playerScript.Stats["Mp"].ApplyModifier(new StatModifier() { Magnitude = -_skillData.Cost });

        OnSkillsCanUseNumsChanged?.Invoke();
        SkillSlotIds.RemoveAt(_slotIndex);
    }
}