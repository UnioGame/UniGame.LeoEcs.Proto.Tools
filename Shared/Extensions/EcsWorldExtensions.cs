namespace UniGame.LeoEcs.Shared.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using Cysharp.Threading.Tasks;
    using Game.Ecs.Core.Components;
    using Leopotam.EcsProto;
    using Leopotam.EcsProto.QoL;
    using UniGame.Core.Runtime;
    using Runtime.DataFlow;
    using Runtime.Utils;
    using Unity.Collections;
    using UnityEngine;

#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    public static class ProtoWorldExtensions
    {
        private static MemorizeItem<ProtoWorld,WorldContextData> _memorizeItem = MemorizeTool
            .Memorize<ProtoWorld, WorldContextData>(static x =>
            {
                var lifeTime = new LifeTime();
                var hashMap = new NativeHashMap<int, ProtoPackedEntity>(8, Allocator.Persistent);
                lifeTime.AddDispose(hashMap);
                
                var worldData = new WorldContextData()
                {
                    World = x,
                    LifeTime = lifeTime,
                    SingleEntities =hashMap,
                };
                
                UpdateWorldLifeTime(x,lifeTime).Forget();
                return worldData;
                
                static async UniTask UpdateWorldLifeTime(ProtoWorld world,LifeTime lifeTime)
                {
                    while (world.IsAlive())
                    {
#if UNITY_EDITOR
                        if (!Application.isPlaying)
                        {
                            lifeTime.Terminate();
                            return;
                        }        
#endif
                        await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate);
                    }
      
                    lifeTime.Terminate();
                }
                
            },x => x.LifeTime.Terminate());

        private static Slice<object> _removingComponents = new Slice<object>();
        
#if UNITY_EDITOR
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void OnWorldExtensionInit()
        {
            Application.quitting -= Clear;
            Application.quitting += Clear;
            Clear();
        }
        
        public static void Clear()
        {
            _memorizeItem.Dispose();
        }
#endif

#if ENABLE_IL2CPP
        [Il2CppSetOption (Option.NullChecks, false)]
        [Il2CppSetOption (Option.ArrayBoundsChecks, false)]
#endif
        public static ref TComponent AddComponent<TComponent>(this ProtoWorld world, int entity)
            where TComponent : struct
        {
            return ref world.AddComponent<TComponent>((ProtoEntity)entity);
        }
        
#if ENABLE_IL2CPP
        [Il2CppSetOption (Option.NullChecks, false)]
        [Il2CppSetOption (Option.ArrayBoundsChecks, false)]
#endif
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ProtoItChain Filter<TComponent>(this ProtoWorld world)
            where TComponent : struct
        {
            return It.Chain<TComponent>();
        }
        
#if ENABLE_IL2CPP
        [Il2CppSetOption (Option.NullChecks, false)]
        [Il2CppSetOption (Option.ArrayBoundsChecks, false)]
#endif
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Unpack(this ProtoWorld world, ProtoPackedEntity packedEntity, out ProtoEntity entity)
        {
            return packedEntity.TryUnpack(world, out entity);
        }
        
#if ENABLE_IL2CPP
        [Il2CppSetOption (Option.NullChecks, false)]
        [Il2CppSetOption (Option.ArrayBoundsChecks, false)]
#endif
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ProtoEntity NewEntity(this ProtoWorld world)
        {
            world.GetPool<NewEntityComponent>().NewEntity(out var entity);
            return entity;
        }

#if ENABLE_IL2CPP
        [Il2CppSetOption (Option.NullChecks, false)]
        [Il2CppSetOption (Option.ArrayBoundsChecks, false)]
#endif
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ProtoPackedEntity PackEntity(this ProtoEntity entity, ProtoWorld world)
        {
            var packedEntity = world.PackEntity(entity);
            return packedEntity;
        }

#if ENABLE_IL2CPP
        [Il2CppSetOption (Option.NullChecks, false)]
        [Il2CppSetOption (Option.ArrayBoundsChecks, false)]
