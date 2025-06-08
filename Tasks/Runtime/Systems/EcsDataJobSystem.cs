namespace Game.Ecs.EcsThreads.Systems
{
    using Leopotam.EcsProto;
    using UniGame.Core.Runtime;
    using UniGame.LeoEcs.Shared.Extensions;
    using UniGame.Runtime.DataFlow;
    using Unity.Jobs;

    public abstract class EcsDataJobSystem<TJob> : IEcsJobDataParallelFor<TJob>,IProtoDestroySystem
        where TJob : struct, IEcsDataJobParallelFor
    {
        public IProtoSystems ecsSystems;
        public ProtoWorld world;
        
        private LifeTimeDefinition _lifeTime;
        
        private int _defaultJobsCount;
        private JobHandle _jobHandle;
        private TJob _job = default;

        public void Init(IProtoSystems systems)
        {
            ecsSystems = systems;
            world = systems.GetWorld();
            
            _lifeTime = new LifeTimeDefinition();
            _defaultJobsCount = 16;
            _jobHandle = default;
            _job = default;
            
            OnInit(systems,_lifeTime);
        }
        
        public void Destroy()
        {
            _lifeTime.Terminate();
        }
        
        public void Run()
        {
            var defaultHandle = default(JobHandle);
            ref var jobHandle = ref Schedule(ecsSystems,ref defaultHandle);
            
            jobHandle.Complete();
            
            UpdateJobResults(ref _job);
        }

        public ref JobHandle Schedule(IProtoSystems systems,ref JobHandle dependsOn)
        {
            _job = default;
            
            var count = UpdateJobData(ref _job);
            if (count <= 0) return ref dependsOn;
            
            var chunkSize = GetChunkSize();
            _jobHandle = _job.Schedule(count,chunkSize ,dependsOn);
            return ref _jobHandle;
        }
        
        public virtual int GetChunkSize() => _defaultJobsCount;
        
        //custom data setup for job
        public virtual int UpdateJobData(ref TJob job) => -1;

        public virtual void UpdateJobResults(ref TJob job) { }

        protected virtual void OnInit(IProtoSystems systems, ILifeTime lifeTime) { }

    }
    
    
    public interface IEcsJobDataParallelFor<TJob> : IProtoRunSystem,IProtoInitSystem
        where TJob : struct, IEcsDataJobParallelFor
    {
        ref JobHandle Schedule(IProtoSystems systems,ref JobHandle dependsOn);
        int GetChunkSize();
        void UpdateJobResults(ref TJob job);
    }
    
    public interface IEcsDataJobParallelFor : IJobParallelFor
    {
    }
}