namespace Game.Ecs.Core.Timer.Systems
{
    using System;
    using Components;
    using Leopotam.EcsProto;
    using Leopotam.EcsProto.QoL;
    using UniGame.Proto.Ownership;
    using UniGame.LeoEcs.Bootstrap.Runtime.Attributes;
    using UniGame.LeoEcs.Shared.Extensions;
    using UniGame.LeoEcs.Timer.Components;
    using UniGame.LeoEcs.Timer.Components.Events;

#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [Serializable]
    [ECSDI]
    public class DestroyByCooldownSystem : IProtoInitSystem, IProtoRunSystem
    {
        private ProtoWorld _world;

        private ProtoItExc _destroyByCooldownFilter = It
            .Chain<DestroyByCooldownComponent>()
            .Inc<CooldownCompleteComponent>()
            .Exc<PrepareToDeathComponent>()
            .End();

        public void Init(IProtoSystems systems)
        {
            _world = systems.GetWorld();
        }

        public void Run()
        {
            foreach (var entity in _destroyByCooldownFilter)
            {
                entity.Kill(_world);
            }
        }
    }
}