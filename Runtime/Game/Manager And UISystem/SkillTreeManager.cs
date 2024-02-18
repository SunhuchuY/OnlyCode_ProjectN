using System.Collections.Generic;
using UnityEngine;

public class CurCardState
{
    public int id;
    public int level;
    public int count;
}

public class SkillTreeManager : MonoBehaviour
{
    public List<CurCardState> CurCardStates;
    public CardDataContainer CardDataContainer;

    private void Start()
    {
        // init
        for (int i = 0; i < CardDataContainer.cards.Length; i++)
        {
            CardDataContainer.cards[i].cardUpgreadeForNeedMoney =
                new[] { 100, 500, 1500, 6000, 18000, 32000, 64000, 128000, 500000 };
            CardDataContainer.cards[i].cardUpgreadeForCardAmount =
                new[] { 10, 50, 100, 500, 750, 1000, 1250, 1500, 2500 };
        }

        CurCardStates = new(CardDataContainer.cards.Length);
        for (int i = 0; i < CardDataContainer.cards.Length; i++)
        {
            CurCardStates.Add(
                new() { id = CardDataContainer.cards[i].cardIndex, count = 0, level = 1 });
        }

        // temp: 임시적으로 1001~1004번 스킬을 가지고 있다고 설정합니다.
        for (int k = 0; k < 4; k++)
            GameManager.Instance.skillTreeManager.CurCardStates[k].count++;

        GameManager.Instance.skillManager.Initialize();
    }
}