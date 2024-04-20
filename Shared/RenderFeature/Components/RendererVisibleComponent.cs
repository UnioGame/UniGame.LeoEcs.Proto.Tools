﻿namespace UniGame.LeoEcs.Shared.Components
{
    using System;

    /// <summary>
    /// enabled render component
    /// </summary>
#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [Serializable]
    public struct RendererVisibleComponent
    {
        public bool Value;
    }
}