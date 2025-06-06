namespace Game.Ecs.Core.Systems
{
    using System;
    using Leopotam.EcsLite;
    using Leopotam.EcsProto;
    using UniGame.LeoEcs.Shared.Extensions;
    using UniGame.LeoEcs.Shared.Components;

    
    /// <summary>
    /// Add an empty target to an ability
    /// </summary>
#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [Serializable]
    public class DestroyNullTransformSystem : IProtoInitSystem, IProtoRunSystem
    {
        private ProtoWorld _world;
        private ProtoIt _transformFilter;
        private ProtoPool<TransformComponent> _transformPool;

        public void Init(IProtoSystems systems)
        {
            _world = systems.World();

            _transformFilter = _world
                .Filter<TransformComponent>()
                //.Inc<ObjectConverterComponent>()
                .End();

            _transformPool = _world.GetPool<TransformComponent>();
        }

        public void Run()
        {
            foreach (var transformEntity in _transformFilter)
            {
                ref var transformComponent = ref _transformPool.Get(transformEntity);
                var transform = transformComponent.Value;
                if (transform)
                {
                    continue;
                }
                
                _world.DelEntity(transformEntity);
            }
        }
    }
}