namespace UniGame.LeoEcs.Shared.Extensions
{
    using System.Runtime.CompilerServices;
    using Leopotam.EcsProto;

    public static class EcsFilterExtensions
    {
        public static readonly ProtoEntity InvalidEntity = (ProtoEntity)(-1); 
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FirstEntity  First(this ProtoIt filter)
        {
            foreach (var entity in filter)
                return new FirstEntity(){Entity = entity, Found = true};

            return new FirstEntity(){Entity = InvalidEntity, Found = false};
        }
        
        public struct FirstEntity
        {
            public bool Found;
            public ProtoEntity Entity;
        }
    }
}