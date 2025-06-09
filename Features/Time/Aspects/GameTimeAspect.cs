namespace Game.Ecs.Time.Aspects
{
    using System;
    using Components;
    using Leopotam.EcsProto;
    using UniGame.LeoEcs.Bootstrap;

    /// <summary>
    /// game time feature aspect
    /// </summary>
#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [Serializable]
    public class GameTimeAspect : EcsAspect
    {
        public ProtoPool<EntityGameTimeComponent> Time;
    }
}