namespace UniGame.LeoEcs.Bootstrap.Runtime
{
    using Bootstrap;
    using Cysharp.Threading.Tasks;
    using Leopotam.EcsProto;

    public class EcsFeatureAsset<TFeature> : BaseLeoEcsFeature
        where TFeature : IEcsFeature, new()
    {
        public TFeature feature = new TFeature();
        
        public override async UniTask InitializeAsync(IProtoSystems ecsSystems)
        {
            await feature.InitializeAsync(ecsSystems);
        }
    }
}