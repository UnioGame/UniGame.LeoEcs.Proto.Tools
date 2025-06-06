namespace UniGame.LeoEcs.Timer.Components
{
    using Leopotam.EcsLite;
    using Leopotam.EcsProto;
    using Proto;

    /// <summary>
    /// Состояние отката.
    /// </summary>
    public struct CooldownStateComponent : IProtoAutoReset<CooldownStateComponent>
    {
        /// <summary>
        /// time of last update.
        /// </summary>
        public float LastTime;
        
        public void SetHandlers(IProtoPool<CooldownStateComponent> pool) => pool.SetResetHandler(AutoReset);

        public static void AutoReset(ref CooldownStateComponent c)
        {
            c.LastTime = 0;
        }
    }
}