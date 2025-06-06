namespace UniGame.LeoEcs.Bootstrap.Runtime
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using Leopotam.EcsProto;
#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;
#endif
    
#if ENABLE_IL2CPP
    [Il2CppSetOption (Option.NullChecks, false)]
    [Il2CppSetOption (Option.ArrayBoundsChecks, false)]
#endif
    [Serializable]
    public class EcsFeatureSystems : IProtoSystems
    {
        public IProtoSystems protoSystems;
        public Slice<IProtoSystem> systems = new ();
        public bool initialized;

        public EcsFeatureSystems(ProtoWorld world)
        {
            protoSystems = new ProtoSystems(world);
        }
        
        public EcsFeatureSystems(IProtoSystems systems)
        {
            protoSystems = systems;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IProtoSystems AddSystem(IProtoSystem system, int weight = default)
        {
            systems.Add(system);
            return protoSystems.AddSystem(system, weight);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IProtoSystems AddService(object injectInstance, Type asType = default)
        {
            return protoSystems.AddService(injectInstance, asType);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IProtoSystems AddModule(IProtoModule module)
        {
            return protoSystems.AddModule(module);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IProtoSystems AddWorld(ProtoWorld world, string name)
        {
            return protoSystems.AddWorld(world,name);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ProtoWorld World(string worldName = default)
        {
            return protoSystems.World(worldName);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Dictionary<string, ProtoWorld> NamedWorlds()
        {
            return protoSystems.NamedWorlds();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Dictionary<Type, object> Services()
        {
            return protoSystems.Services();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Slice<IProtoSystem> Systems()
        {
            return initialized ? protoSystems.Systems() : systems;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Init()
        {
            initialized = true;
            protoSystems.Init();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Run()
        {
            protoSystems.Run();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Destroy()
        {
            protoSystems.Destroy();
        }
    }
}