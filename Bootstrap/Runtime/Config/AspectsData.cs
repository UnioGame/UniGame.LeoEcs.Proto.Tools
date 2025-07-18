﻿namespace UniGame.Ecs.Bootstrap.Runtime.Config
{
    using System;
    using System.Collections.Generic;
    using Core.Runtime.SerializableType;
    using LeoEcs.Bootstrap;
    using LeoEcs.ViewSystem.Aspects;

    using UnityEngine;

#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
#endif
    
    [Serializable]
    public class AspectsData
    {
        public bool autoRegisterAspects = true;
        
#if ODIN_INSPECTOR 
        [Searchable(FilterOptions = SearchFilterOptions.ISearchFilterableInterface)]
        [ListDrawerSettings(ListElementLabelName = "@name")]
#endif
        public List<AspectData> aspects = new List<AspectData>();
        
#if ODIN_INSPECTOR
        [PropertySpace]
#endif

        [SerializeReference]
        public List<IProtoAspectFactory> factories = new(){new ViewPoolAspectProvider()};
        
    }

    [Serializable]
    public class AspectData 
#if ODIN_INSPECTOR
        : ISearchFilterable
#endif
    {
        public string name;
        public bool enabled;
        public SType aspectType = new SType();
        
        public bool IsMatch(string searchString)
        {
            if (string.IsNullOrEmpty(searchString)) return true;
            if(name.Contains(searchString)) return true;
            if(aspectType.TypeName!= null && aspectType.TypeName.Contains(searchString)) return true;
            return false;
        }
    }
}