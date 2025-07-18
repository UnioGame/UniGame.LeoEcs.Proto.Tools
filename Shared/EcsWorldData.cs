namespace UniGame.LeoEcs.Proto.Shared
{
    using System;
    using System.Collections.Generic;
    using Bootstrap;
    using Bootstrap.Runtime;
    using Core.Runtime;
    using Leopotam.EcsProto;

    [Serializable]
    public class EcsWorldData : IDisposable
    {
        public string Id;
        public ProtoWorld World;
        public ILifeTime LifeTime;
        public bool IsInitialized;
        
        public Dictionary<string, EcsFeatureSystems> SystemsMap = new(8);
        public Dictionary<string, IEcsExecutor> Executors = new(8);
        public List<ISystemsPlugin> SystemPlugins = new();
        
        public void Dispose()
        {
            foreach (var systems in SystemsMap.Values)
                systems.Destroy();

            foreach (var ecsExecutor in Executors.Values)
                ecsExecutor.Dispose();

            SystemsMap.Clear();
            Executors.Clear();

            World?.Destroy();
            World = null;
            Id = null;
            LifeTime = null;
        }
    }
}