namespace Game.Ecs.Core.Components
{
    using System;
    using Leopotam.EcsProto.QoL;

    /// <summary>
    /// Representing the character link component.
    /// </summary>
#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [Serializable]
    public struct CharacterLinkComponent
    {
        public ProtoPackedEntity CharacterEntity;
    }
}