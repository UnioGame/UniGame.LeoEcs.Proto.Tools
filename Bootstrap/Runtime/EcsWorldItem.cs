namespace UniGame.LeoEcs.Bootstrap.Runtime
{
    using System;

    [Serializable]
    public class EcsWorldItem : IDisposable
    {
        public IEcsSystemsConfig config;
        public string id;

        public EcsWorldItem(string id,IEcsSystemsConfig config)
        {
            config = config;
            id = id;
        }
        
        public void Dispose()
        {
            
        }
    }
}