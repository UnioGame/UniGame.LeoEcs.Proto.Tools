namespace UniGame.LeoEcs.Bootstrap.Runtime
{
    using System;
    using Proto.Shared;

    [Serializable]
    public class EcsServiceWorld
    {
        public string Id;
        public EcsWorldData WorldData = new ();
        public IEcsSystemsConfig Config;
    }
}