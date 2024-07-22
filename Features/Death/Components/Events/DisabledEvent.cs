namespace Game.Ecs.Core.Death.Components
{
    using System;

    /// <summary>
    /// Represents an event that is triggered when an entity is disabled.
    /// </summary>
#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [Serializable]
    public struct DisabledEvent
    {
    }
}