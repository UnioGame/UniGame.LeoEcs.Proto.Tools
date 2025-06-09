namespace UniGame.LeoEcs.ViewSystem
{
    using Components;
    using System;
    using Aspects;
    using Bootstrap.Runtime.Attributes;
    using Leopotam.EcsProto;
    using Leopotam.EcsProto.QoL;

    /// <summary>
    /// System for updating the status of the view.
    /// </summary>
#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif

    [Serializable]
    [ECSDI]
    public class ViewUpdateStatusSystem : IProtoRunSystem
    {
        private ProtoWorld _world;
        private ViewAspect _viewAspect;

        private ProtoIt _viewFilter = It
            .Chain<ViewComponent>()
            .Inc<ViewStatusComponent>()
            .End();

        public void Run()
        {
            foreach (var entity in _viewFilter)
            {
                ref var viewComponent = ref _viewAspect.View.Get(entity);
                ref var viewStatusComponent = ref _viewAspect.Status.Get(entity);

                var activeStatus = viewStatusComponent.Status;
                var view = viewComponent.View;
                viewStatusComponent.Status = view.Status.CurrentValue;

                if (activeStatus != viewStatusComponent.Status)
                    _viewAspect.StatusChanged.Add(entity);
            }
        }
    }
}