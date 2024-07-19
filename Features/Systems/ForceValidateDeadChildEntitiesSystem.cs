namespace Game.Ecs.Core.Systems
{
    using System;
    using System.Collections.Generic;
    using Components;
    using Death.Aspects;
    using Death.Components;
    using Leopotam.EcsProto;
    using Leopotam.EcsProto.QoL;
    using UniGame.LeoEcs.Bootstrap.Runtime.Abstract;
    using UniGame.LeoEcs.Bootstrap.Runtime.Attributes;
    using UniGame.LeoEcs.Shared.Extensions;

#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [Serializable]
    [ECSDI]
    public sealed class ForceValidateDeadChildEntitiesSystem : IProtoRunSystem
    {
        private ProtoWorld _world;
        private DestroyAspect _destroyAspect;
        private LifeTimeAspect _lifeTimeAspect;
        
        private ProtoIt _requestFilter = It
            .Chain<ValidateDeadChildEntitiesRequest>()
            .End();
        
        private ProtoIt _filter = It
            .Chain<OwnerComponent>()
            .End();
        
        private HashSet<ProtoEntity> _destroyedEntities = new();
        private HashSet<ProtoEntity> _bufferDestroyedEntities = new();
        
        public void Run()
        {
            foreach (var requestEntity in _requestFilter)
            {
                ref var request = ref _destroyAspect.ValidateDeadChild.Get(requestEntity);
                
                _destroyedEntities.Clear();
                var foundDeadChild = false;

                foreach (var entity in _filter)
                {
                    ref var ownerComponent = ref _lifeTimeAspect.Owner.Get(entity);
                    if (ownerComponent.Value.Unpack(_world, out var ownerEntity) )
                        continue;
                    _bufferDestroyedEntities.Add(entity);
                    foundDeadChild = true;
                }
                
                if (foundDeadChild == false)
                    break;

                do
                {
                    foundDeadChild = false;
                    var buffer = _bufferDestroyedEntities;
                    _bufferDestroyedEntities = _destroyedEntities;
                    _bufferDestroyedEntities.Clear();
                    _destroyedEntities = buffer;
                    
                    foreach (var entity in _filter)
                    {
                        ref var ownerComponent = ref _lifeTimeAspect.Owner.Get(entity);
                        if(!ownerComponent.Value.Unpack(_world, out var ownerEntity))
                            continue;
                        
                        if(!_destroyedEntities.Contains(ownerEntity)) continue;

                        _bufferDestroyedEntities.Add(entity);
                        
                        ref var destroyRequest = ref _destroyAspect.DestroySelf.GetOrAddComponent(entity);
                        destroyRequest.ForceDestroy = request.ForceDestroy;
                        foundDeadChild = true;
                    }
                    
                } while (foundDeadChild);
                
                break;
            }
        }
    }
}