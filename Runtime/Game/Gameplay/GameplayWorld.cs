using UniRx;
using UniRx.Triggers;
using Unity.Linq;
using UnityEngine.Assertions;

public class GameplayWorld
{
    public IReadOnlyReactiveCollection<IGameActor> Actors => actors;
    public System.Action<IGameActor> OnAddActor;
    public System.Action<IGameActor> OnRemoveAction;

    private ReactiveCollection<IGameActor> actors = new();

    public GameplayWorld()
    {
        MessageBroker.Default.Receive<RequestSpawnActorEvent>().Subscribe(_event =>
        {
            SpawnActor(_event.Data);
        });

        MessageBroker.Default.Receive<RequestDespawnActorEvent>().Subscribe(_event =>
        {
            RemoveActor(_event.Actor);
        });
    }

    public IGameActor SpawnActor(GameActorData _data)
    {
        var _actor = ActorSpanwer.SpawnActor(_data);
        AddActor(_actor);
        _actor.Go.OnDestroyAsObservable()
            .Subscribe(_ => RemoveActor(_actor));
        return _actor;
    }

    public void AddActor(IGameActor _actor)
    {
        Assert.IsTrue(actors.Contains(_actor) == false);
        actors.Add(_actor);
        OnAddActor?.Invoke(_actor);
    }

    public void RemoveActor(IGameActor _actor)
    {
        Assert.IsTrue(actors.Contains(_actor));
        actors.Remove(_actor);
        OnRemoveAction?.Invoke(_actor);
    }
}