using System.Linq;
using Unity.Linq;
using UnityEngine;

public class RankingPanelUI : MonoBehaviour
{
    public RankingPanelAccessor accessor;
    private RankingUIAccessor template;

    private void Start()
    {
        if (BackEnd.Backend.IsLogin)
        {
            template = GetTemplate();
            CreateRankingAllUI();
        }

        accessor.ExitButton.onClick.AddListener(() => 
        {
            accessor.gameObject.SetActive(false);   
        });
    }

    private void CreateRankingAllUI()
    {
        var rankList = BackEnd.Rank.Rank.GetRankList().OrderBy(x => x.ranking).ToList();
        var myRank = BackEnd.Rank.Rank.GetMyRank();

        InstantiateRankingUI(myRank);

        rankList.Where(rankItem => rankItem != null)
            .ToList()
            .ForEach(rankItem => InstantiateRankingUI(rankItem));
    }

    private RankingUIAccessor GetTemplate()
    {
        var result = accessor.ContentsParent.GetChild(0).GetComponent<RankingUIAccessor>();

        ContentsClear();
        return result;
    }

    private void ContentsClear()
    {
        accessor.ContentsParent.gameObject.Descendants().Destroy();
    }

    private void InstantiateRankingUI(BackEnd.Rank.RankItem rankItem)
    {
        RankingUIAccessor temp = Instantiate(template, accessor.ContentsParent).GetComponent<RankingUIAccessor>();
        InitRankingUI(temp, rankItem);
    }

    private void InitRankingUI(RankingUIAccessor accessor, BackEnd.Rank.RankItem rankItem)
    {
        accessor.GetComponents<Behaviour>().ToList().ForEach(comp => comp.enabled = true);

        accessor.gameObject.Descendants().ToList().ForEach(go =>
        {
            var comp = go.GetComponent<Behaviour>();
            comp.enabled = true;
        });

        accessor.RankingText.text = $"{rankItem.ranking}";  
        accessor.NickNameText.text = $"{rankItem.userName}";
        accessor.MaxDamageText.text = $"{CurrencyHelper.ToCurrencyString(rankItem.score)}";
    }
}