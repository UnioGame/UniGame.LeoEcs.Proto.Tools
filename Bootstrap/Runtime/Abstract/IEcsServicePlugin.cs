namespace UniGame.LeoEcs.Bootstrap
{
    using Core.Runtime;
    using Runtime;

    public interface IEcsServicePlugin
    {
        void Init(EcsFeatureSystems ecsSystems);
        void PreInit(IContext context);
        void PostInit();
    }
}