namespace UniGame.LeoEcs.Bootstrap.Runtime
{
    using System;
    using Bootstrap;
    using Cysharp.Threading.Tasks;
    using Leopotam.EcsProto;

    [Serializable]
    public abstract class EcsFeature : IEcsFeature
    {
        public string name;
        public bool isEnabled = true;

        public virtual bool IsFeatureEnabled => isEnabled;

        public virtual string FeatureName => string.IsNullOrEmpty(name) ? GetType().Name : name;

        public async UniTask InitializeAsync(IProtoSystems ecsSystems)
        {
            if (!isEnabled) return;
            await OnInitializeAsync(ecsSystems);
        }

        public virtual bool IsMatch(string searchString)
        {
            if (string.IsNullOrEmpty(searchString)) return true;
            
            if (!string.IsNullOrEmpty(name) && ContainsSearchString(name,searchString)) 
                return true;
            
            return ContainsSearchString(FeatureName, searchString);
        }

        protected abstract UniTask OnInitializeAsync(IProtoSystems ecsSystems);

        protected bool ContainsSearchString(string source, string filter)
        {
            return !string.IsNullOrEmpty(source) && 
                   source.Contains(filter, StringComparison.OrdinalIgnoreCase);
        }
        
    }
}