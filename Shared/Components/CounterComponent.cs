using System;
using Leopotam.EcsLite;

namespace UniGame.LeoEcs.Shared.Components
{
    using Leopotam.EcsProto;
    using Proto;

    /// <summary>
    /// request component that will be deleted after "counter" cycles
    /// </summary>
    [Serializable]
    public struct CounterComponent<T> : IProtoAutoReset<CounterComponent<T>>
    {
        public int counter;
        
        public void SetHandlers(IProtoPool<CounterComponent<T>> pool) => pool.SetResetHandler(AutoReset);
        
        public static void AutoReset(ref CounterComponent<T> c)
        {
            c.counter = 0;
        }
    }
}