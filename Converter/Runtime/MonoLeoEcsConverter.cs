﻿namespace UniGame.LeoEcs.Converter.Runtime
{
    using System;
    using Abstract;
    using Leopotam.EcsLite;
    using Leopotam.EcsProto;
    using UnityEngine;

    [Serializable]
    [RequireComponent(typeof(ProtoEcsMonoConverter))]
    public abstract class MonoLeoEcsConverter : MonoBehaviour, IEcsComponentConverter
    {
        [SerializeField]
        private bool _isEnabled = true;

        public string Name => GetType().Name;
        
        public void Apply(ProtoWorld world, ProtoEntity entity)
        {
            Apply(gameObject, world, entity);
        }

        public bool IsPlaying => Application.isPlaying;
        
        public virtual bool IsEnabled => _isEnabled;
        
        public abstract void Apply(GameObject target, ProtoWorld world, ProtoEntity entity);
        
        public virtual bool IsMatch(string searchString)
        {
            if (string.IsNullOrEmpty(searchString)) return true;
            if(searchString.Contains(name, StringComparison.OrdinalIgnoreCase))
                return true;
            return false;
        }
    }
}