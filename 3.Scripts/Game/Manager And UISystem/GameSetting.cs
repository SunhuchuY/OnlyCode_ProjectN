using UnityEngine;
using UnityEngine.UI;

public class GameSetting : MonoBehaviour
{
    [SerializeField] private Image speedImage;
    [SerializeField] private Sprite[] speedSprites = new Sprite[3];

    private float curSpeed = 1f;

    public void OnClickSpeedButton()
    {
        switch (curSpeed)
        {
            case 1f: // 2배로
                speedImage.sprite = speedSprites[1];
                Time.timeScale = 1.5f;
                curSpeed = 2f;
                break;
            case 2f: // 3배로
                speedImage.sprite = speedSprites[2];
                Time.timeScale = 2f;
                curSpeed = 3f;

                break;
            case 3f: // 1배로
                speedImage.sprite = speedSprites[0];
                Time.timeScale = 1f;
                curSpeed = 1f;
                break;
        }
    }
}