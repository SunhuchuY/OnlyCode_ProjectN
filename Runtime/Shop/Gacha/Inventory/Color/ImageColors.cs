using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shop.Gacha
{
    public static class ImageColors
    {
        public static Color GetColor(SkillGrade rank)
        {
            switch (rank)
            {
                case SkillGrade.R:
                    return new Color(0.11f, 0.847f, 0.961f, 1f);

                case SkillGrade.SR:
                    return new Color(0.55f, 0.03f, 0.77f, 1f);

                case SkillGrade.SSR:
                    return new Color(0.929f, 0.486f, 0.008f, 1f);

                default:
                    throw new NotSupportedException();
            }
        }
    }
}