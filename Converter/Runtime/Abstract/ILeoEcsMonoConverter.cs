namespace UniGame.LeoEcs.Converter.Runtime
{
    using System.Collections.Generic;
    using Abstract;
    using Leopotam.EcsLite;
    using Leopotam.EcsProto;

    public interface ILeoEcsMonoConverter : 
        IComponentConverterProvider,
        IEcsEntity,
        IConnectableToEntity
    {
        bool AutoCreate { get; }
        
        void Convert(ProtoWorld world, ProtoEntity ecsEntity);

        void DestroyEntity();
    }

    public interface IConnectableToEntity
    {
        void ConnectEntity(ProtoWorld world, ProtoEntity ecsEntity);
    }

    public interface IComponentConverterProvider
    {
        IReadOnlyList<IEcsComponentConverter> ComponentConverters { get; }
    }
}