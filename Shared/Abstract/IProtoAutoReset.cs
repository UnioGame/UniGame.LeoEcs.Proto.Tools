namespace UniGame.LeoEcs.Proto
{
    using Leopotam.EcsProto;

    public interface IProtoAutoReset<TComponent> : IProtoHandlers<TComponent>
        where TComponent : struct
    {
    }
}