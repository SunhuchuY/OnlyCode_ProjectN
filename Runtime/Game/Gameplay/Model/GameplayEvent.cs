public class RequestSpawnActorEvent
{
    public GameActorData Data { get; set; }
}

public class RequestDespawnActorEvent
{
    public IGameActor Actor { get; set; }
}