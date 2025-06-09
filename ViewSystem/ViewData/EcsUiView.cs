namespace UniGame.LeoEcs.ViewSystem.Converters
{
    using System;
    using Bootstrap;
    using Components;
    using Converter.Runtime;
    using Converter.Runtime.Abstract;
    using Leopotam.EcsProto;

    using UiSystem.Runtime;
    using UniGame.ViewSystem.Runtime;
    using UnityEngine;

#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
#endif
    
    [RequireComponent(typeof(ProtoEcsMonoConverter))]
    public abstract class EcsUiView<TViewModel> : View<TViewModel>,
        IEcsComponentConverter,
        IConverterEntityDestroyHandler,
        IEcsView
        where TViewModel : class, IViewModel
    {
        public bool isEnabled = true;
        
#if ODIN_INSPECTOR
        [PropertySpace(8)]
        [FoldoutGroup("settings")]
        [InlineProperty]
        [HideLabel]
#endif
        public EcsViewSettings settings = new();
        
        private EcsViewDataConverter<TViewModel> _dataConverter = new();

        public virtual bool IsEnabled => isEnabled;

        public virtual string Name => GetType().Name;

        public void Apply(ProtoWorld world, ProtoEntity entity)
        {
            _dataConverter.SetUp(settings);
            _dataConverter.Apply(world,entity);
            
            OnApply(world,entity);
        }
        
        public void OnEntityDestroy(ProtoWorld world, ProtoEntity entity)
        {
            _dataConverter.OnEntityDestroy(world, entity);

            EntityDestroy(world, entity);
        }

        protected virtual void EntityDestroy(ProtoWorld world, ProtoEntity entity){}
        
        protected virtual void OnApply(ProtoWorld world, ProtoEntity entity){}
        
        public bool IsMatch(string searchString)
        {
            if (string.IsNullOrEmpty(searchString)) return true;
            if (Name.Contains(searchString, System.StringComparison.OrdinalIgnoreCase))
                return true;
            if(name.Contains(searchString, System.StringComparison.OrdinalIgnoreCase))
                return true;
            return false;
        }
        
        [Serializable]
        public class EcsViewAspect : EcsAspect
        {
            public ProtoPool<ViewComponent<TViewModel>> ViewMarker;
        }
    }
}