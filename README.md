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

### Features

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


# Aspects

# ECS DI

# Ownership

# LifeTime

