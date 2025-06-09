namespace UniGame.LeoEcs.ViewSystem.Behavriour
{
    using Components;
    using Converter.Runtime;
    using Converter.Runtime.Abstract;
    using Core.Runtime;
    using Leopotam.EcsProto;
    using Leopotam.EcsProto.QoL;
    using Runtime.Rx.Runtime.Extensions;
    using Shared.Extensions;

    using UnityEngine;
    using UnityEngine.UI;

#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
#endif
    
    [RequireComponent(typeof(ProtoEcsMonoConverter))]
    public class CloseAllViewsButton : MonoBehaviour, IEcsComponentConverter, ILifeTimeContext
    {
        #region inspector

        public bool isEnabled = true;
        
        public Button trigger;

        #endregion

        private ILifeTime _lifeTime;
        private ProtoPackedEntity _packedEntity;
        private ProtoWorld _world;

        public ILifeTime LifeTime => _lifeTime;

        public bool IsEnabled => isEnabled;

        public string Name => GetType().Name;

        public void Apply(ProtoWorld world, ProtoEntity entity)
        {
            _lifeTime = this.GetAssetLifeTime();
            _world = world;
            _packedEntity = world.PackEntity(entity);
            
            this.Bind(trigger, CloseAll);
        }

#if ODIN_INSPECTOR
        [Button]
#endif
        private void CloseAll()
        {
            if (!_packedEntity.Unpack(_world, out var entity)) return;
            _world.GetOrAddComponent<CloseAllViewsRequest>(entity);
        }
        
#if ODIN_INSPECTOR
        [OnInspectorInit]
#endif
        private void OnInspectorInitialize()
        {
            if (trigger == null)
                trigger = GetComponent<Button>();
        }

        public bool IsMatch(string searchString)
        {
            throw new System.NotImplementedException();
        }
    }
}