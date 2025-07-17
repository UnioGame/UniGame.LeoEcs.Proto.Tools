namespace UniGame.LeoEcs.Bootstrap
{
    using System.Collections.Generic;
    using GameFlow.Runtime;
    using Leopotam.EcsProto;
    using Proto.Shared;

    public interface IEcsService : IGameService
    {

        ProtoWorld World { get; }

        ProtoWorld LastWorld{ get; }
        
        IReadOnlyDictionary<string,EcsWorldData> Worlds{ get; }
        
        void SetDefaultWorld(string worldId);

    }
}