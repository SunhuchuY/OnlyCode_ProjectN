using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

// for Test

public class SpriteManager : MonoBehaviour
{
    [SerializeField] private SpriteAtlas skillAtlas;
    [SerializeField] private SpriteAtlas uiAtlas;

    [SerializeField] private SpriteRenderer ren1;
    [SerializeField] private SpriteRenderer ren2;


    void Start()
    {
        ren1.sprite = GetUIAtlas("i_shop");    
        ren2.sprite = GetUIAtlas("i_sleep");    
    }

    public Sprite GetSkillAtlas(string name)
    {
        Sprite sprite = skillAtlas.GetSprite(name);

        if (sprite != null)
        {
            return sprite;
        }
        else
        {
            Debug.LogError($"{name}의 스프라이트를 Skill아틀라스에서 찾지 못했으므로 Null을 리턴합니다.");
            return null;    
        }
    }

    public Sprite GetUIAtlas(string name)
    {
        Sprite sprite = uiAtlas.GetSprite(name);

        if (sprite != null)
        {
            return sprite;
        }
        else
        {
            Debug.LogError($"{name}의 스프라이트를 UI아틀라스에서 찾지 못했으므로 Null을 리턴합니다.");
            return null;
        }
    }
}
