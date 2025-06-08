namespace UniGame.LeoEcs.ViewSystem.Components
{
    using System;
    using Leopotam.EcsLite;
    using Leopotam.EcsProto;
    using Leopotam.EcsProto.QoL;
    using Proto;
    using Runtime.Utils;
    using UniModules.UniGame.UiSystem.Runtime;

    [Serializable]
    public struct CreateLayoutViewRequest : IProtoAutoReset<CreateLayoutViewRequest>
    {
        public string View;
        public string LayoutType;
        public ProtoPackedEntity Owner;

        public void SetHandlers(IProtoPool<CreateLayoutViewRequest> pool) => pool.SetResetHandler(AutoReset);
        
        public static void AutoReset(ref CreateLayoutViewRequest c)
        {
            c.View = string.Empty;
            c.LayoutType = ViewType.Window.ToStringFromCache();
            c.Owner = default;
        }
    }
}