namespace UniGame.LeoEcs.Bootstrap.Runtime.Abstract
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