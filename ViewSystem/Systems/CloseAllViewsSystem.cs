namespace UniGame.LeoEcs.ViewSystem
{
    using System;
    using Bootstrap.Runtime.Attributes;
    using Components;
    using Leopotam.EcsProto;
    using Leopotam.EcsProto.QoL;
    using Shared.Extensions;
    using UniGame.ViewSystem.Runtime;

    /// <summary>
    /// System for closing all views in the game.
    /// </summary>
#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [Serializable]
    [ECSDI]
    public class CloseAllViewsSystem : IProtoInitSystem, IProtoRunSystem
    {
        private IGameViewSystem _gameViewSystem;
        private ProtoWorld _world;

        private ProtoIt _closeAllFilter = It
            .Chain<CloseAllViewsRequest>()
            .End();

        public void Init(IProtoSystems systems)
        {
            _world = systems.GetWorld();
            _gameViewSystem = _world.GetGlobal<IGameViewSystem>();
        }

        public void Run()
        {
            foreach (var entity in _closeAllFilter)
            {
                _gameViewSystem.CloseAll();
                break;
            }
        }
    }
}