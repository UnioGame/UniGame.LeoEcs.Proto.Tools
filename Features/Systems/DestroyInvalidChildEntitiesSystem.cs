namespace Game.Ecs.Core.Systems
{
    using System;
    using Components;
    using Death.Aspects;
    using Leopotam.EcsProto;
    using Leopotam.EcsProto.QoL;
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
    public sealed class DestroyInvalidChildEntitiesSystem : IProtoRunSystem
    {
        private ProtoWorld _world;
        private DestroyAspect _destroyAspect;
        
        private ProtoIt _filter = It
            .Chain<OwnerDestroyedEvent>()
            .End();
        
        public void Run()
        {
            foreach (var entity in _filter)
                _destroyAspect.Kill.GetOrAddComponent(entity);
        }
    }
}