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

    public void Initialize()
    {
        SkillSlotIds = new() { RandCanUseSkillID(), RandCanUseSkillID(), RandCanUseSkillID(), RandCanUseSkillID() };
        OnSkillsCanUseNumsChanged?.Invoke();
    }

    private void Update()
    {
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
            GameManager.Instance.skillTreeManager.CurCardStates.Where(x => x.count > 0).ToList();
        return _skillsCanUse.Random().id;
    }

    public void ReFillNextSkill()
    {
        NextSkillCooldown = 2f;
        NextSkillId = RandCanUseSkillID();
        OnNextSkillChanged?.Invoke();
    }

    public void UseSkill(int _slotIndex, Vector3 _targetPosition)
    {
        int _legacyId = SkillSlotIds[_slotIndex].Value;
        int _id = _legacyId + 1001; // temp: 임시적으로 구버전 체계의 id에 1001을 더해 신버전 체계의 id로 바꿉니다.
        var _skillData = DataTable.ActiveSkillDataTable[_id];

        if (GameManager.Instance.playerScript.Stats["Mana"].CurrentValueInt < _skillData.Cost)
        {
            GameManager.Instance.commonUI.ToastMessage("마나가 부족합니다.");
            return;
        }

        int _level = GameManager.Instance.skillTreeManager.CurCardStates[_legacyId].level;
        var _player = GameManager.Instance.playerScript;

        _player.SkillController.Perform(
            _player, _skillData, _level, _targetPosition);

        GameManager.Instance.playerScript.Stats["Mana"].ApplyModifier(new StatModifier() { Magnitude = -_skillData.Cost });

        OnSkillsCanUseNumsChanged?.Invoke();
        SkillSlotIds.RemoveAt(_slotIndex);
    }
}