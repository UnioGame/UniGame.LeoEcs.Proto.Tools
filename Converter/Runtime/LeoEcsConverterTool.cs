using UniGame.LeoEcs.Shared.Components;
using UniGame.LeoEcs.Shared.Extensions;

namespace UniGame.LeoEcs.Converter.Runtime
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using Abstract;
    using Components;
    using Converters;
    using Cysharp.Threading.Tasks;
    using Leopotam.EcsProto;
    using Leopotam.EcsProto.QoL;
    using Proto;
    using Proto.Shared;
    using UniCore.Runtime.ProfilerTools;
    using UniGame.Runtime.ObjectPool.Extensions;
    using UnityEngine;
    using UnityEngine.Pool;
    using Object = UnityEngine.Object;

    public static class LeoEcsConverterTool
    {
        /// <summary>
        /// base common converters for gameobjects
        /// </summary>
        public static List<IEcsComponentConverter> DefaultGameObjectConverters = new()
        {
            new BaseGameObjectComponentConverter(),
            new ParentEntityComponentConverter(),
        };

        public static void ApplyEcsComponents(
            this GameObject target, 
            ProtoWorld world, 
            ProtoEntity entityId, 
            IEnumerable<IEcsComponentConverter> converterTasks)
        {
#if UNITY_EDITOR
            if (target == null)
                Debug.LogError($"ECS CONVERTER: GameObject {target} is NULL | ENTITY {entityId}");
#endif
            
            foreach (var converter in converterTasks)
            {   
#if UNITY_EDITOR
                if (converter == null)
                    Debug.LogError($"ECS CONVERTER: Converter == null FOR GameObject {target} is NULL | ENTITY {entityId}",target);
#endif
                if(converter is ILeoEcsConverterStatus {IsEnabled: false}) continue;
                
                ref var gameObjectComponent = ref world
                    .GetOrAddComponent<GameObjectComponent>(entityId);
                
                gameObjectComponent.Value = target;
                
                converter?.Apply(world, entityId);
            }
        }
        
        public static void ApplyEcsComponents(
            this ProtoWorld world, 
            GameObject target, 
            ProtoEntity entityId, 
            IEnumerable<IEcsComponentConverter> converterTasks)
        {
            ApplyEcsComponents(target,world, entityId, converterTasks);
        }

        public static ProtoEntity ApplyEcsComponents(
            this ProtoWorld world,
            GameObject target,
            ProtoEntity entity,
            IEnumerable<IEcsComponentConverter> converterTasks,
            bool spawnInstance)
        {
            return target.ConvertGameObjectToEntity(entity, world, converterTasks, spawnInstance);
        }

        public static void ApplyEcsComponents(
            this ProtoWorld world, 
            ProtoEntity entityId, 
            IEnumerable<IEcsComponentConverter> converters)
        {
#if UNITY_EDITOR
            GameObject gameObject = default;
            if (world.HasComponent<GameObjectComponent>(entityId))
            {
                ref var gameObjectComponent = ref world.GetComponent<GameObjectComponent>(entityId);
                gameObject = gameObjectComponent.Value;
            }
#endif
            foreach (var converter in converters)
            {
                var target = converter;
#if UNITY_EDITOR
                if (target == null)
                {
                    Debug.LogError($"ECS CONVERTER: Converter == null FOR GameObject {gameObject?.name} is NULL | ENTITY {entityId}",gameObject);
                    continue;
                }
#endif
                if (target is ScriptableObject scriptableConverter)
                    target = Object.Instantiate(scriptableConverter) as IEcsComponentConverter;
                
                if(target is ILeoEcsConverterStatus { IsEnabled: false })
                    continue;
                
                target?.Apply(world, entityId);
            }
        }

        public static async UniTask<ProtoEntity> ConvertGameObjectToEntity(
            this GameObject target, 
            IEnumerable<IEcsComponentConverter> converters, 
            bool spawnInstance,
            CancellationToken cancellationToken = default)
        {
            var world = await EcsTools.WaitWorldReady(cancellationToken);
            var entity = ConvertGameObjectToEntity(target, world, converters, spawnInstance);
            return entity;
        }
        
        public static ProtoEntity GetParentEntity(this GameObject source)
        {
            var transform = source.transform;
            var parent = transform.parent;
            
            if (parent == null) return (ProtoEntity)(-1);

            var ecsParent = parent.GetComponentInParent<IEcsEntity>();
            if(ecsParent == null) return (ProtoEntity)(-1);

            return ecsParent.Entity;
        }

        public static ProtoEntity ConvertGameObjectToEntity(
            this GameObject gameObject,
            ProtoWorld world, 
            ProtoEntity entity)
        {
#if UNITY_EDITOR
            var packedEntity = entity.PackEntity(world);
            if (packedEntity.Unpack(world,out var _) == false)
            {
                GameLog.LogError($"ENTITY {entity} IS DEAD: TRY TO CONVERT {gameObject} TO ENT {entity}",gameObject);
                return entity;
            }
            
            if (gameObject == null)
            {
                GameLog.LogError($"{gameObject} IS NULL: TRY TO CONVERT {gameObject} TO ENT {entity}",gameObject);
                return entity;
            }
            
            if (world.IsAlive() == false)
            {
                GameLog.LogError($"WORLD IS DEAD: TRY TO CONVERT {gameObject} TO ENT {entity}",gameObject);
                return entity;
            }
#endif

#if UNITY_EDITOR
            var gameObjectName = gameObject.name;
            var entityIndex = gameObjectName.IndexOf("_ENT_",StringComparison.OrdinalIgnoreCase);
            gameObjectName = entityIndex >= 0 ? gameObjectName.Remove(entityIndex) : gameObjectName;
            gameObject.name = $"{gameObjectName}_ENT_{entity}";
#endif
            var connectable = gameObject.GetComponent<IConnectableToEntity>();
            connectable?.ConnectEntity(world, entity);

            var converters = ListPool<IEcsComponentConverter>.Get();
            converters.Clear();

            SelectMonoConverters(gameObject, converters);
            SelectMonoConverter(gameObject, converters);

            ApplyEcsComponents(world,gameObject,entity, converters, false);

            converters.Clear();
            ListPool<IEcsComponentConverter>.Release(converters);

            world.GetOrAddComponent<ObjectConverterComponent>(entity);

            return entity;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SelectMonoConverters(GameObject gameObject,List<IEcsComponentConverter> converters)
        {
            gameObject.GetComponents(converters);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SelectMonoConverter(GameObject gameObject,List<IEcsComponentConverter> converters)
        {
            var container = gameObject.GetComponent<ProtoEcsMonoConverter>();
            if (container == null) return;
            
            converters.AddRange(container.serializableConverters);
            converters.AddRange(container.assetConverters);
        }
        
        public static ProtoEntity ConvertGameObjectToEntity(this GameObject target, 
            ProtoWorld world, 
            IEnumerable<IEcsComponentConverter> converterTasks, 
            bool spawnInstance)
        {
            var entity = world.NewEntity();
            return ConvertGameObjectToEntity(target, entity, world, converterTasks, spawnInstance);
        }
        
        public static ProtoEntity ConvertGameObjectToEntity(
            this GameObject target, 
            ProtoEntity entity,
            ProtoWorld world, 
            IEnumerable<IEcsComponentConverter> converterTasks, 
            bool spawnInstance)
        {
            target = spawnInstance ? target.Spawn() : target;

            ApplyEcsComponents(target, world, entity, DefaultGameObjectConverters);
            ApplyEcsComponents(target, world, entity, converterTasks);

            return entity;
        }

        public static async UniTask DestroyEntity(ProtoEntity entityId, CancellationToken cancellationToken = default)
        {
            var world = await EcsTools.WaitWorldReady(cancellationToken);
            DestroyEntity(entityId, world);
        }
        
        public static async UniTask DestroyEntityAsync(ProtoPackedEntity entityId, CancellationToken cancellationToken = default)
        {
            var world = await EcsTools.WaitWorldReady(cancellationToken);
            DestroyEntity(entityId, world);
        }
        
        public static void DestroyEntity(ProtoPackedEntity entityId, ProtoWorld world)
        {
            if (world.IsAlive() == false) return;

            if (!entityId.Unpack(world, out var entity))
                return;
            
            DestroyEntity(entity, world);
        }

        public static void DestroyEntity(ProtoEntity entityId, ProtoWorld world)
        {
            if (!world.IsAlive()) return;
            
            var packed = entityId.PackEntity(world);
            if (!packed.Unpack(world, out var aliveEntity)) return;

            //world.AddComponent<InstantDestroyComponent>(aliveEntity);
            world.DelEntity(aliveEntity);
        }

        public static async UniTask DestroyEntityAsync(ProtoEntity entityId, ProtoWorld world)
        {
            await UniTask.Yield(PlayerLoopTiming.PreLateUpdate);
            
            if (!world.IsAlive()) return;
            
            var packed = entityId.PackEntity(world);
            if(packed.Unpack(world,out var aliveEntity))
                world.DelEntity(aliveEntity);
        }

    }
}