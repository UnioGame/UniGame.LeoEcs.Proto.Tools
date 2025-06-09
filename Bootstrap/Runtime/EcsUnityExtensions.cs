namespace UniGame.LeoEcs.Bootstrap.Runtime
{
    using Bootstrap;
    using Cysharp.Threading.Tasks;

    public static class EcsUnityExtensions
    {
        public static PlayerLoopTiming ConvertToPlayerLoopTiming(this EcsPlayerUpdateType updateType)
        {
            return updateType switch
            {
                EcsPlayerUpdateType.None => PlayerLoopTiming.Initialization,
                EcsPlayerUpdateType.Update => PlayerLoopTiming.Update,
                EcsPlayerUpdateType.FixedUpdate => PlayerLoopTiming.FixedUpdate,
                EcsPlayerUpdateType.LateUpdate => PlayerLoopTiming.LastUpdate,
                _ => PlayerLoopTiming.Update
            };
        }
    }
}