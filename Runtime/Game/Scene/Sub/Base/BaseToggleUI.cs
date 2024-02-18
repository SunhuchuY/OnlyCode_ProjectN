using System.Collections.Generic;
using System;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.UI;
using Unity.Linq;
using System.Linq;

public abstract class BaseToggleUI<T> : MonoBehaviour where T : Enum
{
    protected abstract Dictionary<T, (Button, RectTransform)> Dict { get; set; }
    private Dictionary<T, UnityAction> ClickActions;


    protected void InitToggleUI()
    {
        ClickActions = new Dictionary<T, UnityAction>(); 

        InitActions();
        ButtonAddListener();
    }

    private void ButtonAddListener()
    {
        foreach (var d in Dict)
        {
            d.Value.Item1.onClick.AddListener(ClickActions[d.Key]);
        }
    }

    protected void ButtonRemoveListener()
    {
        foreach (var d in Dict)
        {
            d.Value.Item1.onClick.RemoveListener(ClickActions[d.Key]);
        }
    }

    private void InitActions()
    {
        foreach (var d in Dict)
        {
            UnityAction OnClickAction = new UnityAction(() =>
            {
                DisableAllPanels();
                DisableAllButtons();
                d.Value.Item2.gameObject.SetActive(true);

                // TODO: 미리 캐싱 해둬야합니다.
                ToggleEnable(d.Value.Item1.GetComponent<RectTransform>(), true);
            });


            ClickActions.Add(d.Key, OnClickAction);
        }
    }

    protected void DisableAllPanels()
    {
        foreach (var d in Dict)
        {
            d.Value.Item2.gameObject.SetActive(false);
        }
    }

    protected void DisableAllButtons()
    {
        foreach (var d in Dict)
        {
            ToggleEnable(d.Value.Item1.GetComponent<RectTransform>(), false);
        }
    }

    private void ToggleEnable(RectTransform _button, bool _enable)
    {
        // 활성화 여부에 따라 Enabled, Disabled 오브젝트를 활성화/비활성화합니다.
        _button.gameObject.Descendants().First(x => x.name == "Enabled").SetActive(_enable);
        _button.gameObject.Descendants().First(x => x.name == "Disabled").SetActive(!_enable);
    }

    protected void OnAction(T type)
    {
        if (!ClickActions.ContainsKey(type))
        {
            Debug.LogError($"{type.ToString()}는 딕셔너리에 저장되어 있지 않은타입입니다.");
            return;
        }

        ClickActions[type].Invoke();
    }

    /// <summary>
    /// 템플릿을 보관하는 함수를 구현하세요.
    /// </summary>
    protected virtual Dictionary<T, GameObject> GetBackupTemplates(Dictionary<T, RectTransform> placeHolders)
    {
        Dictionary<T, GameObject> templetes = new Dictionary<T, GameObject>();

        foreach (var pl in placeHolders) 
        {
            templetes.Add(pl.Key, Instantiate(pl.Value.gameObject.Children().First().gameObject));
            templetes[pl.Key].hideFlags = HideFlags.HideInHierarchy;
        }

        Cleanup(placeHolders);
        return templetes;
    }

    /// <summary>
    /// 하위 PlaceHolder들을 삭제하는 함수를 구현하세요.
    /// </summary>
    private void Cleanup(Dictionary<T, RectTransform> placeHolders)
    {
        foreach (var pl in placeHolders)
        {
            pl.Value.gameObject.Children().Destroy();
        }
    } 
}