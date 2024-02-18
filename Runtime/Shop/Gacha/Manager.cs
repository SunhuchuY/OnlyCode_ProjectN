using Shop.Gacha.Animation;
using Shop.Gacha.GachaListButtons;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;
using System;
using System.Collections;

namespace Shop.Gacha
{
    public class Manager : MonoBehaviour
    {
        private const float START_DURATION = 2f;
        private const int SUBTRACT_ID = 1001;

        public static Manager Instance { get; private set; }

        [SerializeField] private Animation.GachaAnimationUI_Events gachaAnimationUI_Events;
        [SerializeField] private ImageManager imageManager;
        [SerializeField] private CircleManager circleManager;

        private Dictionary<SkillGrade, List<ActiveSkillData>> summonList;
        private Dictionary<SkillGrade, List<ActiveSkillData>> attackList;

        private IEnumerator Start()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            yield return null;
            yield return null;
            yield return null;

            InitSkillList();
        }

        private void InitSkillList()
        {
            attackList = new Dictionary<SkillGrade, List<ActiveSkillData>>()
            {
                { SkillGrade.R, new() },
                { SkillGrade.SR, new() },
                { SkillGrade.SSR, new() },
            };

            summonList = new Dictionary<SkillGrade, List<ActiveSkillData>>()
            {
                { SkillGrade.R, new() },
                { SkillGrade.SR, new() },
                { SkillGrade.SSR, new() },
            };

            DataTable.ActiveSkillDataTable
                .Where(data => data.Value.Cost != 0)
                .Select(data => data.Value)
                .ToList()
                .ForEach(data => 
                {
                    if (data.Type == SkillType.Active)
                    {
                        attackList[data.Grade].Add(data);
                    }
                    else if (data.Type == SkillType.Summon)
                    {
                        summonList[data.Grade].Add(data);
                    }
                });
        }

        public void StartGacha(GachaType gachaType, int count)
        {
            gachaAnimationUI_Events.FadeoutStartEventHandler(START_DURATION);
            circleManager.ChangeColor(Color.white);

            List<ActiveSkillData> skillList = RandSkillList(gachaType, count);

            // 이미지를 로드합니다.
            imageManager.InitImage(skillList);
            GachaListManager.Instance?.GachaListInitialize(skillList.Count);

            // 카드중 큰 랭크를 Circle 색으로 지정합니다.
            circleManager.circleColors = CircleColorUtils.GetCircleColors(skillList.Max(skill => skill.Grade));

            // 가차 횟수를 업데이트합니다.
            if (BackEnd.Backend.IsLogin)
            {
                GameManager.Instance.questData.questData.DailyGachaCount.Value++;
                GameManager.Instance.questData.questData.WeekGachaCount.Value++;
            }

            // SkillTreeManager에 스킬개수를 업데이트합니다.
            foreach (ActiveSkillData skill in skillList)
            {
                if (GameManager.Instance.userDataManager.userData.SkillDict.ContainsKey(skill.Id))
                {
                    GameManager.Instance.userDataManager.userData.SkillDict[skill.Id].Count++;
                }
                else
                {
                    GameManager.Instance.userDataManager.userData.SkillDict.Add(skill.Id, new SavedSkillData() { Count = 1, Level = 1 });
                }
            }
        }

        private List<ActiveSkillData> RandSkillList(GachaType gachaType, int count)
        {
            List<ActiveSkillData> result = new();

            switch (gachaType)
            {
                case GachaType.SummonsMagic:
                    for (int i = 0; i < count; i++)
                    {
                        SkillGrade grade = RandSkillGrade();
                        result.Add(summonList[grade].Random());
                    }
                    
                    break;

                case GachaType.AttackMagic:
                    for (int i = 0; i < count; i++)
                    {
                        SkillGrade grade = RandSkillGrade();
                        result.Add(attackList[grade].Random());
                    }
                    break;

                default:
                    throw new NotSupportedException();
            }

            return result;
        }

        private SkillGrade RandSkillGrade()
        {
            const int R_PERCENT = 60;
            const int SR_PERCENT = 30;
            const int SRR_PERCENT = 10;

            int rand = UnityEngine.Random.Range(1, 101);

            if (rand <= SRR_PERCENT)
            {
                return SkillGrade.SSR;
            }
            else if (rand <= SR_PERCENT)
            {
                return SkillGrade.SR;
            }
            else
            {
                return SkillGrade.R;
            }
        }
    }
}

