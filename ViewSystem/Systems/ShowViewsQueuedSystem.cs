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
    /// show view queued one by one
    /// </summary>
#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [Serializable]
    [ECSDI]
    public class ShowViewsQueuedSystem : IProtoRunSystem
    {
        private ProtoWorld _world;
        private ViewAspect _viewAspect;
        
        private ProtoIt _viewFilter = It
            .Chain<ViewIdComponent>()
            .End();
        
        private ProtoIt _viewQueuedFilter = It
            .Chain<ShowQueuedRequest>()
            .End();

        public void Run()
        {
            foreach (var requestEntity in _viewQueuedFilter)
            {
                ref var queuedRequest = ref _viewAspect.ShowQueued.Get(requestEntity);

                if (queuedRequest.Value.Count == 0)
                {
                    _viewAspect.ShowQueued.Del(requestEntity);
                    continue;
                }

                var activateNext = queuedRequest.AwaitId == 0;

                foreach (var viewEntity in _viewFilter)
                {
                    if (activateNext) break;
                    ref var viewIdComponent = ref _viewAspect.Id.Get(viewEntity);
                    activateNext = viewIdComponent.Value == queuedRequest.AwaitId;
                }

                if (!activateNext) continue;

                var viewRequestEntity = _world.NewEntity();
                var nextRequest = queuedRequest.Value.Dequeue();
                queuedRequest.AwaitId = nextRequest.ViewId.GetHashCode();

                ref var viewRequest = ref _viewAspect.CreateView.Add(viewRequestEntity);
                viewRequest.ViewId = nextRequest.ViewId;
                viewRequest.ViewName = nextRequest.ViewName;
                viewRequest.Owner = nextRequest.Owner;
                viewRequest.Parent = nextRequest.Parent;
                viewRequest.Tag = nextRequest.Tag;
                viewRequest.Target = nextRequest.Target;
                viewRequest.LayoutType = nextRequest.LayoutType;
                viewRequest.StayWorld = nextRequest.StayWorld;
            }
        }
    }
}