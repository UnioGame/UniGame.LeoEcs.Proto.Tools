namespace Game.Ecs.EcsThreads.Systems
{
    using Leopotam.EcsLite;
    using Leopotam.EcsProto;
    
    public abstract class EcsThreadSystemBase
    {
        protected abstract int GetChunkSize(IProtoSystems systems);
        protected abstract ProtoIt GetFilter(ProtoWorld world);
        protected abstract ProtoWorld GetWorld(IProtoSystems systems);
    }

    public interface IEcsThreadBase
    {
        void Execute(int fromIndex, int beforeIndex);
    }

    public interface IEcsThread<T1> : IEcsThreadBase
        where T1 : struct
    {
        void Init(
            int[] entities,
            T1[] pool1, int[] indices1);
    }

    public interface IEcsThread<T1, T2> : IEcsThreadBase
        where T1 : struct
        where T2 : struct
    {
        void Init(
            int[] entities,
            T1[] pool1, int[] indices1,
            T2[] pool2, int[] indices2);
    }

    public interface IEcsThread<T1, T2, T3> : IEcsThreadBase
        where T1 : struct
        where T2 : struct
        where T3 : struct
    {
        void Init(
            int[] entities,
            T1[] pool1, int[] indices1,
            T2[] pool2, int[] indices2,
            T3[] pool3, int[] indices3);
    }

    public interface IEcsThread<T1, T2, T3, T4> : IEcsThreadBase
        where T1 : struct
        where T2 : struct
        where T3 : struct
        where T4 : struct
    {
        void Init(
            int[] entities,
            T1[] pool1, int[] indices1,
            T2[] pool2, int[] indices2,
            T3[] pool3, int[] indices3,
            T4[] pool4, int[] indices4);
    }
}