using UniRx;
using Unity.Linq;
using UnityEngine.Assertions;

public class GameplayWorld
{
    public IReadOnlyReactiveCollection<IGameActor> Actors => actors;
    public System.Action<IGameActor> OnAddActor;
    public System.Action<IGameActor> OnDespawnAction;

    private ReactiveCollection<IGameActor> actors = new();

    public GameplayWorld()
    {
        MessageBroker.Default.Receive<RequestSpawnActorEvent>().Subscribe(_event =>
        {
            SpawnActor(_event.Data);
        });

        MessageBroker.Default.Receive<RequestDespawnActorEvent>().Subscribe(_event =>
        {
            DespawnActor(_event.Actor);
        });
    }

    public IGameActor SpawnActor(GameActorData _data)
    {
        // todo
        AddActor(null);
        return null;
    }

    public void AddActor(IGameActor _actor)
    {
        Assert.IsTrue(actors.Contains(_actor) == false);
        actors.Add(_actor);
        OnAddActor?.Invoke(_actor);
    }

    public void DespawnActor(IGameActor _actor)
    {
        Assert.IsTrue(actors.Contains(_actor));
        actors.Remove(_actor);
        OnDespawnAction?.Invoke(_actor);
        _actor.Go.Destroy();
    }
}