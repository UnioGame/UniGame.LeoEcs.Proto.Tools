namespace Game.Ecs.Core.Timer.Systems
{
    using System;
    using Leopotam.EcsProto;
    using Leopotam.EcsProto.QoL;
    using UniGame.LeoEcs.Timer.Components;
    using UniGame.LeoEcs.Timer.Components.Events;
    using Time.Service;
    using UniGame.LeoEcs.Bootstrap;
    using UniGame.LeoEcs.Shared.Extensions;
    using UniGame.LeoEcs.Bootstrap.Runtime.Attributes;

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
    public class UpdateTimerSystem : IProtoRunSystem
    {
        private ProtoWorld _world;
        private TimerAspect _timerAspect;
        
        private ProtoItExc _filter = It
            .Chain<CooldownActiveComponent>()
            .Inc<CooldownComponent>()
            .Exc<CooldownFinishedSelfEvent>()
            .End();

        public void Run()
        {
            foreach (var entity in _filter)
            {
                ref var cooldownComponent = ref _timerAspect.Cooldown.Get(entity);
                ref var stateComponent = ref _timerAspect.State.Get(entity);
                ref var remainsTimeComponent = ref _timerAspect.Remains.GetOrAddComponent(entity);
                
                var cooldown = cooldownComponent.Value;
                var timePassed = GameTime.Time - stateComponent.LastTime;
                
                remainsTimeComponent.Value = cooldown - timePassed;
                
                if(timePassed < cooldown) continue;
                
                _timerAspect.Active.Del(entity);
                _timerAspect.Completed.Add(entity);
                _timerAspect.Finished.Add(entity);
            }
        }
    }
}