namespace UniGame.LeoEcs.Bootstrap
{
    using System;
    using Leopotam.EcsProto;

    public interface ISystemsPlugin : IDisposable
    {
        bool IsActive { get; }

        void Execute(ProtoWorld world);

        void Add(IProtoSystems systems);

        void Stop();
    }
}