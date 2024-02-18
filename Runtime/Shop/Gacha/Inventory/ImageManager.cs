using Newtonsoft.Json.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

namespace Shop.Gacha
{
    public class ImageManager : MonoBehaviour
    {
        [SerializeField] private Transform imageParent;
        [SerializeField] private GachaListButtons.ImagePrefab imagePrefab;

        [SerializeField] private SpriteAtlas skillIconAtlas;
        [SerializeField] private GridLayoutGroup gridLayoutGroup;

        private GachaListButtons.ImagePrefab[] prefabs = new GachaListButtons.ImagePrefab[30];

        private void Awake()
        {
            // 미리 이미지들을 레퍼런싱합니다.
            for (int i = 0; i < prefabs.Length; i++)
            {
                prefabs[i] = Instantiate(imagePrefab, imageParent).GetComponent<GachaListButtons.ImagePrefab>();
                prefabs[i].Initialize();
            }
        }

        public void InitImage(List<ActiveSkillData> skillList)
        {
            OffAllImage();

            for (int i = 0; i < skillList.Count; i++)
            {
                Sprite sprite = skillIconAtlas.GetSprite(skillList[i].Name);
                
                prefabs[i].gameObject.SetActive(true);
                prefabs[i].skillImage.sprite = sprite;
                prefabs[i].SetOverrideImageColor(ImageColors.GetColor(skillList[i].Grade));
            }

            LayoutSetting(skillList);
        }

        private void OffAllImage()
        {
            foreach (var p in prefabs)
            {
                p.gameObject.SetActive(false);
            }
        }

        public void AllOpen()
        {
            foreach (var p in prefabs)
            {
                if (p.gameObject.activeSelf && !p.isAleadyOpen)
                {   
                    p.Open();
                }
            }
        }

        private void LayoutSetting(List<ActiveSkillData> skillList)
        {
            // execution: layout setting
            gridLayoutGroup.enabled = true;

            // in case: Gacha 1
            if (skillList.Count == 1)
            {
                gridLayoutGroup.childAlignment = TextAnchor.MiddleCenter;
                gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
                gridLayoutGroup.constraintCount = 1;
            }
            // in case: Gacha 12
            else if (skillList.Count == 12)
            {
                gridLayoutGroup.childAlignment = TextAnchor.MiddleCenter;
                gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
                gridLayoutGroup.constraintCount = 4;
            }
            // in case: Gacha 30
            else
            {
                gridLayoutGroup.childAlignment = TextAnchor.MiddleCenter;
                gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
                gridLayoutGroup.constraintCount = 5;
            }
        }
    }
}
