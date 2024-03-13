using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RankingUIAccessor : MonoBehaviour
{
    [SerializeField] private TMP_Text rankingText;
    [SerializeField] private TMP_Text nickNameText;
    [SerializeField] private TMP_Text maxDamageText;
    [SerializeField] private Image rankingImage;

    public TMP_Text RankingText => rankingText;
    public TMP_Text NickNameText => nickNameText;
    public TMP_Text MaxDamageText => maxDamageText;
    public Image RankingImage => rankingImage;
}