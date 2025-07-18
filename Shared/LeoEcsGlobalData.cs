namespace UniGame.LeoEcs.Proto
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using Bootstrap;
    using Cysharp.Threading.Tasks;
    using LeoEcs.Shared.Extensions;
    using UnityEngine;
    using Leopotam.EcsProto;
    using UniGame.Runtime.DataFlow;

    public static class LeoEcsGlobalData
    {
        public static IEcsService Service;
        public static LifeTime LifeTime;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        public static void Reset()
        {
            Service = null;
            LifeTime?.Terminate();
            LifeTime = new LifeTime();
        }
    }
}