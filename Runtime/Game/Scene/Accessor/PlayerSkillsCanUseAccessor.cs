using UnityEngine;

public class PlayerSkillsCanUseAccessor : MonoBehaviour
{
    public SkillAccessor[] SkillAccessors => skillAccessors;
    public NextSkillAccessor NextSkillAccessor => nextSkillAccessor;

    [SerializeField] private SkillAccessor[] skillAccessors = new SkillAccessor[4];
    [SerializeField] private NextSkillAccessor nextSkillAccessor;
}