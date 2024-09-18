namespace Game.Modules.leoecs.proto.tools.Ownership
{
    using System;
    using Cysharp.Threading.Tasks;
    using Leopotam.EcsProto;
    using Systems;
    using UniGame.LeoEcs.Bootstrap.Runtime;
    using UniGame.LeoEcs.Shared.Extensions;
    using UnityEngine;

#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [Serializable]
    [CreateAssetMenu(menuName = "Proto Features/Core/Ownership Feature", fileName = "Ownership Feature")]
    public class OwnershipFeature : BaseLeoEcsFeature
    {
        public override UniTask InitializeAsync(IProtoSystems ecsSystems)
        {
            ecsSystems.Add(new DeleteEntitySystem());
            ecsSystems.Add(new PrepareEntityDeathSystem());
            
            return UniTask.CompletedTask;
        }
    }
}