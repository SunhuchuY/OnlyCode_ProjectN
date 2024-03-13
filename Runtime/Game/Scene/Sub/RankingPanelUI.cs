using System.Linq;
using Unity.Linq;
using UnityEngine;
using BackEnd.Rank;


public class RankingPanelUI : MonoBehaviour
{
    public RankingPanelAccessor accessor;

    private void Awake()
    {
        accessor.ExitButton.onClick.AddListener(OnClickExitButton);
    }

    private void Start()
    {
        if (!BackEnd.Backend.IsLogin)
        {
            return;
        }        

        ContentsClear();
        UpdateRankingAllUI();
    }

    private void UpdateRankingAllUI()
    {
        var rankList = Rank.GetRankList()
            .OrderBy(x => x.ranking)
            .ToList();
        int contentChildCount = accessor.ContentsParent.childCount;

        rankList
            .ForEach(rankItem =>
            {
                int rankUIIndex = rankItem.ranking;

                // 수정, 생성중 선택합니다.
                RankingUIAccessor rankingUIAccessor = rankUIIndex < contentChildCount
                    ? accessor.transform.GetChild(rankUIIndex).GetComponent<RankingUIAccessor>()
                    : Instantiate(accessor.Template, accessor.ContentsParent).GetComponent<RankingUIAccessor>();

                InsertRankingUI(rankingUIAccessor, rankItem);
            });

        InsertRankingUI(accessor.MyRankUIAccessor, Rank.GetMyRank());
    }

    private void ContentsClear()
    {
        accessor.ContentsParent.gameObject
            .Descendants()
            .Destroy();
    }

    private void InsertRankingUI(RankingUIAccessor accessor, RankItem rankItem)
    {
        // 3위안으로는 텍스트 대신 이미지를 사용합니다.
        if (rankItem.ranking <= 3) 
        {
            accessor.RankingImage.sprite = this.accessor.SpriteAtlas.GetSprite($"ui_icon_ranking_{rankItem.ranking}");
            accessor.RankingText.gameObject.SetActive(false);
        }
        else
        {
            accessor.RankingText.text = $"{rankItem.ranking}";
            accessor.RankingImage.gameObject.SetActive(false);
        }

        accessor.NickNameText.text = $"{rankItem.userName}";
        accessor.MaxDamageText.text = $"{CurrencyHelper.ToCurrencyString(rankItem.score)}";
    }
    private void OnClickExitButton() 
    {
        accessor.gameObject.SetActive(false);
    }
}
