namespace UniGame.LeoEcs.Shared.Extensions
{
    using System;
    using System.Runtime.CompilerServices;
    using Leopotam.EcsLite;
    using Leopotam.EcsProto;
    using ViewSystem.Data;

    public static class EcsFilterExtensions
    {
        public static readonly ProtoEntity InvalidEntity = (ProtoEntity)(-1); 
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int First(this EcsFilter filter)
        {
            foreach (var entity in filter)
                return (int)entity;

            return -1;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TypeIt FilterType<TType>(this IProtoIt it)
        {
            return new TypeIt(it, typeof(TType));
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TypeIt FilterType(this IProtoIt it,Type type)
        {
            return new TypeIt(it, type);
        }
    }
}