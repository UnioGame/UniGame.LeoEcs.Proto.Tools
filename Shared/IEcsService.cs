namespace UniGame.LeoEcs.Bootstrap
{
    using System.Collections.Generic;
    using System.Threading;
    using Cysharp.Threading.Tasks;
    using GameFlow.Runtime;
    using Leopotam.EcsProto;
    using Proto.Shared;
    using R3;

    public interface IEcsService : IGameService
    {
        ReadOnlyReactiveProperty<EcsWorldData> DefaultWorld { get; }
        
        ReadOnlyReactiveProperty<EcsWorldData> LastWorld { get; }

        ProtoWorld World { get; }
        
        IReadOnlyDictionary<string,EcsWorldData> Worlds{ get; }
        
        bool SetDefaultWorld(string worldId);

        UniTask<ProtoWorld> GetWorldAsync(string worldId, bool createIfNotExists = false,
            CancellationToken cancellationToken = default);

        UniTask<EcsWorldData> CreateWorldAsync(string worldId);
    }
}