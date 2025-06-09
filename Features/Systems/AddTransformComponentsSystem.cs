namespace Game.Ecs.Core.Systems
{
    using System;
    using Components;
    using Leopotam.EcsProto;
    using Leopotam.EcsProto.QoL;
    using UniGame.LeoEcs.Bootstrap;
    using UniGame.LeoEcs.Bootstrap.Runtime.Attributes;
    using UniGame.LeoEcs.Shared.Components;
    using UniGame.LeoEcs.Shared.Extensions;
#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [Serializable]
    [ECSDI]
    public sealed class AddTransformComponentsSystem : IProtoRunSystem
    {
        private ProtoWorld _world;
        private UnityAspect _unityAspect;

        private ProtoItExc _filter = It
            .Chain<TransformComponent>()
            .Exc<TransformPositionComponent>()
            .Exc<PrepareToDeathComponent>()
            .End();
        
        public void Run()
        {
            foreach (var entity in _filter)
            {
                _unityAspect.Position.GetOrAddComponent(entity);
                _unityAspect.Direction.GetOrAddComponent(entity);
                _unityAspect.Scale.GetOrAddComponent(entity);
                _unityAspect.Rotation.GetOrAddComponent(entity);
            }
        }
    }
}