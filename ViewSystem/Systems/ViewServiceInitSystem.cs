namespace UniGame.LeoEcs.ViewSystem
{
    using System;
    using Aspects;
    using Bootstrap.Runtime.Attributes;
    using Leopotam.EcsProto;
    using Shared.Extensions;
    using UniGame.ViewSystem.Runtime;
    
    /// <summary>
    /// Initializes the ViewService.
    /// </summary>
#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [Serializable]
    [ECSDI]
    public class ViewServiceInitSystem : IProtoInitSystem
    {
        private ProtoWorld _world;
        private ViewSystemAspect _viewSystemAspect;
        private IGameViewSystem _gameViewSystem;
        
        public void Init(IProtoSystems systems)
        {
            _world = systems.GetWorld();
            _gameViewSystem = _world.GetGlobal<IGameViewSystem>();
            
            var entity = _world.NewEntity();
            ref var component = ref _viewSystemAspect.ViewService.Add(entity);
            component.ViewSystem = _gameViewSystem;
        }
    }
}
