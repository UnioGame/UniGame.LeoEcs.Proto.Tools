namespace Game.Ecs.Core.Systems
{
    using System;
    using Components;
    using Leopotam.EcsProto;
    using Leopotam.EcsProto.QoL;
    using UniGame.LeoEcs.Bootstrap.Runtime.Abstract;
    using UniGame.LeoEcs.Bootstrap.Runtime.Attributes;

#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [Serializable]
    [ECSDI]
    public sealed class CheckInvalidChildEntitiesSystem : IProtoRunSystem
    {
        private ProtoWorld _world;
        private LifeTimeAspect _lifeTimeAspect;
        
        private ProtoIt _filter = It
            .Chain<OwnerComponent>()
            .End();
        
        public void Run()
        {
            
            foreach (var entity in _filter)
            {
                ref var ownerComponent = ref _lifeTimeAspect.Owner.Get(entity);
                if(ownerComponent.Value.Unpack(_world, out _))
                    continue;

                _lifeTimeAspect.OwnerDestroyedEvent.Add(entity);
            }
        }
    }
}