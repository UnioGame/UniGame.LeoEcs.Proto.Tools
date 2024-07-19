namespace Game.Ecs.Core.Components
{
    using System;
    using UnityEngine;

    /// <summary>
    /// Component representing ground information.
    /// </summary>
#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [Serializable]
    public struct GroundInfoComponent
    {
        public float CheckDistance;
        
        public Vector3 Normal;
        public bool IsGrounded;
    }
}