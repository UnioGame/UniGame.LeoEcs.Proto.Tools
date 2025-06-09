namespace Game.Ecs.Core.Converters
{
    using System;
    using Components;
    using Leopotam.EcsProto;

    using UniGame.LeoEcs.Converter.Runtime;
    using UniGame.LeoEcs.Shared.Extensions;
    using UnityEngine;

#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
#endif
    
    [Serializable]
    public sealed class AvatarConverter : LeoEcsConverter
    {
#if ODIN_INSPECTOR
        [InlineProperty]
        [HideLabel]
        [TitleGroup("Avatar Bounds")]
#endif
        [SerializeField] 
        public EntityBounds entityBounds;

        [Space]
        [SerializeField]
        public Transform headRoot;
        [SerializeField] 
        public Transform bodyRoot;
        [SerializeField]
        public Transform feetRoot;
        [SerializeField]
        public Transform handRoot;

        [Space]
        [SerializeField]
        public Transform weaponRoot;

        public override void Apply(GameObject target, ProtoWorld world, ProtoEntity entity)
        {
            var avatarPool = world.GetPool<EntityAvatarComponent>();
            ref var avatar = ref avatarPool.GetOrAddComponent(entity);

            avatar.Bounds = entityBounds;
            avatar.Head = headRoot;
            avatar.Body = bodyRoot;
            avatar.Feet = feetRoot;
            avatar.Hand = handRoot;
            avatar.Weapon = weaponRoot;
            
            avatar.All = new[]
            {
                headRoot,
                bodyRoot,
                feetRoot,
                handRoot,
                weaponRoot
            };
        }
    }
}