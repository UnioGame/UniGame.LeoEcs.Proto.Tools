namespace Game.Ecs.Core
{
    using System;
    using Systems;
    using Components;
    using Cysharp.Threading.Tasks;
    using Death.Components;
    using Death.Systems;
    using Leopotam.EcsProto;
    using Leopotam.EcsProto.QoL;
    using Time;
    using Timer;
    using UniGame.LeoEcs.Bootstrap.Runtime;
    using UniGame.LeoEcs.Shared.Extensions;
    using UniGame.LeoEcs.Shared.Components;

    [Serializable]
    public class CoreFeature : EcsFeature
    {
        public TimerFeature timerFeature = new();
        public GameTimeFeature gameTimeFeature = new();
        
        protected override async UniTask OnInitializeAsync(IProtoSystems ecsSystems)
        {
            ecsSystems.Add(new AddTransformComponentsSystem());
            ecsSystems.Add(new UpdateTransformDataSystem());
            
            await gameTimeFeature.InitializeAsync(ecsSystems);
            
            //ecsSystems.Add(new KillMeNextTimeHandleSystem());
            ecsSystems.Add(new ProcessDestroySilentSystem());
            
            ecsSystems.Add(new UpdateRenderStatusSystem());
            ecsSystems.Add(new DisableColliderSystem());
            ecsSystems.Add(new ProcessDeadSimpleEntitiesSystem());
            ecsSystems.Add(new ProcessDeadTransformEntitiesSystem());
            
            //ecsSystems.Add(new DestroyInvalidChildEntitiesSystem());
            //ecsSystems.Add(new ForceValidateDeadChildEntitiesSystem());
            //ecsSystems.DelHere<OwnerDestroyedEvent>();
            
            //ecsSystems.Add(new CheckInvalidChildEntitiesSystem());
            ecsSystems.Add(new ProcessDespawnSystem());
            
            ecsSystems.DelHere<DeadEvent>();
            ecsSystems.DelHere<DisabledEvent>();
            ecsSystems.DelHere<KillEvent>();
            
            ecsSystems.Add(new ProcessKillRequestSystem());
            
            ecsSystems.DelHere<KillRequest>();
            ecsSystems.DelHere<ValidateDeadChildEntitiesRequest>();
            
            ecsSystems.Add(new ProcessDestroySilentSystem());

            await timerFeature.InitializeAsync(ecsSystems);
        }
    }
}