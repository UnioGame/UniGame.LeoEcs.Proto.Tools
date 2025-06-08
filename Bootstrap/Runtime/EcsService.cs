namespace UniGame.LeoEcs.Bootstrap.Runtime
{
    using System;
    using UniGame.Core.Runtime;
    using UniGame.UniNodes.GameFlow.Runtime;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Abstract;
    using Aspects;
    using Ecs.Bootstrap.Runtime.Config;
    using Converter.Runtime;
    using Core.Runtime.Extension;
    using Cysharp.Threading.Tasks;
    using Leopotam.EcsLite;
    using Leopotam.EcsProto;
    using Leopotam.EcsProto.QoL;
    using Leopotam.EcsProto.Unity;
    using Shared.Extensions;
    using UniCore.Runtime.ProfilerTools;
    using UniGame.Runtime.DataFlow;
    using UniGame.Runtime.Utils;
    using Object = UnityEngine.Object;

    public class EcsService : GameService, IEcsService
    {
        private IEcsSystemsConfig _config;
        private IEcsExecutorFactory _ecsExecutorFactory;
        private IEnumerable<ISystemsPlugin> _plugins;
        private Dictionary<string, EcsFeatureSystems> _systemsMap;
        private Dictionary<string, IEcsExecutor> _systemsExecutors;
        private IContext _context;
        private ProtoWorld _world;

        private bool _isInitialized;
        private float _featureTimeout;

        private List<IEcsSystem> _lateSystems = new() { };

        public ProtoWorld World => _world;

        public EcsService(IContext context,
            IEcsSystemsConfig config,
            IEcsExecutorFactory ecsExecutorFactory,
            IEnumerable<ISystemsPlugin> plugins,
            float featureTimeout)
        {
            _systemsMap = new Dictionary<string, EcsFeatureSystems>(8);
            _systemsExecutors = new Dictionary<string, IEcsExecutor>(8);

            _context = context;
            _config = config;

            _ecsExecutorFactory = ecsExecutorFactory;
            _plugins = plugins;
            _featureTimeout = featureTimeout;

            _world = CreateWorld(_config);

            LifeTime.AddCleanUpAction(CleanUp);
        }

        public ProtoWorld CreateWorld(IEcsSystemsConfig config)
        {
            var worldConfig = config.WorldConfiguration.Create();
            var aspectsData = config.AspectsData;
            var worldAspect = new WorldAspect();
            
            foreach (var factory in aspectsData.factories)
            {
                var aspect = factory.Create();
                worldAspect.AddAspect(aspect);
            }
            
            foreach (var aspect in aspectsData.aspects)
            {
                if (!aspect.enabled) continue;
                var aspectType = (Type)aspect.aspectType;
                
                if (aspectType == null) continue;
                
                if (aspectType.IsGenericType && !aspectType.IsConstructedGenericType)
                    continue;
                
                var aspectInstance = aspectType.CreateWithDefaultConstructor() as IProtoAspect;
                worldAspect.AddAspect(aspectInstance);
            }


            var protoWorld = new ProtoWorld(worldAspect, worldConfig);
            return protoWorld;
        }

        public void SetDefaultWorld(ProtoWorld world)
        {
            LeoEcsGlobalData.World = world;
        }

        public override async UniTask InitializeAsync()
        {
#if DEBUG
            var stopwatch = Stopwatch.StartNew();
            stopwatch.Start();
#endif
            await InitializeEcsService(_world);

            _isInitialized = true;

            var plugins = _config.Plugins
                .Where(x => x.enabled && x.plugin != null)
                .Select(x => x.plugin)
                .ToList();

            foreach (var plugin in plugins)
            {
                plugin.PreInit(_context);
            }

            foreach (var systems in _systemsMap.Values)
            {
                if (_config.EnableUnityModules)
                {
                    systems.AddModule(new UnityModule(new UnityModule.Config()
                    {
                        NotBakeComponentsInName = true
                    }));
                }

                foreach (var plugin in plugins)
                {
                    plugin.Init(systems);
                }
            }

            foreach (var systems in _systemsMap.Values)
            {
                systems.Init();
            }

            foreach (var plugin in plugins)
            {
                plugin.PostInit();
            }
#if DEBUG
            LogServiceTime("InitializeAsync", stopwatch);
#endif
        }

        public void Execute()
        {
            if (!_isInitialized) return;

            foreach (var (updateType, systems) in _systemsMap)
            {
                if (!_systemsExecutors.TryGetValue(updateType, out var executor))
                {
                    executor = _ecsExecutorFactory.Create(updateType);
                    _systemsExecutors[updateType] = executor;
                }

                executor.Execute(_world);
                executor.Add(systems);
            }

            ApplyPlugins(_world);
        }

        public void Pause()
        {
            foreach (var systemsExecutor in _systemsExecutors.Values)
                systemsExecutor.Stop();
        }

        public void CleanUp()
        {
            foreach (var systems in _systemsMap.Values)
                systems.Destroy();

            foreach (var ecsExecutor in _systemsExecutors.Values)
                ecsExecutor.Dispose();

            _systemsMap.Clear();
            _systemsExecutors.Clear();

            _world?.Destroy();
            _world = null;
        }

        [Conditional("DEBUG")]
        private void LogServiceTime(string message, Stopwatch timer, bool stop = true)
        {
            var elapsed = timer.ElapsedMilliseconds;
            timer.Restart();
            if (stop) timer.Stop();
            GameLog.Log($"ECS FEATURE SOURCE: LOAD {message} TIME = {elapsed} ms");
        }

        private async UniTask InitializeEcsService(ProtoWorld world)
        {
            var groups = _config
                .FeatureGroups
                .Select(x => CreateEcsGroupAsync(x, world));

            await UniTask.WhenAll(groups);
        }

        private List<ILeoEcsFeature> CollectFeatures(EcsConfigGroup ecsGroup)
        {
            var features = new List<ILeoEcsFeature>();
            foreach (var feature in ecsGroup.features)
                features.Add(feature.Feature);
            return features;
        }

        private async UniTask CreateEcsGroupAsync(EcsConfigGroup ecsGroup, ProtoWorld world)
        {
            var systemsGroup = CollectFeatures(ecsGroup);
            await CreateEcsGroup(ecsGroup.updateType, world, systemsGroup);
        }

        private void ApplyPlugins(ProtoWorld world)
        {
            foreach (var systemsPlugin in _plugins)
            {
                systemsPlugin.AddTo(LifeTime);

                foreach (var map in _systemsMap)
                    systemsPlugin.Add(map.Value);

                systemsPlugin.Execute(world);
            }
        }

        private EcsFeatureSystems CreateEcsSystems(string groupId, ProtoWorld world)
        {
            var systems = new EcsFeatureSystems(world);
            systems.AddService(_context,typeof(IContext));

            _systemsMap[groupId] = systems;
            return systems;
        }

        private async UniTask CreateEcsGroup(
            string updateType,
            ProtoWorld world,
            IReadOnlyList<ILeoEcsFeature> runnerFeatures)
        {
            if (!_systemsMap.TryGetValue(updateType, out var ecsSystems))
            {
                ecsSystems = CreateEcsSystems(updateType, world);
                ecsSystems.AddModule(new AutoInjectModule());
            }

            var asyncFeatures = runnerFeatures
                .Select(x => InitializeFeatureAsync(ecsSystems, x));

            await UniTask.WhenAll(asyncFeatures);

            foreach (var startupSystem in _lateSystems)
                ecsSystems.AddSystem(startupSystem);
        }

        public async UniTask InitializeFeatureAsync(IProtoSystems ecsSystems, ILeoEcsFeature feature)
        {
            if (!feature.IsFeatureEnabled) return;

#if DEBUG
            var timer = Stopwatch.StartNew();
            timer.Restart();
#endif

            if (feature is ILeoEcsInitializableFeature initializeFeature)
            {
                var featureLifeTime = new LifeTimeDefinition();

                await initializeFeature
                    .InitializeAsync(ecsSystems)
                    .AttachTimeoutLogAsync(GetErrorMessage(initializeFeature), _featureTimeout, featureLifeTime.Token);

                featureLifeTime.Terminate();
            }

#if DEBUG
            LogServiceTime($"{feature.FeatureName} | {feature.GetType().Name}", timer, false);
#endif

            if (feature is not IEcsSystemsGroup groupFeature)
                return;

            foreach (var system in groupFeature.EcsSystems)
            {
                var leoEcsSystem = system;

                //create instance of SO systems
                if (leoEcsSystem is Object systemAsset)
                {
                    systemAsset = Object.Instantiate(systemAsset);
                    leoEcsSystem = systemAsset as IEcsSystem;
                }

                var featureLifeTime = new LifeTimeDefinition();
                if (leoEcsSystem is ILeoEcsInitializableFeature initFeature)
                {
#if DEBUG
                    timer.Restart();
#endif
                    await initFeature
                        .InitializeAsync(ecsSystems)
                        .AttachTimeoutLogAsync(GetErrorMessage(initFeature), _featureTimeout, featureLifeTime.Token);

#if DEBUG
                    LogServiceTime($"\tSUB FEATURE {feature.GetType().Name}", timer);
#endif

                    featureLifeTime.Release();
                }

                ecsSystems.Add(leoEcsSystem);
            }
        }

        private string GetErrorMessage(ILeoEcsInitializableFeature feature)
        {
            var featureName = feature is ILeoEcsFeature ecsFeature
                ? ecsFeature.FeatureName
                : feature.GetType().Name;

            return $"ECS Feature Timeout Error for {featureName}";
        }
    }
}