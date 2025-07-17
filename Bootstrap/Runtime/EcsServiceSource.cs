namespace UniGame.LeoEcs.Bootstrap.Runtime
{
    using System;
    using Bootstrap;
    using Ecs.Bootstrap.Runtime.Config;
    using Converter.Runtime;
    using Core.Runtime;
    using Cysharp.Threading.Tasks;
    using Context.Runtime;
    using R3;
    using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

#if TRI_INSPECTOR
    using TriInspector;
#endif
    
    [Serializable]
    [CreateAssetMenu(menuName = "ECS Proto/Ecs Service Source", fileName = nameof(EcsServiceSource))]
    public class EcsServiceSource : ServiceDataSourceAsset<IEcsService>,IEcsExecutorFactory
    {
        #region inspector

        /// <summary>
        /// timeout in ms for feature initialization
        /// </summary>
        [Tooltip("timeout in ms for feature initialization")]
        public float featureTimeout = 20000f;
        
#if ODIN_INSPECTOR || TRI_INSPECTOR
        [InlineEditor] 
#endif
        public EcsConfiguration features;
        

#if ODIN_INSPECTOR || TRI_INSPECTOR
        [InlineEditor]
        [ShowIf(nameof(IsRuntimeConfigVisible))]
#endif
        [Space(10)]
        [SerializeField]
        public EcsConfiguration _runtimeConfiguration;

        #endregion

        private EcsUpdateMap _updateMapData;
        
        public bool IsRuntimeConfigVisible => Application.isPlaying && _runtimeConfiguration != null;

        protected override async UniTask<IEcsService> CreateServiceInternalAsync(IContext context)
        {
            LeoEcsGlobalData.World = null;
            LeoEcsGlobalData.Service = null;
            
            var config = Instantiate(features);
            _updateMapData = config.ecsUpdateMap;
            _runtimeConfiguration = config;

            var ecsService = new EcsService(context,
                config,
                this, 
                featureTimeout);

            //start ecs service update
            await ecsService.CreateWorldAsync("default");
            ecsService.Execute();
            
            var world = ecsService.DefaultWorld.CurrentValue;
            
            context.Publish(world);
            ecsService.DefaultWorld
                .Subscribe(context,(x,y) =>
                {
                    LeoEcsGlobalData.World = x.World;
                    y.Publish(x.World);
                });
            
            LeoEcsGlobalData.World = ecsService.World;
            LeoEcsGlobalData.Service = ecsService;
            
            context.LifeTime.AddDispose(ecsService);
            return ecsService;
        }
        
        public IEcsExecutor Create(string updateId)
        {
            foreach (var updateOrder in _updateMapData.updateQueue)
            {
                if (updateOrder.OrderId.Equals(updateId, StringComparison.OrdinalIgnoreCase))
                    return updateOrder.Factory.Create();
            }

            return _updateMapData.defaultFactory?.Create();
        }

    }
}