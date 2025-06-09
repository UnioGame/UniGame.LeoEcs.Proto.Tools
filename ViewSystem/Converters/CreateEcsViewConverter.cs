using System;
using UniGame.Core.Runtime.SerializableType;
using UniGame.Core.Runtime.SerializableType.Attributes;
using UniGame.LeoEcs.Converter.Runtime;
using UniGame.LeoEcs.ViewSystem.Extensions;
using UniModules.UniGame.UiSystem.Runtime;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace UniGame.LeoEcs.ViewSystem.Converters
{
    using Leopotam.EcsProto;

    [Serializable]
    public class CreateEcsViewConverter : LeoEcsConverter
    {
        
#if ODIN_INSPECTOR
        [DrawWithUnity]
#endif
        [STypeFilter(typeof(IEcsView))]
        public SType viewType;

        public ViewType layoutType = ViewType.Screen;

        public SkinId skinTag;
        
        public override void Apply(GameObject target, ProtoWorld world, ProtoEntity entity)
        {
            world.MakeViewRequest(viewType, layoutType,null,skinTag);
        }
    }
}
