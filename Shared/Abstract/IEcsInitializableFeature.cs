namespace UniGame.LeoEcs.Bootstrap
{
    using Cysharp.Threading.Tasks;
    using Leopotam.EcsProto;
    using UnityEngine.Scripting.APIUpdating;

    [MovedFrom(true,sourceClassName:"ILeoEcsInitializableFeature")]
    public interface IEcsInitializableFeature
    {
        UniTask InitializeAsync(IProtoSystems ecsSystems);
    }
}