#endif
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref TComponent GetComponent<TComponent>(this ProtoWorld world, ProtoEntity entity)
            where TComponent : struct
        {
            var pool = world.GetPool<TComponent>();
            return ref pool.Get(entity);
        }
        
#if ENABLE_IL2CPP
        [Il2CppSetOption (Option.NullChecks, false)]
        [Il2CppSetOption (Option.ArrayBoundsChecks, false)]
#endif
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TAspect Aspect<TAspect>(this ProtoWorld world)
            where TAspect : class, IProtoAspect
        {
            var aspect = world.Aspect(typeof(TAspect));
            return aspect as TAspect;
        }
        
#if ENABLE_IL2CPP
        [Il2CppSetOption (Option.NullChecks, false)]
        [Il2CppSetOption (Option.ArrayBoundsChecks, false)]
#endif
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref TComponent GetComponent<TComponent>(this ProtoWorld world, int entity)
            where TComponent : struct
        {
            return ref world.GetComponent<TComponent>((ProtoEntity)entity);
        }

#if ENABLE_IL2CPP
        [Il2CppSetOption (Option.NullChecks, false)]
        [Il2CppSetOption (Option.ArrayBoundsChecks, false)]
#endif
        public static IEnumerable<ProtoPackedEntity> PackAll(this ProtoWorld world, IEnumerable<int> entities)
        {
            foreach (var entity in entities)
            {
                yield return world.PackEntity(entity);
            }
        }
        
#if ENABLE_IL2CPP
        [Il2CppSetOption (Option.NullChecks, false)]
        [Il2CppSetOption (Option.ArrayBoundsChecks, false)]
#endif
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PackAll(this ProtoWorld world,List<ProtoPackedEntity> container, IEnumerable<ProtoEntity> entities)
        {
            foreach (var entity in entities)
                container.Add(world.PackEntity(entity));
        }
        
#if ENABLE_IL2CPP
        [Il2CppSetOption (Option.NullChecks, false)]
        [Il2CppSetOption (Option.ArrayBoundsChecks, false)]
#endif
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PackAll(this ProtoWorld world,List<ProtoPackedEntity> container, IEnumerable<int> entities)
        {
            foreach (var entity in entities)
                container.Add(world.PackEntity(entity));
        }

#if ENABLE_IL2CPP
        [Il2CppSetOption (Option.NullChecks, false)]
        [Il2CppSetOption (Option.ArrayBoundsChecks, false)]
#endif
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool UnpackAll(this ProtoWorld world,List<ProtoEntity> result, List<ProtoPackedEntity> packedEntities)
        {
            var unpackResult = true;
            foreach (var ProtoPackedEntity in packedEntities)
            {
                
                if (!ProtoPackedEntity.Unpack(world, out var entity))
                {
                    unpackResult = false;
                    continue;
                }
                result.Add(entity);
            }

            return unpackResult;
        }
        
#if ENABLE_IL2CPP
        [Il2CppSetOption (Option.NullChecks, false)]
        [Il2CppSetOption (Option.ArrayBoundsChecks, false)]
#endif
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int UnpackAll(this ProtoWorld world,ProtoPackedEntity[] from,ProtoEntity[] to)
        {
            var amount = 0;
            foreach (var ProtoPackedEntity in from)
            {
                if (!ProtoPackedEntity.Unpack(world, out var entity))
                    continue;
                to[amount] = entity;
                amount++;
            }

            return amount;
        }
        
#if ENABLE_IL2CPP
        [Il2CppSetOption (Option.NullChecks, false)]
        [Il2CppSetOption (Option.ArrayBoundsChecks, false)]
#endif
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int PackAll(this ProtoWorld world,int[] source,ProtoPackedEntity[] result, int count)
        {
            var counter = 0;
            for (var i = 0; i < count; i++)
            {
                var entity = source[i];
                if(entity < 0) continue;
                result[counter] = world.PackEntity(source[i]);
                counter++;
            }
            return counter;
        }
        
#if ENABLE_IL2CPP
        [Il2CppSetOption (Option.NullChecks, false)]
        [Il2CppSetOption (Option.ArrayBoundsChecks, false)]
