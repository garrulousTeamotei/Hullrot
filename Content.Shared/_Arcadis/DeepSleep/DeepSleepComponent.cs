using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared._Arcadis.DeepSleep;

/// <summary>
/// Component for DeepSleep shader effect
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
//[Access(typeof(ComputerDiskSystem))]
public sealed partial class DeepSleepShaderComponent : Component
{
    /// <summary>
    /// How deep asleep have they become?
    /// </summary>
    [DataField, AutoNetworkedField]
    public float SleepProgression = 0.0f; // 0.0f = no shader, 0.5f = halfway asleep, 1f = blank screen
}
