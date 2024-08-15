namespace UniGame.LeoEcs.ViewSystem.Systems
{
    using System;
    using Aspects;
    using Bootstrap.Runtime.Attributes;
    using Components;
    using Game.Modules.UnioModules.UniGame.LeoEcsLite.LeoEcs.ViewSystem.Components;
    using Layouts.Aspects;
    using Leopotam.EcsProto;
    using Leopotam.EcsProto.QoL;
    using Shared.Components;

    /// <summary>
    /// check is container state is changed to free
    /// </summary>
#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [Serializable]
    [ECSDI]
    public class UpdateViewContainerBusyStatusSystem : IProtoRunSystem
    {
        private ProtoWorld _world;

        private ViewAspect _viewAspect;
        private ViewLayoutAspect _viewLayoutAspect;
        private ViewContainerAspect _viewContainerAspect;
        
        private ProtoIt _containerFilter= It
            .Chain<ViewContainerComponent>()
            .Inc<TransformComponent>()
            .Inc<ViewContainerBusyComponent>()
            .End();
        
        private ProtoIt _parentingViewFilter= It
            .Chain<ViewParentComponent>()
            .End();

        public void Run()
        {
            foreach (var containerEntity in _containerFilter)
            {
                ref var transformComponent = ref _viewAspect.Transform.Get(containerEntity);
                var isEmpty = true;

                foreach (var parentEntity in _parentingViewFilter)
                {
                    ref var parentComponent = ref _viewLayoutAspect.Parent.Get(parentEntity);
                    if (parentComponent.Value != transformComponent.Value) continue;

                    isEmpty = false;
                    break;
                }

                if (isEmpty) _viewContainerAspect.Busy.Del(containerEntity);
            }
        }
    }
}