using UnityEngine;
using UnityEngine.UI;

public class BossPopup : MonoBehaviour
{
    const string ILLUSTIMAGE_NAME_FOREHEAD = "illust_";

    [SerializeField] GameObject bossPopUpObject;
    [SerializeField] Image bossIllustrationImage;

    private Animator anim;

    private void Start()
    {
        anim = bossPopUpObject.GetComponent<Animator>();
        Hide();
    }

    private void OnStateExit(AnimatorStateInfo _stateInfo, int _layer)
    {
        if (_stateInfo.IsName("Popup"))
            bossPopUpObject.SetActive(false);
    }

    public void Show()
    {
        // 마지막 인덱스는 보스몬스터입니다.
        int bossID = StageParser.Instance.bigWaveProfiles[GameManager.Instance.userDataManager.userData.BigWave].
            MonsterIDs[StageParser.Instance.bigWaveProfiles[GameManager.Instance.userDataManager.userData.BigWave].MonsterIDs.Count - 1];
        string bossName = StageParser.Instance.monsterProfiles[bossID].MonsterImageName;

        bossIllustrationImage.sprite = GetBossSprite(bossName);
        bossPopUpObject.SetActive(true);
        anim.Rebind();
        anim.Update(0);
    }

    public void Hide()
    {
        bossPopUpObject.SetActive(false);
    }

    /// <summary> 일러스트 이름과 같지 않은 상황마저 체크합니다. </summary>
    private Sprite GetBossSprite(string bossName)
    {
        Sprite result = null;
        result = Resources.Load<Sprite>($"Sprite/Illustration/{ILLUSTIMAGE_NAME_FOREHEAD + bossName}");

        if (result == null)
        {
            int lastUnderBarIndex = bossName.LastIndexOf('_');

            if (lastUnderBarIndex != -1)
            {
                string newBossName = bossName.Substring(0, lastUnderBarIndex);
                result = Resources.Load<Sprite>($"Sprite/Illustration/{ILLUSTIMAGE_NAME_FOREHEAD + newBossName}");
            }
        }

        return result;
    }
}