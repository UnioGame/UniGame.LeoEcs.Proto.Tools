namespace UniGame.LeoEcs.Shared.Extensions
{
    using System.Runtime.CompilerServices;
    using Leopotam.EcsProto;

    public static class EcsMigrationExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ProtoWorld GetWorld(this IProtoSystems systems)
        {
            return systems.World();
        }

    }
}