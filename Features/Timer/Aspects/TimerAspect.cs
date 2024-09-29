namespace UniGame.LeoEcs.Bootstrap.Runtime.Abstract
{
    using System;
    using Timer.Components;
    using Timer.Components.Events;
    using Timer.Components.Requests;
    using Leopotam.EcsProto;

#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;
#endif
    
#if ENABLE_IL2CPP
    [Il2CppSetOption (Option.NullChecks, false)]
    [Il2CppSetOption (Option.ArrayBoundsChecks, false)]
#endif
    [Serializable]
    public class TimerAspect : EcsAspect
    {
        public ProtoPool<CooldownComponent> Cooldown;
        public ProtoPool<CooldownStateComponent> State;
        public ProtoPool<CooldownActiveComponent> Active;
        public ProtoPool<CooldownCompleteComponent> Completed;
        public ProtoPool<CooldownRemainsTimeComponent> Remains;
        public ProtoPool<CooldownAutoRestartComponent> AutoRestart;
        public ProtoPool<DestroyByCooldownComponent> DestroyByCooldown;
        
        //requests
        public ProtoPool<RestartCooldownSelfRequest> Restart;
        
        //events
        public ProtoPool<CooldownFinishedSelfEvent> Finished;
    }
}