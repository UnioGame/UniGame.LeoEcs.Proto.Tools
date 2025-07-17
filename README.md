# UniGame.LeoEcs.Proto.Tools

# Overview

The leoecs.proto.tools module provides a set of tools and utilities to enhance the functionality of the LeoECS Proto framework. This module includes aspects, systems, and extensions that facilitate the management of entity ownership, lifecycle, and other common tasks in ECS-based projects.

# Installation

To install the leoecs.proto.tools module, add the following dependency to your project:

//TODO right now we are preparing the package for the release

> **Note:** The package depending from Leo Ecs 
> Proto package, so you need to install it first.

```json
{
  "dependencies": {
    "com.unigame.leoecs.proto.tools": "1.0.0"
  }
}
```

# Configuration

To enabled package add the following define to Scripting Define Symbols in Player Settings:

```json
LEO_ECS_PROTO_ENABLED
```

![proto game define](https://github.com/UnioGame/UniGame.LeoEcs.Proto.Tools/blob/main/Assets/ecsproto_define.png)

## Settings

To Create of ECS call a menu:

```
- Create/ECS Proto/Create ECS Configuration
```

Configuration parts:

- **Ecs Update Map** - map of ecs workers, that's define how to update ecs systems
- **Ecs Features Configuration** - define features of ECS and order of execution

# Features

**Feature - is a unit of ECS functionality, that implements some game logic with multiple systems/components/aspects.**

Features can be added to the ECS Configuration by two ways:

- As a scriptable object

```csharp
[CreateAssetMenu(menuName = "ECS Proto/Features/Core Feature", fileName = "Core Feature")]
public class CoreFeatureAsset : BaseLeoEcsFeature
{
    public CoreFeature coreFeature = new();
    
    public override async UniTask InitializeAsync(IProtoSystems ecsSystems)
    {
        await coreFeature.InitializeAsync(ecsSystems);
    }
}
```

- As regular class with **SerializedReference** attribute

```csharp
[Serializable]
public class CoreFeature : EcsFeature,IAutoInitFeature
{
    public TimerFeature timerFeature = new();
    public GameTimeFeature gameTimeFeature = new();
    public OwnershipFeature ownershipFeature = new();
    
    protected override async UniTask OnInitializeAsync(IProtoSystems ecsSystems)
    {
        await ownershipFeature.InitializeAsync(ecsSystems);
        
        ecsSystems.Add(new AddTransformComponentsSystem());
        ecsSystems.Add(new UpdateTransformDataSystem());
        
        await gameTimeFeature.InitializeAsync(ecsSystems);
        
        //ecsSystems.Add(new KillMeNextTimeHandleSystem());
        ecsSystems.Add(new ProcessDestroySilentSystem());
        
        ecsSystems.Add(new UpdateRenderStatusSystem());
        ecsSystems.Add(new DisableColliderSystem());
        ecsSystems.Add(new ProcessDeadSimpleEntitiesSystem());
        ecsSystems.Add(new ProcessDeadTransformEntitiesSystem());
    }
}
```

![ecs configuration](https://github.com/UnioGame/UniGame.LeoEcs.Proto.Tools/blob/main/Assets/ecsproto_add_feature.png)


Predefined features can be found in the: https://github.com/UnioGame/UniGame.LeoEcs.Proto.Features

![predefined features](https://github.com/UnioGame/UniGame.LeoEcs.Proto.Tools/blob/main/Assets/ecsproto_features.png)

# Multiple Worlds Support

Ecs Services allow you to create multiple worlds and manage them

To predefine the worlds names you can create a scriptable object with the following code:

```csharp
[CreateAssetMenu(menuName = "ECS Proto/ECS Worlds", fileName = nameof(EcsWorldsConfiguration),order = 0)]
public class EcsWorldsConfiguration : ScriptableObject
```

It's optional, you can use string world id at runtime, but this configuration allows you select
one of the predefined worlds in dropdown in some inspectors like **ProtoEcsMonoConverter***



# Aspects

# ECS DI

# Ownership

# LifeTime

# Prefab Converters

To make you prefab to be converted to ECS entity you need to add **ProtoEcsMonoConverter** mono component to the prefab

When you can register in the converter your components providers

1. Serializable Converter - implements **IEcsComponentConverter** interface

```csharp
    public interface IEcsComponentConverter : 
        ILeoEcsConverterStatus, ISearchFilterable
    {
        public string Name { get; }
        
        void Apply(ProtoWorld world, ProtoEntity entity);
    }
```

You can use as a base class of the serializable converter - **LeoEcsConverter** 

2. ScriptableObject Converter - **EcsConverterAsset**

SO converters allow you to make presets of converters and use them in different prefabs

# Views

- Feature for support of View System package in ECS Proto.

```csharp
    [CreateAssetMenu(menuName = "ECS Proto/Features/Views Feature", fileName = "ECS Views Feature")]
    public class ViewSystemFeature
```

Create a feature asset and add to the ECS Configuration

## View Filter

Allow to filter views in the Ecs World by it's model

view filter will check few components on the entity

- ViewModelComponent
- ViewComponent
- ViewComponent<TModel>
- ViewInitializedComponent

```csharp
    //create view filter for TViewModel
    public ProtoIt chain = ViewIt.ViewChain().Inc<TComponent>().End();
    public ProtoIt chain2 = ViewIt.ViewChain<TViewModel2>().Inc<TComponent2>().End();
    public ProtoIt chain3 = It.Chain<GameObjectComponent>().ViewChain<TViewModel3>().End();
```

you can check the source code in 'EcsViewExtensions.cs'
