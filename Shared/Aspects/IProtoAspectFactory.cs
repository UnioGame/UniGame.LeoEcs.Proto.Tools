namespace UniGame.LeoEcs.Bootstrap
{
    using Leopotam.EcsProto;

    public interface IProtoAspectFactory
    {
        IProtoAspect Create();

#if UNITY_EDITOR
        void EditorInitialize();
#endif
        
    }
    
}