using TMPro;
using UnityEngine;

public class CurrencyPanelAccessor : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI magicstoneText;
    [SerializeField] private TextMeshProUGUI diamondText;

    public TextMeshProUGUI GoldText => goldText;
    public TextMeshProUGUI MagicstoneText => magicstoneText;
    public TextMeshProUGUI DiamondText => diamondText;
}