using UnityEngine;

public enum FriendMoveType
{
    Static,
    Chase
}

public class FriendData : GameActorData
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public FriendMoveType MoveType { get; set; }
    public int AttackSkillId { get; set; }
    public string HitPivot { get; set; }
    public string DamagePivot { get; set; }
}