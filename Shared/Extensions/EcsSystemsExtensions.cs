using UniGame.LeoEcs.Shared.Systems;

namespace UniGame.LeoEcs.Shared.Extensions
{
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using Ecs.Shared;
    using Leopotam.EcsLite;
    using Leopotam.EcsProto;
    using Leopotam.EcsProto.QoL;

#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    public static class LeoEcsExtensions
    {
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TService GetService<TService>(this IProtoSystems systems) where TService : class
        {
            var services = systems.Services();
            if(services.TryGetValue(typeof(TService),out var service))
                return service as TService;
            return default;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TService AddService<TService>(this IProtoSystems systems, TService service) where TService : class
        {
            systems.Services()[typeof(TService)] = service;
            return service;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TService GetShared<TService>(this IProtoSystems systems) where TService : class
        {
            return GetService<TService>(systems);
        }
            
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IProtoSystems CreateOn<TTrigger,TTarget>(this IProtoSystems systems)
            where TTrigger : struct
            where TTarget : struct
        {
            systems.AddSystem(new SelfConvertComponentSystem<TTrigger,TTarget>());
            return systems;
        }
        
        public static IProtoSystems FireOn<TFilter,TComponent>(this IProtoSystems systems)
            where TFilter : struct
            where TComponent : struct
        {
            systems.AddSystem(new FireOnSystem<TFilter,TComponent>());
            return systems;
        }
        
        public static IProtoSystems FireOn<TFilter1,TFilter2,TComponent>(this IProtoSystems systems)
            where TFilter1 : struct
            where TFilter2 : struct
            where TComponent : struct
        {
            systems.AddSystem(new FireOnSystem<TFilter1,TFilter2,TComponent>());
            return systems;
        }
        
        public static void FireOn<TFilter1,TFilter2,TFilter3,TComponent>(this IProtoSystems systems)
            where TFilter1 : struct
            where TFilter2 : struct
            where TFilter3 : struct
            where TComponent : struct
        {
            systems.AddSystem(new FireOnSystem<TFilter1,TFilter2,TFilter3,TComponent>());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool EntityHasAll<T1, T2>(this ProtoWorld world,int entity)
            where T1 : struct
            where T2 : struct
        {
            return EntityHasAll<T1, T2>(world, (ProtoEntity)entity);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool EntityHasAll<T1, T2>(this ProtoWorld world,ProtoEntity entity)
            where T1 : struct
            where T2 : struct
        {
            var pool1 = world.Pool<T1>();
            var pool2 = world.Pool<T2>();
            return pool1.Has(entity) && pool2.Has(entity);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool EntityHasAll<T1, T2,T3>(this ProtoWorld world,ProtoEntity entity)
            where T1 : struct
            where T2 : struct
            where T3 : struct
        {
            var pool1 = world.Pool<T1>();
            var pool2 = world.Pool<T2>();
            var pool3 =  world.Pool<T3>();

            return pool1.Has(entity) && pool2.Has(entity) && pool3.Has(entity);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool EntityHasAll<T1, T2,T3,T4>(this ProtoWorld world,ProtoEntity entity)
            where T1 : struct
            where T2 : struct
            where T3 : struct
            where T4 : struct
        {
            var pool1 = world.Pool<T1>();
            var pool2 = world.Pool<T2>();
            var pool3 =  world.Pool<T3>();
            var pool4 =  world.Pool<T4>();

            return pool1.Has(entity) && pool2.Has(entity) && pool3.Has(entity) && pool4.Has(entity);
        }
        
        public static ProtoIt GetFilter<TComponent>(this IProtoSystems ecsSystems)
            where TComponent : struct
        {
            var world = ecsSystems.GetWorld();
            var filter = world.Filter<TComponent>().End();

            return filter;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ProtoPool<TComponent> GetPool<TComponent>(this IProtoSystems ecsSystems)
            where TComponent : struct
        {
            var world = ecsSystems.GetWorld();
            var pool = world.Pool<TComponent>();

            return pool as ProtoPool<TComponent>;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryRemoveComponent<TComponent>(this IProtoSystems systems, ProtoEntity entity)
            where TComponent : struct
        {
            var world = systems.GetWorld();
            return world.TryRemoveComponent<TComponent>(entity);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryRemoveComponent<TComponent>(this IProtoSystems systems, int entity)
            where TComponent : struct
        {
            var entityProto = (ProtoEntity)entity;
            return TryRemoveComponent<TComponent>(systems, entityProto);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref TComponent GetComponent<TComponent>(this IProtoSystems systems, ProtoEntity entity)
            where TComponent : struct
        {
            var pool = systems.GetPool<TComponent>();
            return ref pool.Get(entity);
        }
            
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IProtoSystems Add(this IProtoSystems systems, IProtoSystem system)
        {
            systems.AddSystem(system);
            return systems;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref TComponent GetComponent<TComponent>(this IProtoSystems systems, int entity)
            where TComponent : struct
        {
            return ref GetComponent<TComponent>(systems, (ProtoEntity)entity);
        }
        
        public static ref TComponent AddComponent<TComponent>(this IProtoSystems systems, ProtoEntity entity)
            where TComponent : struct
        {
            var world = systems.GetWorld();
            return ref world.AddComponent<TComponent>(entity);
        }

        public static ref TComponent AddComponent<TComponent>(this IProtoSystems systems, int entity)
            where TComponent : struct
        {
            return ref AddComponent<TComponent>(systems, (ProtoEntity)entity);
        }
        
        public static IProtoSystems Add(this IProtoSystems ecsSystems, IEnumerable<IEcsSystem> systems)
        {
            foreach (var system in systems)
                ecsSystems.AddSystem(system);

            return ecsSystems;
        }
    }
}
