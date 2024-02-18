using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shop.Gacha.Animation
{
    public class GachaAnimationUI_GameObjects : MonoBehaviour
    {
        [SerializeField] private GameObject _gachaBox_World;
        public GameObject gachaBox_World => _gachaBox_World;

        [SerializeField] private GameObject _gachaBox_UI;
        public GameObject gachaBox_UI => _gachaBox_UI;

        [SerializeField] private GameObject _gachaList_UI;
        public GameObject gachaList_UI => _gachaList_UI;

    }
}