﻿namespace UniGame.LeoEcsLite.LeoEcs.Shared.Components
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using Core.Runtime;
    using Leopotam.EcsLite;
    using Leopotam.EcsProto;
    using UniGame.LeoEcs.Proto;
    using Runtime.DataFlow;
    
#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [Serializable]
    public struct LifeTimeComponent : IProtoAutoReset<LifeTimeComponent>
    {
        private LifeTime _value;

        public CancellationToken Token;
        
        public bool IsTerminated => _value.isTerminated;
        
        public ILifeTime LifeTime => _value;
        
        public void SetHandlers(IProtoPool<LifeTimeComponent> pool) => pool.SetResetHandler(AutoReset);
        
        public static void AutoReset(ref LifeTimeComponent c)
        {
            c._value ??= new LifeTime();
            c._value.Restart();
            c.Token = c._value.Token;
        }
        
        internal void Release()
        {
            _value.Terminate();
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