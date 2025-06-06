namespace Game.Ecs.Core.Death.Components
{
    using System;
    using Leopotam.EcsLite;
    using Leopotam.EcsProto;
    using UniGame.LeoEcs.Proto;
    /// <summary>
    /// validate all dead child entities in single frame
    /// </summary>
#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	[Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [Serializable]
    public struct ValidateDeadChildEntitiesRequest : IProtoAutoReset<ValidateDeadChildEntitiesRequest>
    {
        public bool ForceDestroy;
        
        public void SetHandlers(IProtoPool<ValidateDeadChildEntitiesRequest> pool) => pool.SetResetHandler(AutoReset);
        
        public static void AutoReset(ref ValidateDeadChildEntitiesRequest c)
        {
            c.ForceDestroy = false;
        }
    }
}