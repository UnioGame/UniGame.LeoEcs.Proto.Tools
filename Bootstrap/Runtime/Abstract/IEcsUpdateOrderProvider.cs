using UniGame.LeoEcs.Bootstrap;

namespace UniGame.Ecs.Bootstrap.Runtime.Config
{
    public interface IEcsUpdateOrderProvider
    {
        IEcsExecutor Create();
    }
}