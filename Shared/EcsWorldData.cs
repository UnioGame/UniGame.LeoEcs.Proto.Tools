namespace UniGame.LeoEcs.Proto.Shared
{
    using System;
    using Core.Runtime;
    using Leopotam.EcsProto;
    using Runtime.DataFlow;

    [Serializable]
    public class EcsWorldData
    {
        public string Id;
        public ProtoWorld World;
        public ILifeTime LifeTime;
    }
}