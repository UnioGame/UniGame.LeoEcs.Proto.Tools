namespace UniGame.LeoEcs.ViewSystem.Systems
{
    using System;
    using Aspects;
    using Bootstrap.Runtime.Attributes;
    using Components;
    using Leopotam.EcsProto;
    using Leopotam.EcsProto.QoL;

    /// <summary>
    /// System fore creates a layout view based on a request.
    /// </summary>
#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif

    [Serializable]
    [ECSDI]
    public class CreateLayoutViewSystem : IProtoRunSystem
    {
        private ProtoWorld _world;
        private ViewAspect _viewAspect;

        private ProtoIt _createFilter = It
            .Chain<CreateLayoutViewRequest>()
            .End();

        public void Run()
        {
            foreach (var entity in _createFilter)
            {
                ref var requestLayoutComponent = ref _viewAspect.CreateLayoutView.Get(entity);
                ref var requestComponent = ref _viewAspect.CreateView.Add(entity);

                requestComponent.Parent = null;
                requestComponent.Tag = string.Empty;
                requestComponent.ViewName = string.Empty;
                requestComponent.ViewId = requestLayoutComponent.View;
                requestComponent.LayoutType = requestLayoutComponent.LayoutType;
                requestComponent.StayWorld = false;
                requestComponent.Owner = requestLayoutComponent.Owner;
            }
        }
    }
}