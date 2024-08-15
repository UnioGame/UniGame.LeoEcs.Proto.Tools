namespace UniGame.LeoEcs.ViewSystem.Extensions
{
    using Components;
    using Game.Ecs.UI.EndGameScreens.Systems;
    using LeoEcsLite.LeoEcs.ViewSystem.Systems;
    using Leopotam.EcsLite;
    using Leopotam.EcsProto;
    using Shared.Extensions;
    using UniGame.ViewSystem.Runtime;
    using UniModules.UniCore.Runtime.Utils;
    using UniModules.UniGame.UiSystem.Runtime;

    public static class EcsViewSystemExtensions
    {
        
        public static IProtoSystems ShowQueuedOn<TEvent, TView1, TView2>(
            this IProtoSystems systems,
            ViewType layoutType = ViewType.Window)
            where TEvent : struct
            where TView1 : IView
            where TView2 : IView
        {
            var viewData = new EcsViewData()
            {
                LayoutType = layoutType,
                StayWorld = false,
            };
            
            systems.Add(new ShowQueuedViewOnSystem<TEvent,TView1,TView2>(viewData));
            return systems;
        }
        
        /// <summary>
        /// Show view and mark entity forbidden for same view
        /// </summary>
        public static IProtoSystems ShowSingleOn<TEvent, TView>(
            this IProtoSystems systems,
            ViewType layoutType = ViewType.Window)
            where TEvent : struct
            where TView : IView
        {
            systems.Add(new ShowSingleLayoutViewWhen<TEvent, TView>(layoutType.ToStringFromCache()));
            return systems;
        } 
            
        public static IProtoSystems ShowOn<TView>(
            this IProtoSystems systems,
            EcsFilter filter,
            ViewType layoutType = ViewType.Window)
            where TView : IView
        {
            systems.Add(new ShowLayoutViewWhenSystem<TView>(filter,layoutType.ToStringFromCache()));
            return systems;
        }
        
        public static IProtoSystems CloseOn<TEvent,TView>(this IProtoSystems systems)
            where TEvent : struct
            where TView : IViewModel
        {
            var system = new CloseOnSystem<TEvent>();
            systems.Add(system);
            return systems;
        }
        
        public static IProtoSystems ShowInContainerOn<TEvent,TView>(this IProtoSystems systems,bool useBusy = false,bool ownView = false)
            where TEvent : struct
            where TView : IView
        {
            var data = new ViewContainerSystemData()
            {
                View = typeof(TView).Name,
                UseBusyContainer = useBusy,
                OwnViewBySource = ownView,
            };

            var system = new ShowViewInContainerWhenSystem<TEvent, TView>(data);
            
            systems.Add(system);
            
            return systems;
        }
        
        public static IProtoSystems ShowOn<TEvent, TView>(
            this IProtoSystems systems,
            ViewType layoutType = ViewType.Window)
            where TEvent : struct
            where TView : IView
        {
            systems.Add(new ShowLayoutViewWhenSystem<TEvent, TView>(layoutType.ToStringFromCache()));
            return systems;
        }

        
        public static IProtoSystems ShowOn<TComponent1,TComponent2, TView>(
            this IProtoSystems systems,
            ViewType layoutType = ViewType.Window)
            where TComponent1 : struct
            where TComponent2 : struct
            where TView : IView
        {
            systems.Add(new ShowLayoutViewWhenSystem<TComponent1,TComponent2, TView>(layoutType));
            return systems;
        }

        public static IProtoSystems ShowOn<TView>(
            this IProtoSystems systems,
            EcsFilter filter,
            ViewRequestData viewData)
            where TView : IView
        {
            systems.Add(new ShowViewWhenSystem<TView>(filter,viewData));
            return systems;
        }
        
        public static IProtoSystems ShowOn<TEvent, TView>(
            this IProtoSystems systems,
            ViewRequestData viewData)
            where TEvent : struct
            where TView : IView
        {
            systems.Add(new ShowViewWhenSystem<TEvent, TView>(viewData));
            return systems;
        }
        
        public static IProtoSystems ShowOn<TComponent1,TComponent2, TView>(
            this IProtoSystems systems,
            ViewRequestData viewData)
            where TComponent1 : struct
            where TComponent2 : struct
            where TView : IView
        {
            systems.Add(new ShowViewWhenSystem<TComponent1,TComponent2, TView>(viewData));
            return systems;
        }
    }
}