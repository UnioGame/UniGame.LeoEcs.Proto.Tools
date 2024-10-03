namespace Game.Modules.leoecs.proto.tools.Ownership.Aspects
{
    using System;
    using Components;
    using Ecs.Core.Components;
    using Leopotam.EcsProto;
    using Leopotam.EcsProto.QoL;
    using UniCore.Runtime.ProfilerTools;
    using UniGame.LeoEcs.Bootstrap.Runtime.Abstract;
    using UniGame.LeoEcs.Proto.Filters.Game.Modules.leoecs.proto.tools.LifeTime.Components;
    using UniGame.LeoEcs.Shared.Extensions;
    using UniGame.LeoEcsLite.LeoEcs.Shared.Components;

#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [Serializable]
    public class OwnershipAspect : EcsAspect
    {
        public ProtoPool<OwnerComponent> Owner;
        public ProtoPool<OwnerLinkComponent> OwnerLink;
        
        public ProtoPool<LifeTimeComponent> LifeTime;
        public ProtoPool<PrepareToDeathComponent> PrepareToDeath;
        public ProtoPool<DeleteEntityNextComponent> DeleteEntity;
        
        // Events
        public ProtoPool<OwnerDestroyedEvent> OwnerDestroyed;
        
        public void AddChild(ProtoEntity owner, ProtoEntity child)
        {
            ref var ownerComponent = ref Owner.GetOrAdd(owner);
            var packedChild = child.PackEntity(world);

            if (!ownerComponent.HasChild(packedChild))
            {
                ownerComponent.AddChild(packedChild);
            }
            
            ref var ownerLinkComponent = ref OwnerLink.GetOrAdd(child);
            var packedOwner = owner.PackEntity(world);
            ownerLinkComponent.Value = packedOwner;
        }
        
        public void AddChild(ref ProtoEntity owner, ref ProtoPackedEntity packedChild)
        {
            if (!packedChild.Unpack(world, out var child))
            {
                GameLog.LogWarning("Cannot unpack child entity.");
                return;
            }

            AddChild(owner, child);
        }
        
        public void AddChild(ref ProtoPackedEntity ownerPacked, ref ProtoEntity child)
        {
            if (!ownerPacked.Unpack(world, out var owner))
            {
                GameLog.LogWarning("Cannot unpack owner entity.");
                return;
            }
            
            AddChild(owner, child);
        }   
        
        public void AddChild(ref ProtoPackedEntity ownerPacked, ref ProtoPackedEntity packedChild)
        {
            if (!ownerPacked.Unpack(world, out var owner))
            {
                GameLog.LogWarning("Cannot unpack owner entity.");
                return;
            }
            
            AddChild(ref owner, ref packedChild);
        }
        
        public void Kill(ProtoEntity entity)
        {
            PrepareToDeath.GetOrAdd(entity);
        }
        
        public bool TryGetOwner(ProtoEntity entity, out ProtoEntity ownerEntity)
        {
            if (OwnerLink.Has(entity))
            {
                ref var ownerLinkComponent = ref OwnerLink.Get(entity);
                return ownerLinkComponent.Value.Unpack(world, out ownerEntity);
            }
            
            ownerEntity = default;
            return false;
        }
    }
}