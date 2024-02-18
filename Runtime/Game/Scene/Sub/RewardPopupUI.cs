using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class RewardPopupUI : MonoBehaviour
{
    private const float DURATION = 2f;

    [SerializeField] private RewardPopupUIAccessor accessor;
    [SerializeField] private SpriteAtlas atlas;

    private Queue<Quest> questQueue = new();
    private Tween tweener;

    private void Awake()
    {
        Init();
        CheckNextReward();
    }

    private void Init()
    {
        accessor.SkipButton.onClick.AddListener(PopupSkip);

        Vector2 originalPosition = accessor.transform.position;
        tweener = accessor.transform.DOMoveY(originalPosition.y + 2f, DURATION)
            .SetEase(Ease.OutQuint)
            .OnPlay(() => 
            {
                accessor.gameObject.SetActive(true);
            })
            .OnComplete(() =>
            {
                accessor.gameObject.SetActive(false);
                accessor.transform.position = originalPosition;
                CheckNextReward();
            })
            .Pause();
    }

    public void OnPopup(Quest quest)
    {
        if (tweener.IsPlaying())
        {
            questQueue.Enqueue(quest);    
        }
        else
        {
            UpdateUI(quest);
            tweener.Restart();
        }
    }

    private void PopupSkip()
    {
        tweener.Complete();
    }

    private void CheckNextReward()
    {
        if (questQueue.Count > 0)
        {
            OnPopup(questQueue.Dequeue());
        }
    }

    private void UpdateUI(Quest quest)
    {
        accessor.RewardImage.sprite = atlas.GetSprite(GetSpriteName(quest.reward.type));
        accessor.TitleText.text = quest.ConditionStr;
        accessor.RewardText.text = CurrencyHelper.ToCurrencyString(quest.reward.value);
    }

    private string GetSpriteName(Quest.Reward.Type type)
    {
        return type switch
        {
            Quest.Reward.Type.Diamond => "i_diamond",
            Quest.Reward.Type.Gold => "i_gold",
            _ => null,
        };
    }
}