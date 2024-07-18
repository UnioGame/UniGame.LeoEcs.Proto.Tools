namespace UniGame.LeoEcs.ViewSystem.Converters
{
    using UniGame.LeoEcs.Converter.Runtime;
    using UniGame.UiSystem.Runtime;
    using UnityEngine;
    
    [RequireComponent(typeof(ProtoEcsMonoConverter))]
    [RequireComponent(typeof(EcsViewConverter))]
    public class EcsView : ViewBase, IEcsView
    {
        
    }
}
