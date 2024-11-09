namespace Game.Modules.leoecs.proto.tools.Ownership.Systems
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using Aspects;
    using Components;
    using Ecs.Core.Components;
    using Leopotam.EcsProto;
    using Leopotam.EcsProto.QoL;
    using UniGame.LeoEcs.Bootstrap.Runtime.Attributes;
    using UniGame.LeoEcs.Proto.Filters.Game.Modules.leoecs.proto.tools.LifeTime.Components;
    using UniGame.LeoEcs.Shared.Extensions;

#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [Serializable]
    [ECSDI]
    public class PrepareEntityDeathSystem : IProtoInitSystem, IProtoRunSystem
    {
        private ProtoWorld _world;

        private OwnershipAspect _ownershipAspect;

        private ProtoItExc _ownerFilter = It
            .Chain<PrepareToDeathComponent>()
            .Exc<DeleteEntityNextComponent>()
            .End();

        public void Init(IProtoSystems systems)
        {
            _world = systems.GetWorld();
        }

        public void Run()
        {
            foreach (var ownerEntity in _ownerFilter)
            {
                // Check for higher level owner
                if (_ownershipAspect.OwnerLink.Has(ownerEntity))
                {
                    ref var ownerLinkComponent = ref _ownershipAspect.OwnerLink.Get(ownerEntity);
                    if (ownerLinkComponent.Value.Unpack(_world, out var unpackedOwnerEntity))
                    {
                        ref var higherOwnerComponent = ref _ownershipAspect.Owner.Get(unpackedOwnerEntity);
                        higherOwnerComponent.RemoveChild(ownerEntity.PackEntity(_world));
                    }
                }

                ReleaseLifeTime(ownerEntity);
                // WARNING: it is possible that entity was destroyed by game object converter! (destroy on disable)
                _ownershipAspect.DeleteEntity.GetOrAdd(ownerEntity);
                OwnerComponent ownerComponent = default;
                if (!TryGetOwnerComponent(ownerEntity, ref ownerComponent))
                {
                    continue;
                }
                
                var killStack = new Stack<OwnerComponent>();
                killStack.Push(ownerComponent);
                while (killStack.TryPop(out ownerComponent))
                {
                    if (!ownerComponent.Children.IsCreated)
                    {
                        continue;
                    }
                    
                    foreach (var child in ownerComponent.Children)
                    {
                        if (!child.Unpack(_world, out var unpackedChild))
                        {
                            continue;
                        }

                        ReleaseLifeTime(unpackedChild);
                        _ownershipAspect.DeleteEntity.GetOrAdd(unpackedChild);
                        if (TryGetOwnerComponent(unpackedChild, ref ownerComponent))
                        {
                            killStack.Push(ownerComponent);
                        }
                    }
                }
                
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool TryGetOwnerComponent(ProtoEntity entity, ref OwnerComponent result)
        {
            if (!_ownershipAspect.Owner.Has(entity))
            {
                return false;
            }

            result = _ownershipAspect.Owner.Get(entity);
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ReleaseLifeTime(ProtoEntity entity)
        {
            if (!_ownershipAspect.LifeTime.Has(entity))
            {
                return;
            }

            ref var lifeTimeComponent = ref _ownershipAspect.LifeTime.Get(entity);
            lifeTimeComponent.Release();
        }
    }
}