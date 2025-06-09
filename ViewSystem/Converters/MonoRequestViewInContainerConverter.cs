namespace UniGame.LeoEcs.ViewSystem.Converters
{
    using System;
    using Components;
    using Leopotam.EcsProto;
    using Leopotam.EcsProto.QoL;
    using Shared.Extensions;

    using UiSystem.Runtime.Settings;
    using UniGame.LeoEcs.Converter.Runtime;
    using UniGame.LeoEcs.Converter.Runtime.Converters;
    using UnityEngine;

#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
#endif
    
    public sealed class MonoRequestViewInContainerConverter : MonoLeoEcsConverter<RequestViewInContainerConverter>
    {

    }

    [Serializable]
    public class RequestViewInContainerConverter : LeoEcsConverter
    {
#if ODIN_INSPECTOR
        [TitleGroup("View Data")]
#endif
        public ViewId view;
        
#if ODIN_INSPECTOR
        [TitleGroup("View Data")]
#endif
        public bool useBusyContainer = false;
        
#if ODIN_INSPECTOR
        [TitleGroup("View Data")]
#endif
        public bool ownView = false;
        
        /// <summary>
        /// Optional Data
        /// </summary>
#if ODIN_INSPECTOR
        [TitleGroup("View Data")]
        [Optional]
#endif
        public string Tag = string.Empty;
        
        /// <summary>
        /// Optional Data
        /// </summary>
#if ODIN_INSPECTOR
        [TitleGroup("View Data")]
        [Optional]
#endif
        public string ViewName = string.Empty;
        
        /// <summary>
        /// Optional Data
        /// </summary>
#if ODIN_INSPECTOR
        [TitleGroup("View Data")]
        [Optional]
#endif
        public bool StayWorld;
        
        public sealed override void Apply(GameObject target, ProtoWorld world, ProtoEntity entity)
        {
            
#if UNITY_EDITOR
            if (string.IsNullOrEmpty(view))
            {
                Debug.LogError($"View Is is Empty for Create in Container {target.name}",target);
                return;
            }
#endif
            
            var requestEntity = world.NewEntity();
            ref var request = ref world.AddComponent<CreateViewInContainerRequest>(requestEntity);

            request.View = view;
            request.UseBusyContainer = useBusyContainer;
            request.Tag = Tag;
            request.ViewName = ViewName;
            request.StayWorld = StayWorld;

            if (ownView) request.Owner = world.PackEntity(entity);
        }       
    }
}