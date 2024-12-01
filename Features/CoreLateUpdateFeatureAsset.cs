namespace Game.Ecs.Core
{
    using Cysharp.Threading.Tasks;
    using Leopotam.EcsProto;
    using UniGame.LeoEcs.Bootstrap.Runtime;
    using UnityEngine;

    [CreateAssetMenu(menuName = "ECS Proto/Features/Core/Core Late Feature", fileName = "Core Late Feature")]
    public sealed class CoreLateUpdateFeatureAsset : BaseLeoEcsFeature
    {
        public CoreLateUpdateFeature coreLateUpdateFeature = new();
        
        public override async UniTask InitializeAsync(IProtoSystems ecsSystems)
        {
            await coreLateUpdateFeature.InitializeAsync(ecsSystems);
        }
    }
}