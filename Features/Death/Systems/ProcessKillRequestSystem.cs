namespace Game.Ecs.Core.Death.Systems
{
    using System;
    using Aspects;
    using Components;
    using Leopotam.EcsProto;
    using Leopotam.EcsProto.QoL;
    using UniGame.LeoEcs.Bootstrap.Runtime.Attributes;
    using UniGame.LeoEcs.Shared.Extensions;

    /// <summary>
    /// System for processing kill requests.
    /// </summary>
#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [Serializable]
    [ECSDI]
    public sealed class ProcessKillRequestSystem : IProtoRunSystem
    {
        private ProtoWorld _world;
        private DestroyAspect _destroyAspect;
        
        private ProtoItExc _filter = It
            .Chain<KillSelfRequest>()
            .Exc<DestroyComponent>()
            .Exc<DontKillComponent>()
            .End();
        
        public void Run()
        {
            foreach (var entity in _filter)
            {
                ref var killRequest = ref _destroyAspect.Kill.Get(entity);

                var killEventEntity = _world.NewEntity();
                ref var killEvent = ref _destroyAspect.KillEvent.Add(killEventEntity);
                killEvent.Source = killRequest.Source;
                killEvent.Destination = _world.PackEntity(entity);

                if (_destroyAspect.Pooling.Has(entity))
                {
                    _destroyAspect.DeadEvent.TryAdd(entity);
                    continue;
                }

                _destroyAspect.Destroy.TryAdd(entity);
                _destroyAspect.DeadEvent.TryAdd(entity);
            }
        }
    }
}