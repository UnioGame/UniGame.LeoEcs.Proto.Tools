namespace UniGame.LeoEcs.Bootstrap
{
#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
#endif

#if TRI_INSPECTOR
    using TriInspector;
#endif

    public interface ILeoEcsFeature : ILeoEcsInitializableFeature
#if ODIN_INSPECTOR
        , ISearchFilterable
#endif
    {
        bool IsFeatureEnabled { get; }
        string FeatureName {get;}
    }
}