#endif
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int UnpackAll(this ProtoWorld world, ProtoPackedEntity[] packedEntities,int[] result, int amount)
        {
            var counter = 0;
            
            for (var i = 0; i < amount; i++)
            {
                ref var packedEntity = ref packedEntities[i];
                if (!packedEntity.Unpack(world, out var entity))
                    continue;
                
                result[counter] = (int)entity;
                counter++;
            }

            return counter;
        }
        
#if ENABLE_IL2CPP
        [Il2CppSetOption (Option.NullChecks, false)]
        [Il2CppSetOption (Option.ArrayBoundsChecks, false)]
#endif
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int UnpackAll(this ProtoWorld world, 
            ProtoPackedEntity[] packedEntities,
            ProtoEntity[] result, int amount)
        {
            var counter = 0;
            
            for (var i = 0; i < amount; i++)
            {
                ref var packedEntity = ref packedEntities[i];
                if (!packedEntity.Unpack(world, out var entity))
                    continue;
                
                result[counter] = entity;
                counter++;
            }

            return counter;
        }
        
        
#if ENABLE_IL2CPP
        [Il2CppSetOption (Option.NullChecks, false)]
        [Il2CppSetOption (Option.ArrayBoundsChecks, false)]
#endif
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryRemoveComponent<TComponent>(this ProtoWorld world, int entity)
            where TComponent : struct
        {
            return TryRemoveComponent<TComponent>(world, (ProtoEntity)entity);
        }
        
#if ENABLE_IL2CPP
        [Il2CppSetOption (Option.NullChecks, false)]
        [Il2CppSetOption (Option.ArrayBoundsChecks, false)]
#endif
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryRemoveComponent<TComponent>(this ProtoWorld world, ProtoEntity entity)
            where TComponent : struct
        {
            var pool = world.GetPool<TComponent>();
            if (!pool.Has(entity))
                return false;

            pool.Del(entity);
            return true;
        }

#if ENABLE_IL2CPP
        [Il2CppSetOption (Option.NullChecks, false)]
        [Il2CppSetOption (Option.ArrayBoundsChecks, false)]
#endif
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RemoveComponent<TComponent>(this ProtoWorld world, ProtoEntity entity)
            where TComponent : struct
        {
            var pool = world.GetPool<TComponent>();
            pool.Del(entity);
        }
        
#if ENABLE_IL2CPP
        [Il2CppSetOption (Option.NullChecks, false)]
        [Il2CppSetOption (Option.ArrayBoundsChecks, false)]
#endif
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasComponent<TComponent>(this ProtoWorld world, int entity)
            where TComponent : struct
        {
            return world.HasComponent<TComponent>((ProtoEntity)entity);
        }
                
#if ENABLE_IL2CPP
        [Il2CppSetOption (Option.NullChecks, false)]
        [Il2CppSetOption (Option.ArrayBoundsChecks, false)]
#endif
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasComponent<TComponent>(this ProtoWorld world, ProtoEntity entity)
            where TComponent : struct
        {
            var pool = world.GetPool<TComponent>();
            return pool.Has(entity);
        }
        
#if ENABLE_IL2CPP
        [Il2CppSetOption (Option.NullChecks, false)]
        [Il2CppSetOption (Option.ArrayBoundsChecks, false)]
#endif
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref TComponent AddComponent<TComponent>(this ProtoWorld world, ProtoEntity entity)
            where TComponent : struct
        {
            var pool = world.GetPool<TComponent>();
            ref var component = ref pool.Add(entity);
            return ref component;
        }
        
#if ENABLE_IL2CPP
        [Il2CppSetOption (Option.NullChecks, false)]
        [Il2CppSetOption (Option.ArrayBoundsChecks, false)]
#endif
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref TComponent GetOrAddComponent<TComponent>(this ProtoWorld world, ProtoEntity entity)
            where TComponent : struct
        {
            var pool = world.GetPool<TComponent>();
            ref var component = ref pool.Has(entity) ? ref pool.Get(entity) : ref pool.Add(entity);
            return ref component;
        }
        
