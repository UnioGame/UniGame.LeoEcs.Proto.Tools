namespace UniGame.LeoEcsLite.LeoEcs.ViewSystem.Systems
{
    using System;
    using Leopotam.EcsProto;
    using Leopotam.EcsProto.QoL;
    using UniGame.LeoEcs.Bootstrap.Runtime.Attributes;
    using UniGame.LeoEcs.Shared.Extensions;
    using UniGame.LeoEcs.ViewSystem.Aspects;
    using UniGame.LeoEcs.ViewSystem.Components;
    using UniGame.LeoEcs.ViewSystem.Extensions;
    
    /// <summary>
    /// make request to shown view queued
    /// </summary>
#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [Serializable]
    [ECSDI]
    public class ShowQueuedViewOnSystem<TEvent, TView1, TView2> : IProtoRunSystem
        where TEvent : struct
    {
        private readonly EcsViewData _viewData;
        private ProtoWorld _world;
        private ViewAspect _viewAspect;
        
        private ProtoIt _filter = It
            .Chain<TEvent>()
            .End();

        public ShowQueuedViewOnSystem(EcsViewData viewData)
        {
            _viewData = viewData;
        }

        public void Run()
        {
            foreach (var entity in _filter)
            {
                var requestEntity = _world.NewEntity();
                ref var request = ref _viewAspect.ShowQueued.Add(requestEntity);

                request.AwaitId = 0;

                request.Value.Enqueue(EcsViewExtensions.CreateViewRequest(typeof(TView1).Name,
                    _viewData.LayoutType, _viewData.Parent, _viewData.Tag, _viewData.ViewName, _viewData.StayWorld));
                request.Value.Enqueue(EcsViewExtensions.CreateViewRequest(typeof(TView2).Name,
                    _viewData.LayoutType, _viewData.Parent, _viewData.Tag, _viewData.ViewName, _viewData.StayWorld));
            }
        }
    }
}