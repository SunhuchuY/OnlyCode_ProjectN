using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shop.Gacha
{
    public static class CircleColorUtils
    {
        public static Color[] GetCircleColors(SkillGrade grade)
        {
            switch (grade)
            {
                case SkillGrade.R:
                    return new Color[]
                    {
                        new Color(0.11f, 0.847f, 0.961f, 1f),
                        new Color(0.11f, 0.847f, 0.902f, 1f),
                        new Color(0.11f, 0.847f, 0.843f, 1f),
                    };

                case SkillGrade.SR:
                    return new Color[]
                    {
                        new Color(0.55f, 0.03f, 0.77f, 1f),
                        new Color(0.55f, 0.03f, 0.77f, 1f),
                        new Color(0.55f, 0.03f, 0.77f, 1f),
                    };

                case SkillGrade.SSR:
                    return new Color[]
                    {
                        new Color(0.929f, 0.486f, 0.008f, 1f),
                        new Color(0.929f, 0.486f, 0.008f, 1f),
                        new Color(0.929f, 0.486f, 0.008f, 1f),
                    };

                default:
                    throw new KeyNotFoundException("The specified skill grade does not exist.");
            }
        }
    }
}
