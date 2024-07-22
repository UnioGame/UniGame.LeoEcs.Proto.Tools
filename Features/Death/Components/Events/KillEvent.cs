namespace Game.Ecs.Core.Death.Components
{
    using System;
    using Leopotam.EcsProto.QoL;

    /// <summary>
    /// separate entity event with source and dest targets
    /// </summary>
#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [Serializable]
    public struct KillEvent
    {
        public ProtoPackedEntity Source;
        public ProtoPackedEntity Destination;
    }
}