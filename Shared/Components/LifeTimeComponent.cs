namespace UniGame.LeoEcsLite.LeoEcs.Shared.Components
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using Core.Runtime;
    using Leopotam.EcsLite;
    using UniModules.UniCore.Runtime.DataFlow;

    /// <summary>
    /// lifetime component
    /// </summary>
#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [Serializable]
    public struct LifeTimeComponent : IEcsAutoReset<LifeTimeComponent>,ILifeTime
    {
        private LifeTime _value;
        
        public bool IsTerminated => _value.isTerminated;
        
        public CancellationToken Token => _value.Token;
        
        public void AutoReset(ref LifeTimeComponent c)
        {
            c._value ??= LifeTime.Create();
            c._value.Restart();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ILifeTime AddCleanUpAction(Action cleanAction)
        {
            return _value.AddCleanUpAction(cleanAction);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ILifeTime AddDispose(IDisposable item)
        {
            return _value.AddDispose(item);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ILifeTime AddRef(object o)
        {
            return _value.AddRef(o);
        }

       
    }
}