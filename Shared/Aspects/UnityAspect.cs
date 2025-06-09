namespace UniGame.LeoEcs.Bootstrap
{
    using System;
    using Game.Ecs.Core.Components;
    using LeoEcsLite.LeoEcs.Shared.Components;
    using Leopotam.EcsProto;
    using Shared.Components;

    [Serializable]
    public class UnityAspect : EcsAspect
    {
        public ProtoPool<UnityObjectComponent> UnityObject;
        public ProtoPool<GameObjectComponent> GameObject;
        public ProtoPool<AssetComponent> Asset;
        public ProtoPool<RotationComponent> QuaternionRotation;
        public ProtoPool<TransformComponent> Transform;
        public ProtoPool<TransformPositionComponent> Position;
        public ProtoPool<TransformDirectionComponent> Direction;
        public ProtoPool<TransformScaleComponent> Scale;
        public ProtoPool<TransformRotationComponent> Rotation;
        public ProtoPool<SpriteComponent> Sprite;
        public ProtoPool<LightComponent> Light;
    }
}