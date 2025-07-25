﻿namespace UniGame.LeoEcs.Bootstrap
{
    using System;
    using Attributes;
    using Leopotam.EcsProto;
    using Runtime.Attributes;
    using Shared.Components;

    /// <summary>
    /// unity animation components
    /// </summary>
#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [Serializable]
    [ECSDI]
    public class UnityAnimationAspect : EcsAspect
    {
        public ProtoPool<AnimatorComponent> Animator;
    }
}