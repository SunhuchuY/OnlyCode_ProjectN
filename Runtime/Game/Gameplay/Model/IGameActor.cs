using UnityEngine;

public enum ActorType
{
    Player,
    Monster,
    Friend
}

public interface IGameActor
{
    public GameObject Go { get; }
    public Stats Stats { get; }
    public GameplayEffectController EffectController { get; }
    public TagController TagController { get; }
    public ActiveSkillController SkillController { get; }
    public Animator Anim { get; }
    ActorType ActorType { get; }
}