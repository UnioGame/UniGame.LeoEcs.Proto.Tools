namespace UniGame.LeoEcs.ViewSystem.Components
{
    using System;
    using Leopotam.EcsLite;
    using Leopotam.EcsProto;
    using Leopotam.EcsProto.QoL;

    [Serializable]
    public class ViewContainerSystemData
    {
        public string View = string.Empty;
        public bool UseBusyContainer = false;
        public ProtoPackedEntity Owner = default;
        public bool OwnViewBySource = false;
        public bool Single = false;
        public string Tag = string.Empty;
        public string ViewName = string.Empty;
        public bool StayWorld = false;
        public ProtoIt FilterMask = null;
    }
}