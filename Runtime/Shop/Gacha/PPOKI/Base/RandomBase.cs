using Shop.Gacha.Animation;
using System.Collections.Generic;
using UnityEngine;

namespace Shop.Gacha
{
    [System.Serializable]
    public abstract class RandomBase : MonoBehaviour
    {
        protected int[] ids;

        public RandomBase(int[] ids)
        {
            this.ids = ids;
        }

    }
}

