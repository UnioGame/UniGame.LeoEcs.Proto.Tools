namespace UniGame.Proto.Ownership
{
    using System;
    using Leopotam.EcsProto.QoL;

    [Serializable]
    public struct OwnerLinkComponent
    {
        public ProtoPackedEntity Value;
    }
}