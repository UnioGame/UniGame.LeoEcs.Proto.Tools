namespace Game.Ecs.Core.Components
{
    using System;
    using Leopotam.EcsLite;
    using Leopotam.EcsProto;
    using UniGame.LeoEcs.Proto;
    using UnityEngine;
    
    public struct EntityAvatarComponent : IProtoAutoReset<EntityAvatarComponent>
    {
        public EntityBounds Bounds;
        
        public Transform Head;
        public Transform Body;
        public Transform Feet;
        public Transform Hand;
        public Transform Weapon;
        
        public Transform[] All;
        
        public void SetHandlers(IProtoPool<EntityAvatarComponent> pool) => pool.SetResetHandler(AutoReset);
        
        public static void AutoReset(ref EntityAvatarComponent c)
        {
            c.Bounds = default;
            c.Head = default;
            c.Body = default;
            c.Feet = default;
            c.Hand = default;
            c.Weapon = default;
            
            c.All = Array.Empty<Transform>();
        }
    }
}