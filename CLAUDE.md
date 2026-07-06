# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

**Frostfall** — RTS (Real-Time Strategy) de supervivencia desarrollado en Unity 6 (6000.3.8f1) como Trabajo Fin de Máster. El jugador construye y gestiona una base, recluta unidades y defiende contra oleadas de enemigos en un entorno de invierno. El pipeline de renderizado es URP (Universal Render Pipeline 17.3.0).

Hay dos escenas: `Assets/Scenes/Menu.unity` (menú principal) y `Assets/Scenes/Game.unity` (partida).

## Workflow de Unity

Este proyecto **no tiene CLI de compilación separada**. El flujo habitual es:

- Abrir el proyecto en Unity Hub con Unity 6000.3.8f1.
- Editar scripts en el IDE (Rider o Visual Studio) — Unity recompila automáticamente al detectar cambios.
- Ejecutar el juego desde el Editor usando el botón Play.
- Para builds: `File > Build Settings` en el Editor de Unity.

No hay comandos de build, lint ni test ejecutables desde terminal para este proyecto.

## Arquitectura General

### Event System (ScriptableObject Events)

El núcleo de comunicación entre sistemas usa eventos genéricos basados en ScriptableObjects:

```
Assets/Scripts/Events/GameEvent.cs          ← clase base genérica GameEvent<T>
Assets/Events/*.asset                       ← instancias concretas (UnitSelectedEvent, ResourceEvent, etc.)
```

Cada evento es un asset que los MonoBehaviours referencian via `[SerializeField]`. Los suscriptores llaman a `Register`/`Unregister` en `Awake`/`OnDestroy`. Este patrón desacopla completamente los sistemas — no hay referencias directas entre, por ejemplo, `PlayerController` y `UIManager`.

### Jerarquía de entidades

```
CommonActions (MonoBehaviour abstracto)
├── BaseUnit : CommonActions, IMoveable, IHealable
│   └── EnemyController
└── BaseBuilding : CommonActions, IHealable
    ├── ProductionBuilding
    ├── RecruitBuilding
    ├── AttackerBuilding
    ├── BuilderBuilding
    └── ExplodingBuilding
```

`CommonActions` (`Assets/Scripts/Units/CommonActions.cs`) es la base compartida de unidades y edificios. Implementa selección (con `selectionDecal`), salud, muerte, visibilidad (fog of war) y el array de `BaseAction[]` que determina qué acciones aparecen en la UI.

### Sistema de Acciones

Las acciones son **ScriptableObjects** (`BaseAction : ScriptableObject, IAction`). Cada entidad tiene un array `Actions[]` serializado que la UI lee para generar botones. Las acciones soportan:
- `CanExecute` / `Blocked` — validan si se puede ejecutar
- `Execute(ActionInfo)` — lógica de la acción
- `Restrictions[]` — array de `Restriction` SOs que validan placement (NavMesh, capas, recursos, fog of war)
- `UseClickToExecute` — si true, la acción espera un click en el mapa tras activarse

Acciones clave: `MoveAction`, `AttackAction`, `HealAction`, `PatrolAction`, `BuildBuildingAction`, `RecruitUnitAction`, `RepairAction`, `DestroyBuildingAction`.

`SetCommandsOverrides(BaseAction[])` en `CommonActions` permite sobreescribir temporalmente el array de acciones (p.ej. durante construcción de edificio).

### Behaviour Trees (Unity Behavior)

Las unidades y edificios usan **Unity Behavior** (`com.unity.behavior`) para su IA. Los nodos personalizados están en `Assets/Scripts/BehaviourTrees/`. Los `BehaviorGraphAgent` reciben variables via `SetVariableValue(key, value)` desde los scripts de C#:

- Unidades: variable clave `"UnitActions"` (enum `UnitActions`: Stop, Move, Attack, Patrol, Heal, Repair)
- Edificios: variable clave `"BuildingActions"` (enum `BuildingActions`: Build, Repair, etc.)

### Game Loop

`GameManager.cs` orquesta el loop de juego:
1. Corrutina `FillWatch` — cuenta el tiempo entre oleadas (progressbar circular)
2. Al acabar el tiempo, lanza `StartWaveEvent` → `EnemySpawner.GenerateWave()`
3. Cuando todos los enemigos mueren, `FinishWaveEvent` → siguiente oleada
4. Victoria: wave > totalWaves. Game Over: se destruye el `CommandPost` (tag `"CommandPost"`)

La dificultad se guarda en `PlayerPrefs` con key `"dificultyMode"` (enum `DificultyMode`: Easy, Medium, Hard, Infinite) y modifica `timeBetweenWaves` y multiplicadores de enemigos.

### Recursos

Seis tipos (enum `ResourcesType`): Food, Wood, Stone, Iron, Electricity, Population. Cada uno tiene un SO asset en `Assets/Resources/`. Las operaciones de recursos se comunican via `ResourceEvent` con el struct `ResourceOP(SO_Resource, delta, minValue)`. Los edificios de producción (`ProductionBuilding`) generan recursos periódicamente; los edificios de recursos requieren placement sobre `ResourceArea` con el tipo correcto.

### PlayerController

`Assets/Scripts/Player/PlayerController.cs` gestiona toda la entrada del jugador:
- Selección simple y por caja (drag)
- Grupos de unidades (Ctrl+número / número)
- Movimiento de cámara por bordes de pantalla y WASD
- Zoom con scroll
- Placement de edificios (preview con materiales Ok/Ko basado en `Restriction.CanPlace`)
- Acción activa (`selectedAction`) que determina qué hace el click derecho/izquierdo

### Fog of War

`FogOfWar` (singleton) usa una Camera ortogonal con `RenderTexture` para determinar visibilidad. En `LateUpdate` lee los píxeles y llama a `SetVisible(bool)` en todos los `IHideable` registrados. Los objetos Enemy implementan `IHideable` a través de `CommonActions`.

### UI

- `UIManager.cs` — gestiona qué panel de info se muestra (unidad única, múltiples unidades, edificio)
- `UIActions.cs` + `UIActionButton.cs` — generan los botones de acción dinámicamente del array `Actions[]`
- `UIResources.cs` — muestra los recursos actuales, escucha `ResourceEvent`
- `UIMinimapManager.cs` — minimap con click para navegar via `MinimapClickEvent`
- Los menús de juego (pausa, victoria, derrota) están en `Assets/Scripts/UI/GameMenus/`
- El menú principal y opciones en `Assets/Scripts/UI/MainMenu/`

### Audio

`AudioManager` es un MonoBehaviour con métodos estáticos para efectos de sonido (`SetAudioClips` + `PlayAudio`). Usa `AudioMixer` para control de volumen. Los clips de selección y movimiento los tienen los propios SOs de unidades (`SO_BaseUnit.SelectionAudioClips`).

## Convenciones del Proyecto

- **ScriptableObjects de configuración** tienen prefijo `SO_` (p.ej. `SO_BaseUnit`, `SO_Building`, `SO_Resource`, `SO_AttackInfo`)
- **Eventos** se definen como clases vacías que heredan de `GameEvent<T>` (p.ej. `public class UnitSelectedEvent : GameEvent<CommonActions> {}`)
- Los assets de recursos en runtime van en `Assets/Resources/` para poder cargarlos con `Resources.Load`
- Los prefabs de unidades y enemigos están en `Assets/Units/` y `Assets/Enemies/`; los de edificios en `Assets/Buildings/`
