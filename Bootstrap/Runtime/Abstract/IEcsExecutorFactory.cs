namespace UniGame.LeoEcs.Bootstrap
{
    public interface IEcsExecutorFactory
    {
        IEcsExecutor Create(string updateId);
    }
}