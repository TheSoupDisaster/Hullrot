using Content.Shared.Actions;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Shared._Arcadis.DeepSleep;

/// <summary>
/// Component for DeepSleep shader effect
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class DeepSleepShaderComponent : Component
{
    /// <summary>
    /// How deep asleep have they become?
    /// </summary>
    [DataField, AutoNetworkedField]
    public float SleepProgression = 0.0f; // 0.0f = no shader, 0.5f = halfway asleep, 1f = blank screen
}

/// <summary>
/// The actual DeepSleep control component (why did I seperate it from the shader? UHHHHH JINGLING KEYS)
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class DeepSleepSleepingComponent : Component
{
    /// <summary>
    /// How much progression does the person get per second?
    /// </summary>
    [DataField, AutoNetworkedField]
    public float SleepProgressionSpeed = 0.1f;

    [DataField]
    public ResPath DreamerMap = new ResPath("TOBEWRITTEN");
}

/// <summary>
/// Components given to those stuck in sleep limbo to allow them to "go back" when they want.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class DreamerComponent : Component
{
    [DataField]
    public EntityUid OriginalEntity = new EntityUid(0); // if the ID is 0 you're doomed forever with no escape (lmao rip bozo)

    [DataField]
    public string WakeUpAction = "ActionDevour";
    public EntityUid? WakeUpActionEntity;
}

[RegisterComponent, NetworkedComponent]
public sealed partial class DeepSleepSpawnEntityComponent : Component
{

    [DataField]
    public bool BuckleToOnSpawn = false;

    // Beds with this component will be the default point where an entity going into dream-world will appear.

    // This can be literally anything. If it can be buckled to, they'll be buckled to it.

    // Go ahead, make it an entirely invisible entity for stuff like bitrunners, idc
}

public sealed partial class DeepSleepActionEvent : InstantActionEvent;
