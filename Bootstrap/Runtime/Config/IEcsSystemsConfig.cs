namespace UniGame.LeoEcs.Bootstrap
{
    using System.Collections.Generic;
    using Ecs.Bootstrap.Runtime.Config;

    public interface IEcsSystemsConfig
    {
        public bool UseFeaturesLoadingLog { get; }
        IReadOnlyList<EcsPlugin> Plugins { get; }
        IReadOnlyList<EcsConfigGroup> FeatureGroups { get; }
        IReadOnlyList<IEcsSystemsPluginProvider> SystemsPlugins { get; }
        
        public bool EnableUnityModules { get; }
        public WorldConfiguration WorldConfiguration { get; }
        public AspectsData AspectsData { get; }
    }
}