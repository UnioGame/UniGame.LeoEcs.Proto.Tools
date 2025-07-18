namespace UniGame.LeoEcs.Proto
{
    using System.Runtime.CompilerServices;
    using System.Threading;
    using Cysharp.Threading.Tasks;
    using Leopotam.EcsProto;
    using Shared;
    using UniCore.Runtime.ProfilerTools;
    using UnityEngine;

    public static class EcsTools
    {
        public static async UniTask<ProtoWorld> WaitWorldReady(
            this GameObject target,
            CancellationToken cancellationToken = default)
        {
            return await WaitWorldReady(cancellationToken);
        }
        
        public static async UniTask<ProtoWorld> WaitWorldReady(
            this GameObject target,
            string worldId,
            WorldType worldType,
            bool createIfNotExists = true,
            CancellationToken cancellationToken = default)
        {
            switch (worldType)
            {
                case WorldType.DefaultWorld:
                    return await WaitDefaultWorldReady(cancellationToken);
                case WorldType.LastWorld:
                    return await WaitLastWorldReady(cancellationToken);
            }
            
            return await WaitWorldReady(worldId,createIfNotExists,cancellationToken);
        }
        
        public static async UniTask<ProtoWorld> WaitWorldReady(
            this GameObject target,
            string worldId,
            bool createIfNotExists = true,
            CancellationToken cancellationToken = default)
        {
            return await WaitWorldReady(worldId,createIfNotExists,cancellationToken);
        }

        public static async UniTask<ProtoWorld> WaitDefaultWorldReady(CancellationToken cancellationToken = default)
        {
            return await WaitWorldReady(string.Empty,false, cancellationToken);
        }
        
        public static async UniTask<ProtoWorld> WaitLastWorldReady(CancellationToken cancellationToken = default)
        {
            if (LeoEcsGlobalData.Service == null)
            {
                await UniTask.WaitWhile(static () => LeoEcsGlobalData.Service == null, 
                    cancellationToken: cancellationToken);
            }
            
            var service = LeoEcsGlobalData.Service;
            if (service == null)
            {
                GameLog.LogError("ECS SERVICE IS NULL: WaitLastWorldReady WAITING FOR SERVICE TO BE INITIALIZED");
                return null;
            }
            
            var lastWorld = service.LastWorld;
            
            await UniTask.WaitWhile(lastWorld, x => x.CurrentValue==null ||
                x.CurrentValue.IsInitialized == false,cancellationToken: cancellationToken);
            
            var lastWorldData = lastWorld.CurrentValue;
            return lastWorldData.World;
        }

        public static async UniTask<ProtoWorld> WaitWorldReady(
            string worldId,
            bool createIfNotExists,
            CancellationToken cancellationToken = default)
        {
            if (LeoEcsGlobalData.Service == null)
            {
                await UniTask.WaitWhile(static () => LeoEcsGlobalData.Service == null, 
                    cancellationToken: cancellationToken);
            }
            
            var service = LeoEcsGlobalData.Service;
            if (service == null)
            {
                GameLog.LogError("ECS SERVICE IS NULL: WAITING FOR SERVICE TO BE INITIALIZED");
                return null;
            }
            
            return await service.GetWorldAsync(worldId, createIfNotExists, cancellationToken);
        }

        public static async UniTask<ProtoWorld> WaitWorldReady(CancellationToken cancellationToken = default)
        {
            return await WaitWorldReady(string.Empty, false, cancellationToken);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async UniTask<ProtoWorld> WaitAliveWorld(string worldId = null,
            CancellationToken cancellationToken = default)
        {
            if (LeoEcsGlobalData.Service == null)
            {
                await UniTask.WaitWhile(static () => LeoEcsGlobalData.Service == null, 
                    cancellationToken: cancellationToken);
            }

            if (string.IsNullOrEmpty(worldId))
                return await WaitDefaultWorldReady(cancellationToken);

            return await WaitWorldReady(worldId, false, cancellationToken);
        }
    }
}