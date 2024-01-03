using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProfileAndProgressPanelAccessor : MonoBehaviour
{
    [SerializeField] private Image avatarIcon;
    [SerializeField] private TextMeshProUGUI usernameText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI experienceText;
    [SerializeField] private Slider experienceSlider;
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private TextMeshProUGUI monsterKillCountText;
    [SerializeField] private Slider monsterKillCountSlider;

    public Image AvatarIcon => avatarIcon;
    public TextMeshProUGUI UsernameText => usernameText;
    public TextMeshProUGUI LevelText => levelText;
    public TextMeshProUGUI ExperienceText => experienceText;
    public Slider ExperienceSlider => experienceSlider;
    public TextMeshProUGUI WaveText => waveText;
    public TextMeshProUGUI MonsterKillCountText => monsterKillCountText;
    public Slider MonsterKillCountSlider => monsterKillCountSlider;
}