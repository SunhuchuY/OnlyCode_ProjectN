using System.Collections;
using UnityEngine;
using UniRx;
public class HealthBar : MonoBehaviour
{
    private const float SHOW_DURATION = 2f;

    [SerializeField] private HealthBarAccessor accessor;

    private Coroutine showHpBarCoroutine;
    private Stats stats;

    private IEnumerator Start()
    {
        yield return null;

        accessor.Ratio = stats["Hp"].CurrentValue / stats["Hp"].Cap.Value;
        accessor.gameObject.SetActive(false);

        stats["Hp"].OnChangesCurrentValueAsObservable
            .Subscribe(_ =>
            {
                accessor.Ratio = stats["Hp"].CurrentValue / stats["Hp"].Cap.Value;
                ShowHpBar();
            })
            .AddTo(gameObject);

        stats["Hp"].Cap
            .Subscribe(_ =>
            {
                accessor.Ratio = stats["Hp"].CurrentValue / stats["Hp"].Cap.Value;
                ShowHpBar();
            })
            .AddTo(gameObject);
    }

    private void ShowHpBar()
    {
        if (showHpBarCoroutine != null)
            StopCoroutine(showHpBarCoroutine);

        accessor.gameObject.SetActive(true);
        showHpBarCoroutine = StartCoroutine(ShowHpBar_Coroutine());
    }

    private IEnumerator ShowHpBar_Coroutine()
    {
        yield return new WaitForSeconds(SHOW_DURATION);
        accessor.gameObject.SetActive(false);
    }

    public void SetStats(Stats stats)
    {
        this.stats = stats;
    }

    public void SetFillImageColor(Color color)
    {
        accessor.SetFillImageColor(color);
    }

}