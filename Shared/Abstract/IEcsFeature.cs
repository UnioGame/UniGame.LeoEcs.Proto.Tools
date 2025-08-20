namespace UniGame.LeoEcs.Bootstrap
{
    using UnityEngine.Scripting.APIUpdating;
#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
#endif

#if TRI_INSPECTOR
    using TriInspector;
#endif

    [MovedFrom(true,sourceClassName:"ILeoEcsFeature")]
    public interface IEcsFeature : IEcsInitializableFeature
#if ODIN_INSPECTOR
        , ISearchFilterable
#endif
    {
        bool IsFeatureEnabled { get; }
        string FeatureName {get;}
    }
}