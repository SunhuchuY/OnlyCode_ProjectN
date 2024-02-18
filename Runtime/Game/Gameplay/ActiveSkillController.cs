using System;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSkillController : MonoBehaviour
{
    private List<ActiveSkillContext> skillsInRunning = new();

    private void Update()
    {
        UpdateSkills();
    }

    private void UpdateSkills()
    {
        HashSet<ActiveSkillContext> skillsInEnd = new();

        foreach (ActiveSkillContext _context in skillsInRunning)
        {
            _context.RunningTime += Time.deltaTime;

            var _nextActionTime = _context.Data.ActionTimes[_context.NextActionTimesIndex];
            if (_context.RunningTime >= _nextActionTime.time)
            {
                ++_context.NextActionTimesIndex;
                if (_context.NextActionTimesIndex >= _context.Data.ActionTimes.Length)
                {
                    // active skill의 모든 action을 실행한 뒤 종료합니다.
                    skillsInEnd.Add(_context);
                }

                var _data = _context.Data.Actions[_nextActionTime.index];
                Type type = _data.GetType();
                var _handler = GameplayHandlers.GetActionHandler(type);

                try
                {
                    _handler.Invoke(_data, _context.ActionContext);
                }
                catch (Exception e)
                {
                    Debug.LogError($"{e.GetType().Name}: {e.Message}\n{e.StackTrace}");
                }
            }
        }

        // 이번 프레임에 종료된 action을 제거합니다.
        skillsInRunning.RemoveAll(x => skillsInEnd.Contains(x));
    }

    public void Perform(IGameActor _actor, ActiveSkillData _data, int _skillLevel, Vector3 _skillPosition)
    {
        float _scaleByLevel = 0f;
        if (_data.LevelPer[^1] == '+')
        {
            string _valueStr = _data.LevelPer.Substring(0, _data.LevelPer.Length - 1);
            float _value = float.Parse(_valueStr);
            _scaleByLevel = _value * (_skillLevel - 1);
        }
        else
        {
            float _value = float.Parse(_data.LevelPer);
            _scaleByLevel = Mathf.Pow(_value, _skillLevel - 1);
        }

        skillsInRunning.Add(new()
        {
            Data = _data,
            SkillLevel = _skillLevel,
            ActionContext = new ActionSequenceContext()
            {
                Actor = _actor, ScaleByLevel = _scaleByLevel, Position = _skillPosition
            }
        });
    }
}