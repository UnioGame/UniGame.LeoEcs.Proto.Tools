namespace UniGame.LeoEcs.ViewSystem.Behavriour
{
    using Converter.Runtime;
    using Converter.Runtime.Abstract;
    using Core.Runtime;

    using Extensions;
    using Leopotam.EcsProto;
    using UiSystem.Runtime.Settings;
    using Runtime.Rx.Runtime.Extensions;
    using UniModules.UniGame.UiSystem.Runtime;
    using UnityEngine;
    using UnityEngine.UI;

#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
#endif
    
    [RequireComponent(typeof(ProtoEcsMonoConverter))]
    public class OpenViewButton : MonoBehaviour, IEcsComponentConverter, ILifeTimeContext
    {
        #region inspector

        public bool isEnabled = true;
        
        public Button trigger;
        
        /// <summary>
        /// target view type
        /// </summary>
        public ViewId view;
        
        /// <summary>
        /// target layout
        /// </summary>
        public ViewType layoutType = ViewType.Window;

        #endregion

        private ILifeTime _lifeTime;

        public ILifeTime LifeTime => _lifeTime;

        public bool IsEnabled => isEnabled;

        public string Name => GetType().Name;

        public void Apply(ProtoWorld world, ProtoEntity entity)
        {
            _lifeTime = this.GetAssetLifeTime();
            trigger ??= GetComponent<Button>();
            
            this.Bind(trigger, x => world
                .MakeViewRequest(view, layoutType));
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
            if(string.IsNullOrEmpty(searchString)) return true;
         
            if(Name.Contains(searchString, System.StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        }
    }
}
