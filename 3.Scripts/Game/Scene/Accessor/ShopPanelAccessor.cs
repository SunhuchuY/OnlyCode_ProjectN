using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopPanelAccessor : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI diamondText;
    [SerializeField] private Button closeButton;

    [SerializeField] private TextMeshProUGUI summonMagicGacha30CostText;
    [SerializeField] private Button summonMagicGacha30Button;
    [SerializeField] private TextMeshProUGUI summonMagicGacha12CostText;
    [SerializeField] private Button summonMagicGacha12Button;
    [SerializeField] private Button summonMagicGachaAdsButton;

    [SerializeField] private TextMeshProUGUI attackMagicGacha30CostText;
    [SerializeField] private Button attackMagicGacha30Button;
    [SerializeField] private TextMeshProUGUI attackMagicGacha12CostText;
    [SerializeField] private Button attackMagicGacha12Button;
    [SerializeField] private Button attackMagicGachaAdsButton;

    [SerializeField] private RectTransform dailyItemsRoot;

    public TextMeshProUGUI DiamondText => diamondText;
    public Button CloseButton => closeButton;

    public TextMeshProUGUI SummonMagicGacha30CostText => summonMagicGacha30CostText;
    public Button SummonMagicGacha30Button => summonMagicGacha30Button;
    public TextMeshProUGUI SummonMagicGacha12CostText => summonMagicGacha12CostText;
    public Button SummonMagicGacha12Button => summonMagicGacha12Button;
    public Button SummonMagicGachaAdsButton => summonMagicGachaAdsButton;

    public TextMeshProUGUI AttackMagicGacha30CostText => attackMagicGacha30CostText;
    public Button AttackMagicGacha30Button => attackMagicGacha30Button;
    public TextMeshProUGUI AttackMagicGacha12CostText => attackMagicGacha12CostText;
    public Button AttackMagicGacha12Button => attackMagicGacha12Button;
    public Button AttackMagicGachaAdsButton => attackMagicGachaAdsButton;

    public RectTransform DailyItemsRoot => dailyItemsRoot;
}