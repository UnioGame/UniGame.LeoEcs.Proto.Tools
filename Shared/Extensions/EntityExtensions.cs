namespace UniGame.LeoEcs.Shared.Extensions
{
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using Leopotam.EcsProto;
    using Leopotam.EcsProto.QoL;
    using UniGame.Core.Runtime;
    using Abstract;
    using UnityEngine;

#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;
#endif
    
    public static class EntityExtensions
    {
        public readonly static ProtoEntity InvalidEntity = (ProtoEntity)(-1);
        
        private static List<IInitializeWithEntityData> InitializableComponents = new List<IInitializeWithEntityData>();

#if ENABLE_IL2CPP
        [Il2CppSetOption (Option.NullChecks, false)]
        [Il2CppSetOption (Option.ArrayBoundsChecks, false)]
#endif
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ProtoEntity GetInvalidEntity(this ProtoEntity entity)
        {
            return InvalidEntity;
        }
        
#if ENABLE_IL2CPP
        [Il2CppSetOption (Option.NullChecks, false)]
        [Il2CppSetOption (Option.ArrayBoundsChecks, false)]
#endif
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ILifeTime DestroyEntityWith(this ILifeTime lifeTime, int entity, ProtoWorld world)
        {
            if (entity < 0) return lifeTime;
            return lifeTime.DestroyEntityWith((ProtoEntity)entity, world);
        }
        
#if ENABLE_IL2CPP
        [Il2CppSetOption (Option.NullChecks, false)]
        [Il2CppSetOption (Option.ArrayBoundsChecks, false)]
#endif
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ILifeTime DestroyEntityWith(this ILifeTime lifeTime, ProtoEntity entity, ProtoWorld world)
        {
            if (world.IsAlive() == false) return lifeTime;
            
            lifeTime.AddCleanUpAction(() =>
            {
                if (!world.IsAlive()) return;
                
                var packedEntity = world.PackEntity(entity);
                if (!packedEntity.Unpack(world, out var unpacked)) return;
                
                world.DelEntity(entity);
            });
            
            return lifeTime;
        }

#if ENABLE_IL2CPP
        [Il2CppSetOption (Option.NullChecks, false)]
        [Il2CppSetOption (Option.ArrayBoundsChecks, false)]
#endif
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Object InitializeWithEcsData(this Object target, ProtoWorld world, int entity)
        {
            switch (target)
            {
                case IInitializeWithEntityData initializable:
                    initializable.InitializeEcsData(world, entity);
                    break;
                case GameObject gameObject:
                {
                    InitializableComponents.Clear();
                    gameObject.GetComponents(InitializableComponents);

                    foreach (var initializableComponent in InitializableComponents)
                        initializableComponent.InitializeEcsData(world, entity);

                    InitializableComponents.Clear();
                    
                    
                    
                    break;
                }
            }

            return target;
        }
    }
}