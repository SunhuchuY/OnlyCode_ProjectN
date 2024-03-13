using DG.Tweening;
using System.Collections;
using System.Linq;
using UniRx;
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

    private IEnumerator Start()
    {
        yield return null;

        healthColorOrigin = playerHealthAndManaPanelAccessor.HealthFillImage.color;

        player.OnChangeDefense += _defense => ChangeHealthFillColor();
        
        player.TagController.tags.ObserveAdd()
            .Subscribe(_ => ChangeHealthFillColor())
            .AddTo(this);
        player.TagController.tags.ObserveRemove()
            .Subscribe(_ => ChangeHealthFillColor())
            .AddTo(this);

        player.Stats["Hp"].OnChangesCurrentValueIntAsObservable.Subscribe(UpdateHealthBar).AddTo(gameObject);
        player.Stats["Hp"].Cap.Select(Mathf.RoundToInt).Subscribe(UpdateHealthBar).AddTo(gameObject);

        player.Stats["Mp"].OnChangesCurrentValueIntAsObservable.Subscribe(UpdateMana).AddTo(gameObject);
        player.Stats["Mp"].Cap.Select(Mathf.RoundToInt).Subscribe(UpdateMana).AddTo(gameObject);

        ChangeHealthFillColor();
        UpdateHealthBar(0);
        UpdateMana(0);
    }

    private void OnDestroy()
    {
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
        bool _changeToGray = player.IsDefenseUp || player.GetIsInvincible();

        playerHealthAndManaPanelAccessor.HealthFillImage.color = _changeToGray 
            ? Color.gray 
            : healthColorOrigin;
    }

    private void UpdateHealthBar(int _)
    {
        float _ratio = (float)player.Stats["Hp"].CurrentValueInt / player.Stats["Hp"].Cap.Value;

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

        playerHealthAndManaPanelAccessor.HealthText.text = player.Stats["Hp"].CurrentValueInt.ToString();
    }

    private void UpdateMana(int _)
    {
        for (int i = 0; i < player.Stats["Mp"].CurrentValueInt; ++i)
            playerHealthAndManaPanelAccessor.ManaSlots.GetChild(i).GetChild(0).gameObject.SetActive(true);

        for (int i = player.Stats["Mp"].CurrentValueInt; i < player.Stats["Mp"].Cap.Value; ++i)
            playerHealthAndManaPanelAccessor.ManaSlots.GetChild(i).GetChild(0).gameObject.SetActive(false);

        playerHealthAndManaPanelAccessor.ManaText.text = $"Mp : {player.Stats["Mp"].CurrentValue}";
    }
}