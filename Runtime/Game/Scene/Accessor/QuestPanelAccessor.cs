using UnityEngine;
using UnityEngine.UI;

public class QuestPanelAccessor : MonoBehaviour
{
    [SerializeField] private Button dailyButton;
    [SerializeField] private Button weekButton;
    [SerializeField] private Button storyButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private RectTransform dailyPanel;
    [SerializeField] private RectTransform weekPanel;
    [SerializeField] private RectTransform storyPanel;
    [SerializeField] private RectTransform dailyPlaceholder;
    [SerializeField] private RectTransform weekPlaceholder;
    [SerializeField] private RectTransform storyPlaceholder;

    [SerializeField] private QuestPanelUI UI;

    public Button ExitButton => exitButton;
    public Button DailyButton => dailyButton;
    public Button WeekButton => weekButton;
    public Button StoryButton => storyButton;
    public RectTransform DailyPanel => dailyPanel;
    public RectTransform WeekPanel => weekPanel;
    public RectTransform StoryPanel => storyPanel;
    public RectTransform DailyPlaceholder => dailyPlaceholder;
    public RectTransform WeekPlaceholder => weekPlaceholder;
    public RectTransform StoryPlaceholder => storyPlaceholder;

}