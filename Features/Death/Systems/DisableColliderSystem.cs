namespace Game.Ecs.Core.Death.Systems
{
    using System;
    using Components;
    using Leopotam.EcsProto;
    using Leopotam.EcsProto.QoL;
    using UniGame.LeoEcs.Bootstrap;
    using UniGame.LeoEcs.Bootstrap.Runtime.Attributes;
    using UniGame.LeoEcs.Shared.Components;

    /// <summary>
    /// System for disabling colliders on entities.
    /// </summary>
#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [Serializable]
    [ECSDI]
    public sealed class DisableColliderSystem : IProtoRunSystem
    {
        private ProtoWorld _world;
        private PhysicsAspect _physicsAspect;
        
        private ProtoIt _filter = It
            .Chain<ColliderComponent>()
            .Inc<DisabledEvent>()
            .End();
        
        public void Run()
        {
            foreach (var entity in _filter)
            {
                ref var collider = ref _physicsAspect.Collider.Get(entity);
                collider.Value.enabled = false;
            }
        }
    }
}