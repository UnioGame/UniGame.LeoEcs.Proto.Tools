namespace UniGame.LeoEcs.Bootstrap.Runtime.Abstract
{
    using System;
    using Leopotam.EcsProto;
    using Leopotam.EcsProto.QoL;
    using Runtime;

#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;
    
    [Il2CppSetOption (Option.NullChecks, false)]
    [Il2CppSetOption (Option.ArrayBoundsChecks, false)]
#endif
    [Serializable]
    public abstract class EcsAspect : ProtoAspectInject, IEcsAspect
    {
        public ProtoWorld world;
        
        public sealed override void Init(ProtoWorld aspectWorld)
        {
            this.world = aspectWorld;
            OnInit(aspectWorld);
            base.Init(world);
        }

        protected virtual void OnInit(ProtoWorld aspectWorld)
        {
            
        }
    }
}