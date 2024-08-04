﻿namespace Game.Ecs.Core.Death.Components
{
    using System;
    using Leopotam.EcsLite;

    /// <summary>
    /// destroy target entity without pooling
    /// </summary>
#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [Serializable]
    public struct DestroySelfRequest : IEcsAutoReset<DestroySelfRequest>
    {
        public bool ForceDestroy;
        
        public void AutoReset(ref DestroySelfRequest c)
        {
            c.ForceDestroy = false;
        }
    }
}