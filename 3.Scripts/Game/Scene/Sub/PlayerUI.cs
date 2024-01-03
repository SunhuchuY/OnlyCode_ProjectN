using DG.Tweening;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Player player;

    [SerializeField] private PlayerHealthAndManaPanelAccessor playerHealthAndManaPanelAccessor;

    private float healthFillTweenTime = 0.6f;
    private float healthEmergencyRatio = 0.3f;
    private Color healthColorOrigin;
    private Tweener healthFillRatioLastTween;
    private Tweener healthFillFadeLastTween;

    private void Start()
    {
        healthColorOrigin = playerHealthAndManaPanelAccessor.HealthFillImage.color;

        player.OnChangeDefense += _defense => ChangeHealthFillColor();
        player.OnChangeInvincible += _invincible => ChangeHealthFillColor();

        player.Health.OnCurrentValueChanged += _newValue => UpdateHealthBar();
        player.Health.OnCapChanged += _newCap => UpdateHealthBar();

        player.Mana.OnCurrentValueChanged += _newValue => UpdateMana();
        player.Mana.OnCapChanged += _newCap => UpdateMana();

        ChangeHealthFillColor();
        UpdateHealthBar();
        UpdateMana();
    }

    private void OnDestroy()
    {
        // todo: 구독한 이벤트를 모두 해제합니다.

        if (healthFillRatioLastTween != null && healthFillRatioLastTween.IsActive())
        {
            healthFillRatioLastTween.Kill(true);
            healthFillRatioLastTween = null;
        }

        if (healthFillFadeLastTween != null && healthFillFadeLastTween.IsActive())
        {
            // 깜빡거리는 애니메이션이 재생중이라면, 이를 중지합니다.
            healthFillFadeLastTween.Kill(true);
            healthFillFadeLastTween = null;
        }
    }

    private void ChangeHealthFillColor()
    {
        bool _changeToGray = player.IsDefenseUp || player.IsInvincible;

        playerHealthAndManaPanelAccessor.HealthFillImage.color = _changeToGray ? Color.gray : healthColorOrigin;
    }

    private void UpdateHealthBar()
    {
        float _ratio = (float)player.Health.CurrentValue / player.Health.Cap;

        if (healthFillRatioLastTween != null && healthFillRatioLastTween.IsActive())
        {
            healthFillRatioLastTween.Kill();
            healthFillRatioLastTween = null;
        }

        if (_ratio <= healthEmergencyRatio)
        {
            // 플레이어의 체력이 별로 남아있지 않을 때, 체력 게이지가 깜빡거리도록 합니다.
            healthFillRatioLastTween =
                playerHealthAndManaPanelAccessor.HealthSlider
                    .DOValue(_ratio, healthFillTweenTime)
                    .SetEase(Ease.Linear);

            if (healthFillFadeLastTween == null)
            {
                // 깜빡거리는 애니메이션이 재생중이 아니라면, 이를 재생합니다.
                healthFillFadeLastTween =
                    playerHealthAndManaPanelAccessor.HealthFillImage
                        .DOFade(0.2f, healthFillTweenTime)
                        .SetLoops(-1, LoopType.Yoyo);
            }
        }
        else
        {
            healthFillRatioLastTween =
                playerHealthAndManaPanelAccessor.HealthSlider
                    .DOValue(_ratio, healthFillTweenTime)
                    .SetEase(Ease.Linear);

            if (healthFillFadeLastTween != null && healthFillFadeLastTween.IsActive())
            {
                // 깜빡거리는 애니메이션이 재생중이라면, 이를 중지합니다.
                healthFillFadeLastTween.Kill(true);
                healthFillFadeLastTween = null;

                Color _color = playerHealthAndManaPanelAccessor.HealthFillImage.color;
                _color.a = 1.0f;
                playerHealthAndManaPanelAccessor.HealthFillImage.color = _color;
            }
        }

        playerHealthAndManaPanelAccessor.HealthText.text = player.Health.CurrentValue.ToString();
    }

    private void UpdateMana()
    {
        for (int i = 0; i < player.Mana.CurrentValue; ++i)
            playerHealthAndManaPanelAccessor.ManaSlots.GetChild(i).GetChild(0).gameObject.SetActive(true);

        for (int i = player.Mana.CurrentValue; i < player.Mana.Cap; ++i)
            playerHealthAndManaPanelAccessor.ManaSlots.GetChild(i).GetChild(0).gameObject.SetActive(false);

        playerHealthAndManaPanelAccessor.ManaText.text = $"Mana : {player.Mana.CurrentValue}";
    }
}