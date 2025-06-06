namespace UniGame.LeoEcsLite.LeoEcs.Shared.Systems
{
    using Leopotam.EcsLite;
    using Leopotam.EcsProto;
    using Leopotam.EcsProto.QoL;
    using UniGame.LeoEcs.Shared.Extensions;

    public class SingleValueSystem<TComponent> : IProtoInitSystem,IProtoRunSystem 
        where TComponent : struct
    {
        private ProtoIt _filter;
        private ProtoPool<TComponent> _pool;
        private ProtoWorld _world;

        public void Init(IProtoSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world.Filter<TComponent>().End();
            _pool = _world.GetPool<TComponent>();
        }
        
        public void Run()
        {
            foreach (var entity in _filter)
            {
                ref var component = ref _pool.Get(entity);
                var packed = _world.PackEntity(entity);
            }
        }
    }
}
