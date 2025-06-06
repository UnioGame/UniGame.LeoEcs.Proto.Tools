using System;
using Leopotam.EcsLite;

namespace UniGame.LeoEcs.ViewSystem.Components
{
    using Leopotam.EcsProto;
    using Proto;

    [Serializable]
    public struct UpdateViewRequest : IProtoAutoReset<UpdateViewRequest>
    {
        public int counter;
        
        public void SetHandlers(IProtoPool<UpdateViewRequest> pool) => pool.SetResetHandler(AutoReset);
        
        public static void AutoReset(ref UpdateViewRequest c)
        {
            c.counter = 0;
        }
    }
    
    [Serializable]
    public struct UpdateViewRequest<T> {}
}