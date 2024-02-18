using UnityEngine;
using TMPro;

public class RankingUIAccessor : MonoBehaviour
{
    [SerializeField] private TMP_Text rankingText;
    [SerializeField] private TMP_Text nickNameText;
    [SerializeField] private TMP_Text maxDamageText;

    public TMP_Text RankingText => rankingText;
    public TMP_Text NickNameText => nickNameText;
    public TMP_Text MaxDamageText => maxDamageText;
}