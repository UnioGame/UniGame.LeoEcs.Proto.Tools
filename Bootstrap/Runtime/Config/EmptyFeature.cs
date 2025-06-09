namespace UniGame.Ecs.Bootstrap.Runtime.Config
{
    using System;
    using System.Collections.Generic;
    using Cysharp.Threading.Tasks;
    using LeoEcs.Bootstrap;
    using Leopotam.EcsLite;
    using Leopotam.EcsProto;

#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
#endif

#if TRI_INSPECTOR
    using TriInspector;
#endif

    [Serializable]
    public class EmptyFeature : IEcsSystemsGroup
    {
        public static readonly List<IEcsSystem> EmptySystems = new();
        public const string EcsEmptyFeatureName = "Empty Feature";
        
#if ODIN_INSPECTOR || TRI_INSPECTOR
        [ReadOnly]
#endif
        public string name = EcsEmptyFeatureName;
        
        public UniTask InitializeAsync(IProtoSystems ecsSystems)
        {
            return UniTask.CompletedTask;
        }

        public bool IsMatch(string searchString)
        {
            return string.IsNullOrEmpty(searchString);
        }

        public bool IsFeatureEnabled => false;

        public string FeatureName => EcsEmptyFeatureName;
        public IReadOnlyList<IEcsSystem> EcsSystems => EmptySystems;
        
        public void RegisterSystems(List<IEcsSystem> systems)
        {
            systems.AddRange(EcsSystems);
        }
    }
}