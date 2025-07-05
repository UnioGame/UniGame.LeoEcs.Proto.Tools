namespace UniGame.LeoEcs.Bootstrap.Runtime
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Bootstrap;
    using Ecs.Bootstrap.Runtime.Config;
    using Converter.Runtime;
    using Core.Runtime;
    using Cysharp.Threading.Tasks;
    using Context.Runtime;
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

            var config = Instantiate(features);
            _updateMapData = config.ecsUpdateMap;
            _runtimeConfiguration = config;

            var plugins = config
                .systemsPlugins
                .Select(x => x.Create())
                .ToList();
            
            var ecsService = new EcsService(context,
                config,
                this, 
                plugins,
                featureTimeout);

            var world = ecsService.World;
            context.Publish(ecsService.World);
            
            //start ecs service update
            await ecsService.InitializeAsync();
            ecsService.Execute();
            ecsService.SetDefaultWorld(world);
            
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