namespace UniGame.LeoEcsLite.LeoEcs.ViewSystem.Systems
{
    using System;
    using Game.Modules.UnioModules.UniGame.LeoEcsLite.LeoEcs.ViewSystem.Components;
    using UniGame.LeoEcs.Bootstrap.Runtime.Attributes;
    using Leopotam.EcsProto;
    using Leopotam.EcsProto.QoL;
    using UniGame.LeoEcs.ViewSystem.Aspects;
    using UniGame.LeoEcs.ViewSystem.Components;

    /// <summary>
    /// initialize viewid component for view entity
    /// </summary>
#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [Serializable]
    [ECSDI]
    public class InitializeViewIdComponentSystem : IProtoRunSystem
    {
        private ProtoWorld _world;
        private ViewAspect _viewAspect;

        private ProtoItExc _filter = It
            .Chain<ViewComponent>()
            .Inc<ViewInitializedComponent>()
            .Exc<ViewIdComponent>()
            .End();

        public void Run()
        {
            foreach (var viewEntity in _filter)
            {
                ref var viewComponent = ref _viewAspect.View.Get(viewEntity);
                var view = viewComponent.View;
                ref var viewIdComponent = ref _viewAspect.Id.Add(viewEntity);
                viewIdComponent.Value = view.ViewIdHash;
            }
        }
    }
}