namespace UniGame.LeoEcs.ViewSystem
{
    using System;
    using Aspects;
    using Bootstrap.Runtime.Attributes;
    using Components;
    using Leopotam.EcsProto;
    using Leopotam.EcsProto.QoL;
    using Shared.Extensions;

    /// <summary>
    /// Initializes the model of views system.
    /// </summary>
#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [Serializable]
    [ECSDI]
    public class InitializeModelOfViewsSystem : IProtoRunSystem
    {
        private ProtoWorld _world;
        private ViewAspect _viewAspect;

        private ProtoItExc _filter = It
            .Chain<ViewComponent>()
            .Exc<ViewInitializedComponent>()
            .End();

        public void Run()
        {
            foreach (var entity in _filter)
            {
                ref var viewComponent = ref _viewAspect.View.Get(entity);
                var view = viewComponent.View;

                if (view.ViewModel == null) continue;

                ref var viewModelComponent = ref _viewAspect.Model.GetOrAdd(entity);
                viewModelComponent.Model = view.ViewModel;

                _viewAspect.Initialized.GetOrAddComponent(entity);
            }
        }
    }
}