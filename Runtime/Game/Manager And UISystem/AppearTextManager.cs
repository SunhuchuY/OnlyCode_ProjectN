using DG.Tweening;
using UnityEngine;
using UnityEngine.Pool;

using Vector2 = UnityEngine.Vector2;

public class AppearTextManager : MonoBehaviour
{
    const string PREFAB_PATH = "Prefab/Game/Appear Text";

    [SerializeField] private RectTransform parent;
    private IObjectPool<AppearTextAccessor> objectPool;

    private void Awake()
    {
        objectPool = new ObjectPool<AppearTextAccessor>(
            () =>
            {
                var ac = Object.Instantiate(Resources.Load<GameObject>(PREFAB_PATH), parent)
                    .GetComponent<AppearTextAccessor>();
                return ac;
            },
            ac =>
            {
                ac.gameObject.SetActive(true);
            },
            ac => ac.gameObject.SetActive(false));
    }

    public void PlayerAppearText(string text, Vector2 startPos)
    {
        var ac = objectPool.Get();

        Sequence sequence = DOTween.Sequence();
        sequence.Join(ac.Text.DOFade(0f, 1f).From(1f));
        sequence.Join(ac.transform.DOMoveY(startPos.y + 1.5f, 1f).From(startPos.y + 0.5f).SetEase(Ease.InQuad));

        sequence.OnStart(() =>
        {
            ac.Text.text = text;
            ac.Text.color = Color.red;
            ac.transform.position = startPos;
        });

        sequence.OnComplete(() =>
        {
            objectPool.Release(ac);
        });

        sequence.Play();
    }

    public void MonsterAppearText(string text, Vector2 startPos)
    {
        var ac = objectPool.Get();

        Sequence sequence = DOTween.Sequence();
        sequence.Join(ac.Text.DOFade(0f, 1f).From(1f));
        sequence.Join(ac.transform.DOMoveY(startPos.y + 1.5f, 1f).From(startPos.y + 0.5f).SetEase(Ease.InQuad)); 

        sequence.OnStart(() =>
        {
            ac.Text.text = text;
            ac.Text.color = Color.white;
            ac.transform.position = startPos;   
        });
        
        sequence.OnComplete(() => 
        {
            objectPool.Release(ac);
        });

        sequence.Play();
    }
}