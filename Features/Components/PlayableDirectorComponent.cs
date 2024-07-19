namespace Game.Ecs.Core.Components
{
    using System;
    using UnityEngine.Playables;

    /// <summary>
    /// Компонент со ссылкой на PlayableDirector.
    /// </summary>
#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [Serializable]
    public struct PlayableDirectorComponent
    {
        public PlayableDirector Value;
    }
}