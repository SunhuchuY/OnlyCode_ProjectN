using UnityEngine;

public class PlayerSkillsCanUseAccessor : MonoBehaviour
{
    public SkillAccessor[] SkillAccessors => skillAccessors;
    public SkillAccessor NextSkillAccessor => nextSkillAccessor;

    [SerializeField] private SkillAccessor[] skillAccessors = new SkillAccessor[4];
    [SerializeField] private SkillAccessor nextSkillAccessor;
}