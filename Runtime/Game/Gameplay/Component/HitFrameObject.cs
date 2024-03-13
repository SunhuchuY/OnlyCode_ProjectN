using Cysharp.Threading.Tasks;
using DG.Tweening;
using Spine.Unity;
using System;
using UnityEngine;

public class HitFrameObject : MonoBehaviour
{
    public HitFrameObjectContext Context;

    private float RunningTime = 0.0f;
    private int NextHitIndex = 0;

    private void Start()
    {
        var _spineAnim = GetComponentInChildren<SkeletonAnimation>();
        if(_spineAnim != null)
        {
            _spineAnim.AnimationState.Complete += _ =>
            {
                UniTask.Create(async () =>
                {
                    await UniTask.WaitForSeconds(0.2f);
                    DOTween.To(() => _spineAnim.skeleton.A, value => { _spineAnim.skeleton.A = value; }, 0f, .1f)
                        .OnComplete(() => Destroy(gameObject));
                }).Forget();
            };
        }
    }

    private void Update()
    {
        if (Context.Data.ManualHitTimes.Count > 0)
        {
            // manual hit times의 요소가 하나라도 있다면,
            // 이 오브젝트의 애니메이션에 hit event가 없어도 수동으로 hit event가 발생했다고 간주하여
            // 적절한 시점에 Hit() 함수를 실행합니다.

            if (NextHitIndex >= Context.Data.ManualHitTimes.Count)
                return;

            RunningTime += Time.deltaTime;
            if (RunningTime >= Context.Data.ManualHitTimes[NextHitIndex])
            {
                Hit();
                ++NextHitIndex;
            }
        }
    }

    public void Hit()
    {
        var _actionIndexes = Context.Data.ActionIndexes[Context.NextHitFrameIndex];

        _actionIndexes.ForEach(x =>
        {
            var _data = Context.Data.Actions[x];
            var _handler = GameplayHandlers.GetActionHandler(_data.GetType());
            try
            {
                _handler.Invoke(_data, Context.ActionContext);
            }
            catch (Exception e)
            {
                Debug.LogError($"{e.GetType().Name}: {e.Message}\n{e.StackTrace}");
            }
        });

        ++Context.NextHitFrameIndex;
    }
}