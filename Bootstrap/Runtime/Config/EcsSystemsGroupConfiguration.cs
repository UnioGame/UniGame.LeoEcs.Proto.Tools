using Cysharp.Threading.Tasks;

namespace UniGame.Ecs.Bootstrap.Runtime.Config
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using LeoEcs.Bootstrap.Runtime;
    using LeoEcs.Bootstrap;
    using Leopotam.EcsLite;
    using Leopotam.EcsProto;
    using UniCore.Runtime.ProfilerTools;
    using UnityEngine;
    using UnityEngine.Serialization;
    using Object = UnityEngine.Object;

#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
#endif

#if TRI_INSPECTOR
    using TriInspector;
#endif
    
    [Serializable]
    public class EcsSystemsGroupConfiguration : IEcsSystemsGroup
    {
        private const string FeatureInfoGroup = "feature info";
        
        public bool showFeatureInfo = true;
        
#if ODIN_INSPECTOR
        [BoxGroup(FeatureInfoGroup)]
#endif
#if ODIN_INSPECTOR || TRI_INSPECTOR
        [ShowIf(nameof(showFeatureInfo))]
#endif
        [FormerlySerializedAs("_name")]
        [SerializeField]
        public string name;
        
#if ODIN_INSPECTOR
        [BoxGroup(FeatureInfoGroup)]
#endif
#if ODIN_INSPECTOR || TRI_INSPECTOR
        [ShowIf(nameof(showFeatureInfo))]
#endif
        [SerializeField]
        public bool _active = true;
 
        /// <summary>
        /// ecs group systems
        /// </summary>
        [SerializeReference]
#if ODIN_INSPECTOR
        [Searchable]
#endif
        private List<IEcsSystem> _systems = new();
        
#if ODIN_INSPECTOR
        [Searchable(FilterOptions = SearchFilterOptions.ISearchFilterableInterface)]
        [ListDrawerSettings(ListElementLabelName = "@FeatureName")]
#endif
        public List<BaseLeoEcsFeature> nestedFeatures = new();

#if ODIN_INSPECTOR
        [Searchable]
#endif
        [SerializeReference]
        public List<ILeoEcsFeature> serializableFeatures = new();

        
        public string FeatureName => name;
        
        public bool IsFeatureEnabled => _active;

        public IReadOnlyList<IEcsSystem> EcsSystems => _systems;
        
        public void RegisterSystems(List<IEcsSystem> systems)
        {
            systems.AddRange(EcsSystems);
        }

        public async UniTask InitializeAsync(IProtoSystems ecsSystems)
        {
            if (!IsFeatureEnabled) return;

#if DEBUG
            var timer = Stopwatch.StartNew();   
            timer.Restart();
#endif
            
            foreach (var featureAsset in nestedFeatures)
            {
                if(!featureAsset.IsFeatureEnabled) continue;
                var featureInstance = Object.Instantiate(featureAsset);
#if DEBUG  
                timer.Restart();
#endif
                
                await featureInstance.InitializeAsync(ecsSystems);

#if DEBUG
                var elapsed = timer.ElapsedMilliseconds;
                timer.Stop();
                GameLog.Log($"\tECS SUB FEATURE: LOAD TIME {featureInstance.FeatureName} | {featureInstance.GetType().Name} = {elapsed} ms");
#endif
            }

            foreach (var ecsFeature in serializableFeatures)
            {
                if(!ecsFeature.IsFeatureEnabled) continue; 
#if DEBUG  
                timer.Restart();
#endif
                await ecsFeature.InitializeAsync(ecsSystems);
#if DEBUG
                var elapsed = timer.ElapsedMilliseconds;
                timer.Stop();
                GameLog.Log($"ECS FEATURE: LOAD TIME {ecsFeature.FeatureName} | {ecsFeature.GetType().Name} = {elapsed} ms");
#endif
            }

#if DEBUG  
            timer.Restart();
#endif
            
            await OnInitializeFeatureAsync(ecsSystems);

#if DEBUG
            var elapsedTime = timer.ElapsedMilliseconds;
            timer.Stop();
            GameLog.Log($"ECS FEATURE: LOAD TIME SELF {FeatureName} | {this.GetType().Name} = {elapsedTime} ms");
#endif
        }
        
        public virtual bool IsMatch(string searchString)
        {
            if (string.IsNullOrEmpty(searchString)) return true;

            var typeName = GetType().Name;
            if (typeName.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0) 
                return true;

            if (FeatureName.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0)
                return true;

            foreach (var featureAsset in nestedFeatures)
            {
                if (featureAsset.IsMatch(searchString))
                    return true;
            }

#if ODIN_INSPECTOR
            foreach (var featureAsset in serializableFeatures)
            {
                if (featureAsset.IsMatch(searchString))
                    return true;
            }
#endif
            
            return false;
        }

        protected virtual UniTask OnInitializeFeatureAsync(IProtoSystems ecsSystems)
        {
            return UniTask.CompletedTask;
        }
    }
}