namespace Game.Ecs.EcsThreads.Systems
{
    using Leopotam.EcsLite;
    using Leopotam.EcsProto;
    using UniGame.LeoEcs.Shared.Extensions;
    using Unity.Collections;
    using Unity.Collections.LowLevel.Unsafe;
    using Unity.Jobs;

    
    public abstract class EcsUnityJobSystemBase : IProtoRunSystem
    {
        public abstract void Run();
        protected abstract int GetChunkSize(IProtoSystems systems);
        protected abstract ProtoIt GetFilter(ProtoWorld world);
        protected abstract ProtoWorld GetWorld(IProtoSystems systems);
    }

    public interface IEcsUnityJob<T1> : IJobParallelFor
        where T1 : unmanaged
    {
        void Init(NativeArray<int> entities,
            NativeArray<T1> pool1, NativeArray<int> indices1);
    }

    public interface IEcsUnityJob<T1, T2> : IJobParallelFor
        where T1 : unmanaged
        where T2 : unmanaged
    {
        void Init(
            NativeArray<int> entities,
            NativeArray<T1> pool1, NativeArray<int> indices1,
            NativeArray<T2> pool2, NativeArray<int> indices2);
    }

    public interface IEcsUnityJob<T1, T2, T3> : IJobParallelFor
        where T1 : unmanaged
        where T2 : unmanaged
        where T3 : unmanaged
    {
        void Init(
            NativeArray<int> entities,
            NativeArray<T1> pool1, NativeArray<int> indices1,
            NativeArray<T2> pool2, NativeArray<int> indices2,
            NativeArray<T3> pool3, NativeArray<int> indices3);
    }

    public interface IEcsUnityJob<T1, T2, T3, T4> : IJobParallelFor
        where T1 : unmanaged
        where T2 : unmanaged
        where T3 : unmanaged
        where T4 : unmanaged
    {
        void Init(
            NativeArray<int> entities,
            NativeArray<T1> pool1, NativeArray<int> indices1,
            NativeArray<T2> pool2, NativeArray<int> indices2,
            NativeArray<T3> pool3, NativeArray<int> indices3,
            NativeArray<T4> pool4, NativeArray<int> indices4);
    }

    public static class NativeHelpers
    {
        public static unsafe NativeWrappedData<T> WrapToNative<T>(T[] managedData) where T : unmanaged
        {
            fixed (void* ptr = managedData)
            {
#if UNITY_EDITOR
                var nativeData =
                    NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<T>(ptr, managedData.Length,
                        Allocator.None);
                var sh = AtomicSafetyHandle.Create();
                NativeArrayUnsafeUtility.SetAtomicSafetyHandle(ref nativeData, sh);
                return new NativeWrappedData<T> { Array = nativeData, SafetyHandle = sh };
#else
                return new NativeWrappedData<T> { Array =
 NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<T> (ptr, managedData.Length, Allocator.None) };
#endif
            }
        }

#if UNITY_EDITOR
        public static void UnwrapFromNative<T1>(NativeWrappedData<T1> sh) where T1 : unmanaged
        {
            AtomicSafetyHandle.CheckDeallocateAndThrow(sh.SafetyHandle);
            AtomicSafetyHandle.Release(sh.SafetyHandle);
        }
#endif
        public struct NativeWrappedData<TT> where TT : unmanaged
        {
            public NativeArray<TT> Array;
#if UNITY_EDITOR
            public AtomicSafetyHandle SafetyHandle;
#endif
        }
    }
}