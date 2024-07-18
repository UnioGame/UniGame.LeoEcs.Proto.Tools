namespace UniGame.LeoEcs.Converter.Runtime.Converters
{
    using Abstract;
    using Leopotam.EcsLite;
    using Leopotam.EcsProto;
    using Leopotam.EcsProto.QoL;
    using Shared.Components;
    using Shared.Extensions;
    using UnityEngine;
    using UnityEngine.Serialization;

#if TRI_INSPECTOR
    using TriInspector;
#endif
    
#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
#endif
    
    [RequireComponent(typeof(ProtoEcsMonoConverter))]
    public class MonoLeoEcsConverter<TConverter> : 
        MonoBehaviour,
        IEcsComponentConverter,
        IConverterEntityDestroyHandler
        where TConverter : IEcsComponentConverter
    {
        #region inspector
        
        [FormerlySerializedAs("_converter")]
        [SerializeField]
#if ODIN_INSPECTOR || TRI_INSPECTOR
        [HideLabel]
        [InlineProperty]
#endif
        public TConverter converter;

        public TConverter Converter => converter;
        
        public bool IsEnabled => converter.IsEnabled;

        public bool IsRuntime => Application.isPlaying;
        
        public string Name => converter == null ? "EMPTY" : converter.Name;
        public void Apply(ProtoWorld world, ProtoEntity entity)
        {
            throw new System.NotImplementedException();
        }

        #endregion

        public ProtoPackedEntity Entity{get; private set;}
        protected ProtoWorld World{get; private set;}
        
        public void Apply(GameObject target, ProtoWorld world, ProtoEntity entity)
        {
            if (converter == null) return;
            
            var targetEntity = (ProtoEntity)entity;
            
            ref var gameObjectComponent = ref world
                .GetOrAddComponent<GameObjectComponent>(entity);
            gameObjectComponent.Value = target;
            
            converter.Apply(world, targetEntity);

            OnApply(gameObject, world, entity);

            Entity = targetEntity.PackEntity(world);
            World = world;
        }
        
        public virtual void OnEntityDestroy(ProtoWorld world, ProtoEntity entity)
        {
            if(converter is IConverterEntityDestroyHandler destroyHandler)
                destroyHandler.OnEntityDestroy(world, entity);
        }

        protected virtual void OnApply(GameObject target, ProtoWorld world, ProtoEntity entity)
        {
            
        }
        
        private void OnDrawGizmos()
        {
            if (converter is ILeoEcsGizmosDrawer gizmosDrawer)
                gizmosDrawer.DrawGizmos(gameObject);
        }

        public bool IsMatch(string searchString)
        {
            if (string.IsNullOrEmpty(searchString)) return true;
            if (searchString.Contains(name)) return true;
#if ODIN_INSPECTOR
            if (converter.IsMatch(searchString)) return true;
#endif
            return false;
        }
    }
}