using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManagerUI : MonoBehaviour
{
    private const string BackgroundPath = "Sprite/Background/Stage";

    [SerializeField] private SpriteRenderer backgroundRenderer;
    [SerializeField] private WaveManager waveManager;

    private void Start()
    {
        waveManager.OnBigWaveIndexChanged += UpdateBackground;
    }

    private void UpdateBackground()
    {
        string backgroundName = StageParser.Instance.bigWaveProfiles[GameManager.Instance.userDataManager.userData.BigWave].FieldImageName;
        string path = BackgroundPath + "/" + backgroundName;

        if (backgroundRenderer.sprite.name != backgroundName)
        {
            Sprite sprite = Resources.Load<Sprite>(path);

            if (sprite == null)
            {
                Debug.LogError($"{backgroundName}의 배경이미지가 {BackgroundPath}의 경로에 존재하지 않습니다.");
                return;
            }

             backgroundRenderer.sprite = sprite;
        }
    }
}
