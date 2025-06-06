namespace Game.Ecs.Core.Death.Systems
{
    using System;
    using Aspects;
    using Components;
    using Leopotam.EcsProto;
    using Leopotam.EcsProto.QoL;
    using UniGame.LeoEcs.Bootstrap.Runtime.Abstract;
    using UniGame.LeoEcs.Bootstrap.Runtime.Attributes;
    using UniGame.LeoEcs.Shared.Extensions;
    using UnityEngine;
    using Object = UnityEngine.Object;

    /// <summary>
    /// System for destroying entities in a silent manner.
    /// </summary>
#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [Serializable]
    [ECSDI]
    public sealed class ProcessDestroySilentSystem : IProtoRunSystem
    {
        private ProtoWorld _world;
        private DestroyAspect _destroyAspect;
        private UnityAspect _unityAspect;
        
        private ProtoIt _requestFilter = It
            .Chain<DestroySelfRequest>()
            .End();

        public void Run()
        {
            foreach (var entity in _requestFilter)
            {
                var packedEntity = _world.PackEntity(entity);
                
                if(!packedEntity.Unpack(_world,out var _)) continue;
                
                var isTransform = _unityAspect.Transform.Has(entity);
                var isGameObject = _unityAspect.GameObject.Has(entity);
                
                GameObject gameObject = null;
                
                if (isGameObject)
                {
                    ref var gameObjectComponent = ref _unityAspect.GameObject.Get(entity);
                    gameObject = gameObjectComponent.Value;
                }
                else if (isTransform)
                {
                    ref var transformComponent = ref _unityAspect.Transform.Get(entity);
                    var transform = transformComponent.Value;
                    gameObject = transform?.gameObject;
                }
                
                if (gameObject == null)
                {
                    _world.DelEntity(entity);
                    continue;
                }
                
                _world.DelEntity(entity);
                gameObject.SetActive(false);
                Object.Destroy(gameObject);
            }
        }
    }
}