#if ENABLE_IL2CPP
        [Il2CppSetOption (Option.NullChecks, false)]
        [Il2CppSetOption (Option.ArrayBoundsChecks, false)]
#endif
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void GetOrAddComponent(this ProtoWorld world, ProtoEntity entity,Type componentType)
        {
            var pool = world.Pool(componentType);
            if (pool.Has(entity)) return;
            pool.AddRaw(entity);
        }
        
#if ENABLE_IL2CPP
        [Il2CppSetOption (Option.NullChecks, false)]
        [Il2CppSetOption (Option.ArrayBoundsChecks, false)]
#endif
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void TryRemoveComponent(this ProtoWorld world, ProtoEntity entity,Type componentType)
        {
            var pool = world.Pool(componentType);
            if (!pool.Has(entity)) return;
            pool.Del(entity);
        }
        
#if ENABLE_IL2CPP
        [Il2CppSetOption (Option.NullChecks, false)]
        [Il2CppSetOption (Option.ArrayBoundsChecks, false)]
#endif
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref TComponent GetOrAddComponent<TComponent>(this ProtoWorld world, int entity)
            where TComponent : struct
        {
            return ref world.GetOrAddComponent<TComponent>((ProtoEntity)entity);
        }
        
#if ENABLE_IL2CPP
        [Il2CppSetOption (Option.NullChecks, false)]
        [Il2CppSetOption (Option.ArrayBoundsChecks, false)]
#endif
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref TComponent AddRawComponent<TComponent>(this ProtoWorld world, ProtoEntity entity, TComponent component)
            where TComponent : struct
        {
            var pool = world.Pool(typeof(TComponent));
            if (pool.Has(entity))
                pool.Del(entity);
            
            pool.SetRaw(entity,component);
            var typePool = world.GetPool<TComponent>();
            return ref typePool.Get(entity);
        }
        
#if ENABLE_IL2CPP
        [Il2CppSetOption (Option.NullChecks, false)]
        [Il2CppSetOption (Option.ArrayBoundsChecks, false)]
#endif
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RemoveComponents<TType>(this ProtoWorld world, ProtoEntity entity)
        {
            world.EntityComponents(entity, _removingComponents);
            var data = _removingComponents.Data();
            
            for (int i = 0; i < _removingComponents.Len(); i++)
            {
                var component = data[i];
                if(component is not TType)continue;
                var pool = world.Pool(component.GetType());
                pool.Del(entity);
            }
        }
        
#if ENABLE_IL2CPP
        [Il2CppSetOption (Option.NullChecks, false)]
        [Il2CppSetOption (Option.ArrayBoundsChecks, false)]
#endif
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddRawComponents(this ProtoWorld world, ProtoEntity entity,int count, object[] components)
        {
            for (var i = 0; i < count; i++)
            {
                var component = components[i];
                AddRawComponent(world, entity, component.GetType(), component);
            }
        }
        
#if ENABLE_IL2CPP
        [Il2CppSetOption (Option.NullChecks, false)]
        [Il2CppSetOption (Option.ArrayBoundsChecks, false)]
#endif
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddRawComponent(this ProtoWorld world, ProtoEntity entity,Type componentType, object component)
        {
            var pool = world.Pool(componentType);
            if (pool.Has(entity))
                pool.Del(entity);
            
            pool.SetRaw(entity,component);
        }

#if ENABLE_IL2CPP
        [Il2CppSetOption (Option.NullChecks, false)]
        [Il2CppSetOption (Option.ArrayBoundsChecks, false)]
#endif
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void FilterByComponent<T>(this ProtoWorld world, IEnumerable<int> filter, List<int> result) where T : struct
        {
            var pool = world.GetPool<T>();
            foreach (var entity in filter)
            {
                if (pool.Has((ProtoEntity)entity))
                    result.Add(entity);
            }
        }

#if ENABLE_IL2CPP
        [Il2CppSetOption (Option.NullChecks, false)]
        [Il2CppSetOption (Option.ArrayBoundsChecks, false)]
