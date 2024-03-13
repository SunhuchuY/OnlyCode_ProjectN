using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class QuestUIAccessor : MonoBehaviour
{
    [Serializable]
    public class QuestInfoAccessor
    {
        [SerializeField] private RectTransform rectTf;
        [SerializeField] private TMP_Text costText;
        [SerializeField] private Image icon;
        [SerializeField] private Button button;
        
        public TMP_Text CostText => costText;
        public Image Icon => icon;
        public RectTransform RectTf => rectTf;
        public Button Button => button;
    }

    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text rewardText;
    [SerializeField] private Image questIcon;
    [SerializeField] private Slider conditionSlider; 

    [SerializeField] private QuestInfoAccessor questAcquire;
    [SerializeField] private QuestInfoAccessor questComplete;
    [SerializeField] private QuestInfoAccessor questCondition;

    public State state { get; private set; }
    
    public TMP_Text TitleText => titleText;
    public TMP_Text RewardText => rewardText;
    public Image QuestIcon => questIcon;
    public Slider ConditionSlider => conditionSlider;   

    public QuestInfoAccessor QuestAcquire => questAcquire;
    public QuestInfoAccessor QquestComplete => questComplete;
    public QuestInfoAccessor QuestCondition => questCondition;

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
                questComplete.RectTf.gameObject.SetActive(true);
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
        questComplete.RectTf.gameObject.SetActive(false);  
    }

    public void Initialize(Sprite rewardIcon, int cost)
    {
        questCondition.Icon.sprite = rewardIcon;
        questAcquire.Icon.sprite = rewardIcon;

        questCondition.CostText.text = $"{cost}";
        questAcquire.CostText.text = $"{cost}";
    }
}