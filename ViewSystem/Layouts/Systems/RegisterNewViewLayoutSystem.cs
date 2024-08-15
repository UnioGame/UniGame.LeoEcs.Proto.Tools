namespace UniGame.LeoEcs.ViewSystem.Layouts.Systems
{
    using System;
    using Aspects;
    using Components;
    using Leopotam.EcsProto;
    using Leopotam.EcsProto.QoL;
    using UniGame.LeoEcs.Shared.Extensions;
    using UniGame.LeoEcs.Bootstrap.Runtime.Attributes;
    using UniGame.ViewSystem.Runtime;

    /// <summary>
    /// register new layout into view system
    /// </summary>
#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [Serializable]
    [ECSDI]
    public class RegisterNewViewLayoutSystem : IProtoInitSystem, IProtoRunSystem
    {
        private ProtoWorld _world;
        private ViewLayoutAspect _layoutAspect;
        private IGameViewSystem _viewSystem;
        
        private ProtoIt _layoutFilter = It
            .Chain<ViewLayoutComponent>()
            .Inc<RegisterViewLayoutSelfRequest>()
            .End();
        
        public void Init(IProtoSystems systems)
        {
            _viewSystem = _world.GetGlobal<IGameViewSystem>();
        }

        public void Run()
        {
            foreach (var entity in _layoutFilter)
            {
                ref var layoutComponent = ref _layoutAspect.Layout.Get(entity);
                _viewSystem.RegisterLayout(layoutComponent.Id, layoutComponent.Layout);
            }
        }
    }
}