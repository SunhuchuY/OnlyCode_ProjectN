public enum FriendMoveType
{
    Static,
    Chase
}

public class FriendData
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public FriendMoveType MoveType { get; set; }
    public int AttackSkillId { get; set; }
}