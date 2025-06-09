namespace Game.Ecs.Core.Death.Aspects
{
    using System;
    using Components;
    using Leopotam.EcsProto;
    using UniGame.LeoEcs.Bootstrap;

    /// <summary>
    /// destroy entity aspect
    /// </summary>
#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [Serializable]
    public class DestroyAspect : EcsAspect
    {
        public ProtoPool<DestroyComponent> Destroy;
        public ProtoPool<DisabledComponent> Disabled;
        public ProtoPool<DontKillComponent> DontKill;
        public ProtoPool<CanDisableComponent> CanDisable;
        public ProtoPool<PoolingComponent> Pooling;
        
        //requests
        public ProtoPool<ValidateDeadChildEntitiesRequest> ValidateDeadChild;
        public ProtoPool<DestroySelfRequest> DestroySelf;
        public ProtoPool<KillRequest> Kill;
        
        //events
        public ProtoPool<DeadEvent> DeadEvent;
        public ProtoPool<DisabledEvent> DisabledEvent;
        public ProtoPool<KillEvent> KillEvent;
    }
}