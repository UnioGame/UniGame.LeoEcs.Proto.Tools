namespace UniGame.LeoEcs.Converter.Runtime.Abstract
{
    using Core.Runtime;
    using Leopotam.EcsLite;
    using Leopotam.EcsProto;

#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
#endif

    
    public interface IEcsComponentConverter : 
        ILeoEcsConverterStatus, ISearchFilterable
    {
        public string Name { get; }
        
        void Apply(ProtoWorld world, ProtoEntity entity);
    }
}