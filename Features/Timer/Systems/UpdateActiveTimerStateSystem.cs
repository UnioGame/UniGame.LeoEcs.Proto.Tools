namespace Game.Ecs.Core.Timer.Systems
{
    using System;
    using Leopotam.EcsProto;
    using Leopotam.EcsProto.QoL;
    using UniGame.LeoEcs.Timer.Components;
    using UniGame.LeoEcs.Bootstrap;
    using UniGame.LeoEcs.Bootstrap.Runtime.Attributes;
    using UnityEngine;

    /// <summary>
    /// update active timer
    /// </summary>
#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [Serializable]
    [ECSDI]
    public class UpdateActiveTimerStateSystem : IProtoRunSystem
    {
        private ProtoWorld _world;
        private TimerAspect _timerAspect;
        
        private ProtoItExc _filter = It
            .Chain<CooldownActiveComponent>()
            .Inc<CooldownComponent>()
            .Exc<CooldownStateComponent>()
            .End();

        public void Run()
        {
            foreach (var entity in _filter)
            {
                ref var stateComponent = ref _timerAspect.State.Add(entity);
                stateComponent.LastTime = Time.time;
            }
        }
    }
}