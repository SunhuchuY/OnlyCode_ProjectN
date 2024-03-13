using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class RankingPanelAccessor : MonoBehaviour
{
    [SerializeField] private RectTransform contentsParent;
    [SerializeField] private Button exitButton;
    [SerializeField] private RankingUIAccessor template;
    [SerializeField] private RankingUIAccessor myRankUIAccessor;
    [SerializeField] private SpriteAtlas spriteAtlas;

    public RectTransform ContentsParent => contentsParent;
    public Button ExitButton => exitButton;

    public RankingUIAccessor Template => template;
    public RankingUIAccessor MyRankUIAccessor => myRankUIAccessor;
    public SpriteAtlas SpriteAtlas => spriteAtlas;
}