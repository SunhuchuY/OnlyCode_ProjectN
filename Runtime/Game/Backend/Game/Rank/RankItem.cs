
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BackEnd.Rank
{
    public class RankItem
    {
        public string userName { get; private set; }
        public int score { get; private set; }
        public int ranking { get; private set; }

        public RankItem(string userName, int sum, int ranking)
        {
            this.userName = userName;
            this.score = sum;
            this.ranking = ranking;
        }
    }
}