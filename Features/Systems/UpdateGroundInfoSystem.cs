namespace Game.Ecs.Core.Systems
{
    using System;
    using Aspects;
    using Components;
    using Leopotam.EcsProto;
    using Leopotam.EcsProto.QoL;
    using UniGame.LeoEcs.Bootstrap.Runtime.Abstract;
    using UniGame.LeoEcs.Bootstrap.Runtime.Attributes;
    using UniGame.LeoEcs.Shared.Components;
    using UnityEngine;

    /// <summary>
    /// one round trip entity lifetime
    /// </summary>
#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [Serializable]
    [ECSDI]
    public sealed class UpdateGroundInfoSystem : IProtoRunSystem
    {
        private ProtoWorld _world;
        private FeaturesAspect _featuresAspect;
        private UnityAspect _unityAspect;
        
        private ProtoIt _filter = It
            .Chain<GroundInfoComponent>()
            .Inc<TransformComponent>()
            .End();

        public void Run()
        {
            foreach (var entity in _filter)
            {
                ref var transformComponent = ref _unityAspect.Transform.Get(entity);
                ref var groundInfo = ref _featuresAspect.GroundInfo.Get(entity);

                if (Physics.Raycast(transformComponent.Value.position + Vector3.up * 0.1f, Vector3.down, out var hitInfo, groundInfo.CheckDistance))
                {
                    groundInfo.Normal = hitInfo.normal;
                    groundInfo.IsGrounded = true;
                }
                else
                {
                    groundInfo.Normal = Vector3.up;
                    groundInfo.IsGrounded = false;
                }
            }
        }
    }
}