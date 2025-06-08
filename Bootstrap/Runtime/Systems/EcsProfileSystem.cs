namespace UniGame.LeoEcs.Bootstrap.Runtime.Systems
{
    using System;
    using System.Runtime.CompilerServices;
    using Leopotam.EcsLite;
    using Leopotam.EcsProto;
    using Shared.Extensions;
    using UniGame.LeoEcs.Bootstrap.Runtime.Attributes;
    using UniGame.Runtime.ReflectionUtils;
    using Unity.Profiling;

    /// <summary>
    /// ecs proxy profile system
    /// </summary>
#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [Serializable]
    [ECSDI]
    public class EcsProfileSystem : 
        IProtoInitSystem, 
        IProtoRunSystem,
        IEcsDestroySystem
    {
        private ProtoWorld _world;
        private IEcsSystem _system;
        private IProtoRunSystem _runSystem;
        private IEcsDestroySystem _destroySystem;
        private IProtoInitSystem _initSystem;
        private string _systemName;
        private string _profileTag;
        private ProfilerMarker _marker;
        
        public void Initialize(IEcsSystem system)
        {
            _system = system;
            
            _runSystem = system as IProtoRunSystem;
            _destroySystem = system as IEcsDestroySystem;
            _initSystem = system as IProtoInitSystem;
            
            _systemName = system.GetType().GetFormattedName();
            _profileTag = $"ECS.RUN.{_systemName}";
            _marker = new ProfilerMarker(_profileTag);
        }
        
        public void Init(IProtoSystems systems)
        {
            _world = systems.GetWorld();
            _initSystem?.Init(systems);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Run()
        {
            _marker.Begin();
            _runSystem?.Run();
            _marker.End();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Destroy()
        {
            _destroySystem?.Destroy();
        }
    }
}