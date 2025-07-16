namespace UniGame.LeoEcs.Converter.Runtime
{
    using System;
    using System.Runtime.CompilerServices;
    using Bootstrap;
    using Cysharp.Threading.Tasks;
    using UnityEngine;
    using Leopotam.EcsProto;
    using Shared.Extensions;
    using UniGame.Runtime.DataFlow;

    public static class LeoEcsGlobalData
    {
        public static IEcsService EcsService;
        public static ProtoWorld World;
        public static LifeTime LifeTime;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        public static void Reset()
        {
            World = null;
            EcsService = null;
            LifeTime?.Terminate();
            LifeTime = new LifeTime();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasFlag<T>(T flag)
            where T : Enum
        {
            return !World.IsAlive() && World.HasFlag<T>(flag);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async UniTask<ProtoWorld> WaitAliveWorld()
        {
            if (World!=null && World.IsAlive()) return World;

            await UniTask.WaitWhile(() => World==null || !World.IsAlive())
                .AttachExternalCancellation(LifeTime.Token);

            return World;
        }
        
        public static T GetGlobal<T>()
        {
            return World.IsAlive() == false ? default : World.GetGlobal<T>();
        }
        
        public static async UniTask<T> GetValueAsync<T>()
        {
            await WaitAliveWorld()
                .AttachExternalCancellation(LifeTime.Token);
            
            return World.GetGlobal<T>();
        }
    }
}