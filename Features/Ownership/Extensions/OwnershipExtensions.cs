namespace UniGame.Proto.Ownership
{
    using System.Runtime.CompilerServices;
    using UniGame.Proto.Ownership;
    using Ecs.Core.Components;
    using Game.Ecs.Core.Components;
    using Leopotam.EcsProto;
    using Leopotam.EcsProto.QoL;
    using UniCore.Runtime.ProfilerTools;
    using UniGame.LeoEcs.Shared.Extensions;

    public static class OwnershipExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddChild(this ProtoEntity owner, ProtoEntity child, ProtoWorld world)
        {
            var ownerPool = world.GetPool<OwnerComponent>();
            var ownerLinkPool = world.GetPool<OwnerLinkComponent>();
            
            ref var ownerComponent = ref ownerPool.GetOrAdd(owner);
            var packedChild = child.PackEntity(world);

            if (!ownerComponent.HasChild(packedChild))
            {
                ownerComponent.AddChild(packedChild);
            }
            
            ref var ownerLinkComponent = ref ownerLinkPool.GetOrAdd(child);
            var packedOwner = owner.PackEntity(world);
            ownerLinkComponent.Value = packedOwner;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddChild(this ProtoEntity owner, ProtoPackedEntity packedChild, ProtoWorld world)
        {
            if (!packedChild.Unpack(world, out var child))
            {
                GameLog.LogWarning("Cannot unpack child entity.");
                return;
            }

            owner.AddChild(child, world);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddChild(this ProtoPackedEntity ownerPacked, ProtoEntity child, ProtoWorld world)
        {
            if (!ownerPacked.Unpack(world, out var owner))
            {
                GameLog.LogWarning("Cannot unpack owner entity.");
                return;
            }
            
            owner.AddChild(child, world);
        }   
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddChild(this ProtoPackedEntity ownerPacked, ProtoPackedEntity packedChild, ProtoWorld world)
        {
            if (!ownerPacked.Unpack(world, out var owner))
            {
                GameLog.LogWarning("Cannot unpack owner entity.");
                return;
            }
            
            owner.AddChild(packedChild, world);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Kill(this ProtoEntity entity, ProtoWorld world)
        {
            var prepareToDeathPool = world.GetPool<PrepareToDeathComponent>();
            prepareToDeathPool.GetOrAdd(entity);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetOwner(this ProtoEntity entity, ProtoWorld world, out ProtoEntity ownerEntity)
        {
            ownerEntity = default;
            
            var ownerLinkPool = world.GetPool<OwnerLinkComponent>();
            if (!ownerLinkPool.Has(entity))
            {
                return false;
            }

            ref var ownerLinkComponent = ref ownerLinkPool.Get(entity);
            return ownerLinkComponent.Value.Unpack(world, out ownerEntity);
        }
    }
}