using TMPro;
using UnityEngine;

public class ToastMessageAccessor : MonoBehaviour
{
    [SerializeField] private RectTransform messageBox;
    [SerializeField] private TextMeshProUGUI messageText;

    public RectTransform MessageBox => messageBox;
    public TextMeshProUGUI MessageText => messageText;
}