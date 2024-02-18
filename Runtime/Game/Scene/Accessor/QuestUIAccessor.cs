using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class QuestUIAccessor : MonoBehaviour
{
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private Image questIcon;
    [SerializeField] private Button acquireButton;
    [SerializeField] private TMP_Text conditionText;

    [Header("Story")]
    [SerializeField] private Button storyRestartButton;


    [SerializeField] private QuestInfoAccessor questAcquire;
    [SerializeField] private RectTransform questComplete;
    [SerializeField] private QuestInfoAccessor questCondition;

    public State state { get; private set; }
    
    public TMP_Text TitleText => titleText;
    
    public Image QuestIcon => questIcon;

    public Button AcquireButton => acquireButton;
    public Button StoryRestartButton => storyRestartButton;

    public TMP_Text ConditionText => conditionText;

    [Serializable]
    public class QuestInfoAccessor
    {
        [SerializeField] private RectTransform rectTf;
        [SerializeField] private TMP_Text costText;
        [SerializeField] private Image icon;
        public TMP_Text CostText => costText;
        public Image Icon => icon;
        public RectTransform RectTf => rectTf;
    }

    public enum State
    {
        Acquire,
        Complete,
        Condition
    }

    public void ChangeState(State state)
    {
        DeActiveStateRT();

        this.state = state;

        switch (state)
        {
            case State.Acquire:
                questAcquire.RectTf.gameObject.SetActive(true);
                break;
            case State.Complete:
                questComplete.gameObject.SetActive(true);
                break;
            case State.Condition:
                questCondition.RectTf.gameObject.SetActive(true);
                break;
        }
    }

    private void DeActiveStateRT()
    {
        questCondition.RectTf.gameObject.SetActive(false);
        questAcquire.RectTf.gameObject.SetActive(false);  
        questComplete.gameObject.SetActive(false);  
    }

    public void Initialize(Sprite rewardIcon, int cost)
    {
        questCondition.Icon.sprite = rewardIcon;
        questAcquire.Icon.sprite = rewardIcon;

        questCondition.CostText.text = $"{cost}";
        questAcquire.CostText.text = $"{cost}";
    }
}