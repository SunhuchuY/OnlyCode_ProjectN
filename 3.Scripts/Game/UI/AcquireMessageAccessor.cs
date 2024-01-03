using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AcquireMessageAccessor : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Image thumbnail;

    public CanvasGroup CanvasGroup => canvasGroup;
    public TextMeshProUGUI Text => text;
    public Image Thumbnail => thumbnail;
}