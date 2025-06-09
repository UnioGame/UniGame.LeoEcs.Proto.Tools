namespace UniGame.LeoEcs.Bootstrap
{
    using System.Collections.Generic;
    using Ecs.Bootstrap.Runtime.Config;

    public interface IEcsSystemsConfig
    {
        IReadOnlyList<EcsPlugin> Plugins { get; }
        IReadOnlyList<EcsConfigGroup> FeatureGroups { get; }
        
        public bool EnableUnityModules { get; }
        public WorldConfiguration WorldConfiguration { get; }
        public AspectsData AspectsData { get; }
    }
}