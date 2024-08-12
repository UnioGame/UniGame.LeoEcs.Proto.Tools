namespace UniGame.LeoEcs.ViewSystem.Systems
{
    using System;
    using Components;
    using Leopotam.EcsLite;
    using Leopotam.EcsProto;
    using Shared.Extensions;

#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [Serializable]
    public class CreateLayoutViewSystem : IProtoRunSystem,IProtoInitSystem
    {
        private EcsFilter _createFilter;
        private ProtoWorld _world;

        private ProtoPool<CreateLayoutViewRequest> _requestLayoutPool;
        private ProtoPool<CreateViewRequest> _requestPool;

        public void Init(IProtoSystems systems)
        {
            _world = systems.GetWorld();
            _createFilter = _world.Filter<CreateLayoutViewRequest>().End();
            
            _requestLayoutPool = _world.GetPool<CreateLayoutViewRequest>();
            _requestPool = _world.GetPool<CreateViewRequest>();
        }
        
        public void Run()
        {
            foreach (var entity in _createFilter)
            {
                ref var requestLayoutComponent = ref _requestLayoutPool.Get(entity);
                ref var requestComponent = ref _requestPool.Add(entity);

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