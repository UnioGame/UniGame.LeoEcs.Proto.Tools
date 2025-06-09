namespace Game.Ecs.Core.Death.Systems
{
    using System;
    using Aspects;
    using Components;
    using Leopotam.EcsProto;
    using Leopotam.EcsProto.QoL;
    using UniGame.LeoEcs.Bootstrap;
    using UniGame.LeoEcs.Bootstrap.Runtime.Attributes;
    using UniGame.LeoEcs.Shared.Extensions;
    using UniGame.Runtime.ObjectPool.Extensions;
    using UnityEngine;

    /// <summary>
    /// System for despawning killed entities.
    /// </summary>
#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [Serializable]
    [ECSDI]
    public sealed class ProcessDespawnSystem : IProtoRunSystem
    {
        private ProtoWorld _world;
        private DestroyAspect _destroyAspect;
        private UnityAspect _unityAspect;
        
        private ProtoIt _filter = It
            .Chain<KillEvent>()
            .End();
        
        public void Run()
        {
            foreach (var killEventEntity in _filter)
            {
                ref var killEvent = ref _world
                    .GetComponent<KillEvent>(killEventEntity);
                
                if(!killEvent.Destination.Unpack(_world,out var killedEntity))
                    continue;
                
                if(!_destroyAspect.Pooling.Has(killedEntity) || _destroyAspect.DontKill.Has(killedEntity))
                   continue;

                var isTransform = _unityAspect.Transform.Has(killedEntity);
                var isGameObject = _unityAspect.GameObject.Has(killedEntity);
                
                GameObject gameObject = null;

                if (isGameObject)
                {
                    ref var gameObjectComponent = ref _unityAspect.GameObject.Get(killedEntity);
                    gameObject = gameObjectComponent.Value;
                }
                else if (isTransform)
                {
                    ref var transformComponent = ref _unityAspect.Transform.Get(killedEntity);
                    var transform = transformComponent.Value;
                    gameObject = transform?.gameObject;
                }
                
                _world.DelEntity(killedEntity);

                if(gameObject != null)
                    gameObject.Despawn();
            }
        }
    }
}