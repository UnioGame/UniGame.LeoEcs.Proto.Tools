namespace Game.Ecs.Core.Aspects
{
    using System;
    using Components;
    using Leopotam.EcsProto;
    using UniGame.LeoEcs.Bootstrap;

    /// <summary>
    /// Features aspect
    /// </summary>
#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [Serializable]
    public class FeaturesAspect : EcsAspect
    {
        // champion component
        public ProtoPool<ChampionComponent> Champion;
        // Representing the character link component.
        public ProtoPool<CharacterLinkComponent> CharacterLink;
        // Компонент со ссылкой на рутовый transform для эффектов на entity.
        public ProtoPool<EntityAvatarComponent> EntityAvatar;
        // Component representing ground information.
        public ProtoPool<GroundInfoComponent> GroundInfo;
        // Компонент со ссылкой на PlayableDirector.
        public ProtoPool<PlayableDirectorComponent> PlayableDirector;
        // Component used for selection targets in the game.
        public ProtoPool<SelectionTargetComponent> SelectionTarget;
    }
}