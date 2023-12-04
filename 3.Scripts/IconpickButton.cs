using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconpickButton : MonoBehaviour
{
    public int skillNum = 0;

    private void Start()
    {
        gameObject.GetComponent<Image>().sprite = GameManager.Instance.skillTreeManager.cardDatas[skillNum].cardSprite;
    }
}
