namespace UniGame.LeoEcs.Bootstrap
{
    using Cysharp.Threading.Tasks;
    using Leopotam.EcsProto;

    public interface ILeoEcsInitializableFeature
    {
        UniTask InitializeAsync(IProtoSystems ecsSystems);
    }
}