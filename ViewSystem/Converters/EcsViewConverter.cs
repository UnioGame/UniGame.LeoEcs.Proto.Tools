namespace UniGame.LeoEcs.ViewSystem.Converters
{
    using Components;
    using Converter.Runtime;
    using Converter.Runtime.Components;
    using Leopotam.EcsProto;
    using Leopotam.EcsProto.QoL;
    using Shared.Extensions;
    using Sirenix.OdinInspector;
    using UniGame.ViewSystem.Runtime;
    using UnityEngine;

    [RequireComponent(typeof(ProtoEcsMonoConverter))]
    public class EcsViewConverter : MonoLeoEcsConverter, 
        IEcsViewConverter
    {
        #region inspector

        [ReadOnly]
        public ProtoEntity entity;

        public bool followEntityLifeTime = false;
        public bool addChildOrderComponent = false;
        
        #endregion
        
        #region private fields

        private ProtoWorld _ProtoWorld;
        private ProtoPackedEntity _viewPackedEntity;
        private IView _view;

        #endregion
        
        #region public properties
        
        public ProtoWorld World => _ProtoWorld;
        public ProtoPackedEntity PackedEntity => _viewPackedEntity;
        public ProtoEntity Entity => entity;
        
        #endregion
        
        #region public methods

        /// <summary>
        /// entity destroyed
        /// </summary>
        public void OnEntityDestroy(ProtoWorld world, ProtoEntity targetEntity)
        {
            _ProtoWorld = null;
            
            entity = (ProtoEntity)(-1);
        }
        
        public sealed override void Apply(GameObject target, ProtoWorld world, ProtoEntity targetEntity)
        {
            entity = targetEntity;
            
            _view = GetComponent<IView>();
            
            if (!isActiveAndEnabled || _view == null) return;

            _ProtoWorld = world;
            _viewPackedEntity = world.PackEntity(entity);
            
            ref var viewComponent = ref world.GetOrAddComponent<ViewComponent>(entity);
            ref var viewStatusComponent = ref world.GetOrAddComponent<ViewStatusComponent>(entity);
            
            viewStatusComponent.Status = ViewStatus.None;
            viewComponent.View = _view;
            viewComponent.Type = _view.GetType();

            if (addChildOrderComponent)
            {
                ref var childOrderComponent = ref world.GetOrAddComponent<ViewOrderComponent>(entity);
                childOrderComponent.Value = target.transform.GetSiblingIndex();
            }

            //follow entity lifetime  and close view if entity is dead
            if (followEntityLifeTime)
            {
                var lifeTimeEntity = world.NewEntity();
                ref var lifeTimeComponent = ref world.AddComponent<ViewEntityLifeTimeComponent>(lifeTimeEntity);
                lifeTimeComponent.View = _view;
                lifeTimeComponent.Value = _viewPackedEntity;
            }
        }

        #endregion
    }
}