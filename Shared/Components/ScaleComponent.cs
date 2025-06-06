namespace UniGame.LeoEcs.Shared.Components
{
    using System;
    using Leopotam.EcsLite;
    using Leopotam.EcsProto;
    using Proto;
    using UnityEngine;

    /// <summary>
    /// scale component
    /// </summary>
#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [Serializable]
    public struct ScaleComponent : IProtoAutoReset<ScaleComponent>
    {
        public Vector3 Value;
        
        public void SetHandlers(IProtoPool<ScaleComponent> pool) => pool.SetResetHandler(AutoReset);
        
        public static void AutoReset(ref ScaleComponent c)
        {
            c.Value = Vector3.one;
        }
    }
}