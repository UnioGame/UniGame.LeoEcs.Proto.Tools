namespace Game.Ecs.Core
{
    using Cysharp.Threading.Tasks;
    using Leopotam.EcsProto;
    using UniGame.LeoEcs.Bootstrap.Runtime;
    using UnityEngine;

    [CreateAssetMenu(menuName = "ECS Proto/Features/Core Feature", fileName = "Core Feature")]
    public class CoreFeatureAsset : BaseLeoEcsFeature
    {
        public CoreFeature coreFeature = new();
        
        public override async UniTask InitializeAsync(IProtoSystems ecsSystems)
        {
            await coreFeature.InitializeAsync(ecsSystems);
        }
    }
    
}