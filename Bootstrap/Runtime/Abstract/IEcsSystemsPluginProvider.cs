namespace UniGame.Ecs.Bootstrap.Runtime.Config
{
    using LeoEcs.Bootstrap;

    public interface IEcsSystemsPluginProvider
    {
        ISystemsPlugin Create();
    }
}