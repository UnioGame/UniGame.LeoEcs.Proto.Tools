namespace Game.Ecs.Core.Death.Systems
{
    using System;
    using Components;
    using Leopotam.EcsProto;
    using Leopotam.EcsProto.QoL;
    using UniGame.LeoEcs.Bootstrap.Runtime.Attributes;
    using UniGame.LeoEcs.Shared.Components;
    using UniGame.LeoEcs.Shared.Extensions;
    using Object = UnityEngine.Object;

    /// <summary>
    /// System for processes dead transform entities.
    /// </summary>
#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [Serializable]
    [ECSDI]
    public sealed class ProcessDeadTransformEntitiesSystem : IProtoRunSystem
    {
        private ProtoWorld _world;
        
        private ProtoIt _filter = It
            .Chain<DestroyComponent>()
            .Inc<TransformComponent>()
            .End();
        
        public void Run()
        {
            var transformPool = _world.GetPool<TransformComponent>();
            
            foreach (var entity in _filter)
            {
                var packedEntity = _world.PackEntity(entity);
                ref var transformComponent = ref transformPool.Get(entity);
                var transform = transformComponent.Value;
                
                if(transform && transform.gameObject)
                    Object.Destroy(transform.gameObject);
                
                if(packedEntity.Unpack(_world,out var unpackedEntity))
                    _world.DelEntity(entity);
            }
        }
    }
}