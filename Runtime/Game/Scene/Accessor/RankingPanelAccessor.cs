using UnityEngine;
using UnityEngine.UI;

public class RankingPanelAccessor : MonoBehaviour
{
    [SerializeField] private RectTransform contentsParent;
    [SerializeField] private Button exitButton;

    public RectTransform ContentsParent => contentsParent;
    public Button ExitButton => exitButton;
}