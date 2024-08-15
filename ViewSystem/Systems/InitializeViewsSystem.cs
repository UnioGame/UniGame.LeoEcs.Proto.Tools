namespace UniGame.LeoEcs.ViewSystem.Systems
{
    using System;
    using Aspects;
    using Behavriour;
    using Bootstrap.Runtime.Attributes;
    using Components;
    using Cysharp.Threading.Tasks;
    using Leopotam.EcsProto;
    using Leopotam.EcsProto.QoL;
    using Shared.Extensions;

    /// <summary>
    /// System for initializing views.
    /// </summary>
#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [Serializable]
    [ECSDI]
    public class InitializeViewsSystem : IProtoInitSystem, IProtoRunSystem
    {
        private ProtoWorld _world;
        private ViewAspect _viewAspect;
        private IEcsViewTools _viewTools;

        private ProtoItExc _filter = It
            .Chain<ViewComponent>()
            .Exc<ViewInitializedComponent>()
            .End();

        public void Init(IProtoSystems systems)
        {
            _world = systems.GetWorld();
            _viewTools = _world.GetGlobal<IEcsViewTools>();
        }

        public void Run()
        {
            foreach (var entity in _filter)
            {
                _viewAspect.Initialized.Add(entity);

                ref var viewComponent = ref _viewAspect.View.Get(entity);
                var packedEntity = _world.PackEntity(entity);
                var view = viewComponent.View;
                var viewType = viewComponent.Type;
                ref var viewModelComponent = ref _viewAspect.Model.GetOrAddComponent(entity);

                if (view.IsModelAttached)
                {
                    viewModelComponent.Model = view.ViewModel;
                    continue;
                }

                _viewTools
                    .AddModelComponentAsync(_world, packedEntity, view, viewType)
                    .AttachExternalCancellation(_viewTools.LifeTime.Token)
                    .Forget();
            }
        }
    }
}