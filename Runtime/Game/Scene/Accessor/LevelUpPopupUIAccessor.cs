using UnityEngine;
using TMPro;

public class LevelUpPopupUIAccessor : MonoBehaviour
{
    [SerializeField] private Animation animation;
    [SerializeField] private TMP_Text changeLevelText;
    [SerializeField] private TMP_Text changeAtkText;

    public Animation Animation => animation;
    public TMP_Text ChangeLevelText => changeLevelText;
    public TMP_Text ChangeAtkText => changeAtkText;

    public void ActiveFalse()
    {
        gameObject.SetActive(false);
    }
}