using UnityEngine;
using TMPro;

public class AppearTextAccessor : MonoBehaviour
{
    [SerializeField] private TMP_Text text;

    public TMP_Text Text => text;
}