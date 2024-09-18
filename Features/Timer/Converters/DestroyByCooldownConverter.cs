namespace Game.Ecs.Core.Timer.Converters
{
    using Leopotam.EcsProto;
    using UniGame.LeoEcs.Converter.Runtime;
    using UniGame.LeoEcs.Shared.Extensions;
    using UniGame.LeoEcs.Timer.Components;
    using UniGame.LeoEcs.Timer.Components.Requests;
    using UnityEngine;

    public sealed class DestroyByCooldownConverter : LeoEcsConverter
    {
        public float duration;
        
        public override void Apply(GameObject target, ProtoWorld world, ProtoEntity entity)
        {
            world.AddComponent<DestroyByCooldownComponent>(entity);
            ref var cooldownComponent = ref world.AddComponent<CooldownComponent>(entity);
            cooldownComponent.Value = duration;
            world.AddComponent<RestartCooldownSelfRequest>(entity);
        }
    }
}