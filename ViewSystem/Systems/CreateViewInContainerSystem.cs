﻿namespace UniGame.LeoEcs.ViewSystem.Systems
{
    using System;
    using Aspects;
    using Components;
    using Leopotam.EcsLite;
    using UniGame.LeoEcs.Bootstrap.Runtime.Attributes;
    using Leopotam.EcsProto;
    using Shared.Components;
    using Shared.Extensions;

    /// <summary>
    /// listen request to create view in container and find container by id
    /// </summary>
#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [Serializable]
    [ECSDI]
    public class CreateViewInContainerSystem : IProtoRunSystem
    {
        private ProtoWorld _world;
        
        private ViewAspect _viewAspect;
        private ViewContainerAspect _viewContainerAspect;
        
        private EcsFilter _requestFilter;
        private EcsFilter _allContainersFilter;
        private EcsFilter _freeContainersFilter;

        public void Init(IProtoSystems systems)
        {
            _requestFilter = _world
                .Filter<CreateViewInContainerRequest>()
                .End();

            _allContainersFilter = _world
                .Filter<ViewContainerComponent>()
                .Inc<TransformComponent>()
                .End();
            
            _freeContainersFilter = _world
                .Filter<ViewContainerComponent>()
                .Inc<TransformComponent>()
                .Exc<ViewContainerBusyComponent>()
                .End();
        }

        public void Run()
        {
            foreach (var requestEntity in _requestFilter)
            {
                ref var request = ref _viewAspect.CreateInContainer.Get(requestEntity);
                var canUseBusyContainer = request.UseBusyContainer;
                
                var containerFilter = canUseBusyContainer 
                    ? _allContainersFilter 
                    : _freeContainersFilter;

                foreach (var containerEntity in containerFilter)
                {
                    ref var containerComponent = ref _viewContainerAspect.ContainerView.Get(containerEntity);
                    
                    //is container for target view
                    if (containerComponent.ViewId != request.View) continue;
      
                    ref var transformComponent = ref _viewAspect.Transform.Get(containerEntity);
                    
                    //create view in container
                    ref var createViewRequest = ref _viewAspect.CreateView.Add(requestEntity);
                    
                    createViewRequest.ViewId = request.View;
                    createViewRequest.ViewName = request.ViewName;
                    createViewRequest.Tag = request.Tag;
                    createViewRequest.Owner = request.Owner;
                    createViewRequest.StayWorld = request.StayWorld;
                    createViewRequest.Parent = transformComponent.Value;
                    createViewRequest.LayoutType =string.Empty;
                    createViewRequest.Target = requestEntity.PackEntity(_world);
              
                    //mark container as busy
                    _viewContainerAspect.Busy.GetOrAddComponent(containerEntity);
                    
                    //remove request only when container found
                    _viewAspect.CreateInContainer.TryRemove(requestEntity);
                    
                    break;
                }
            }
        }
    }
}