#endif
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void FilterByComponent<T, T2>(this ProtoWorld world, IEnumerable<int> filter, List<int> result)
            where T : struct
            where T2 : struct
        {
            var pool = world.GetPool<T>();
            var pool2 = world.GetPool<T2>();
            foreach (var entity in filter)
            {
                var protoEntity = (ProtoEntity)entity;
                if (pool.Has(protoEntity) && pool2.Has(protoEntity))
                    result.Add(entity);
            }
        }

#if ENABLE_IL2CPP
        [Il2CppSetOption (Option.NullChecks, false)]
        [Il2CppSetOption (Option.ArrayBoundsChecks, false)]
#endif
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void FilterByComponent<T, T2, T3>(this ProtoWorld world, IEnumerable<int> filter, List<int> result)
            where T : struct
            where T2 : struct
            where T3 : struct
        {
            var pool = world.GetPool<T>();
            var pool2 = world.GetPool<T2>();
            var pool3 = world.GetPool<T3>();
            foreach (var entity in filter)
            {
                var protoEntity = (ProtoEntity)entity;
                if (pool.Has(protoEntity) && pool2.Has(protoEntity) && pool3.Has(protoEntity))
                    result.Add(entity);
            }
        }

#if ENABLE_IL2CPP
        [Il2CppSetOption (Option.NullChecks, false)]
        [Il2CppSetOption (Option.ArrayBoundsChecks, false)]
#endif
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void FilterByComponent<T, T2, T3, T4>(this ProtoWorld world, IEnumerable<int> filter, List<int> result)
            where T : struct
            where T2 : struct
            where T3 : struct
            where T4 : struct
        {
            var pool = world.GetPool<T>();
            var pool2 = world.GetPool<T2>();
            var pool3 = world.GetPool<T3>();
            var pool4 = world.GetPool<T4>();

            foreach (var entity in filter)
            {
                var protoEntity = (ProtoEntity)entity;
                if (pool.Has(protoEntity) && pool2.Has(protoEntity) && pool3.Has(protoEntity) && pool4.Has(protoEntity))
                    result.Add(entity);
            }
        }
        
#if ENABLE_IL2CPP
        [Il2CppSetOption (Option.NullChecks, false)]
        [Il2CppSetOption (Option.ArrayBoundsChecks, false)]
#endif
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ProtoPackedEntity PackEntity(this ProtoWorld world, int entity)
        {
            return world.PackEntity((ProtoEntity)entity);
        }
        
#if ENABLE_IL2CPP
        [Il2CppSetOption (Option.NullChecks, false)]
        [Il2CppSetOption (Option.ArrayBoundsChecks, false)]
#endif
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DelEntity(this ProtoWorld world, int entity)
        {
            world.DelEntity((ProtoEntity)entity);
        }

#if ENABLE_IL2CPP
        [Il2CppSetOption (Option.NullChecks, false)]
        [Il2CppSetOption (Option.ArrayBoundsChecks, false)]
#endif
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ILifeTime GetWorldLifeTime(this ProtoWorld world) => _memorizeItem[world].LifeTime;

#if ENABLE_IL2CPP
        [Il2CppSetOption (Option.NullChecks, false)]
        [Il2CppSetOption (Option.ArrayBoundsChecks, false)]
#endif
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ILifeTime GetLifeTime(this ProtoWorld world) => _memorizeItem[world].LifeTime;


#if ENABLE_IL2CPP
        [Il2CppSetOption (Option.NullChecks, false)]
        [Il2CppSetOption (Option.ArrayBoundsChecks, false)]
#endif
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetSingleValue<TComponent>(this ProtoWorld world,ref ProtoPackedEntity value)
        {
            var data = _memorizeItem[world];
            var hash = typeof(TComponent).GetHashCode();
            data.SingleEntities[hash] = value;
        }
        
    }


    public class WorldContextData
    {
        public ProtoWorld World;
        public LifeTime LifeTime;
        public NativeHashMap<int,ProtoPackedEntity> SingleEntities;
    }
}
