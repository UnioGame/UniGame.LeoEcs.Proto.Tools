namespace UniGame.LeoEcs.Bootstrap
{
    using System;
    using Game.Ecs.Core.Components;
    using Leopotam.EcsProto;
    using Shared.Components;

    /// <summary>
    /// physics components
    /// </summary>
#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [Serializable]
    public class PhysicsAspect : EcsAspect
    {
        public ProtoPool<ColliderComponent> Collider;
        public ProtoPool<RigidbodyComponent> Physics;
        public ProtoPool<AngularSpeedComponent> AngularSpeed;
        public ProtoPool<VelocityComponent> Velocity;
    }
}