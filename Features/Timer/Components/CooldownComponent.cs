namespace UniGame.LeoEcs.Timer.Components
{
    using Leopotam.EcsProto;
    using Proto;

    public struct CooldownComponent : IProtoAutoReset<CooldownComponent>
    {
        public float Value;
        
        public void SetHandlers(IProtoPool<CooldownComponent> pool) => pool.SetResetHandler(AutoReset);
        
        public static void AutoReset(ref CooldownComponent c)
        {
            c.Value = 0;
        }
    }
}