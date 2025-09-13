using Robust.Shared.Prototypes;
using Content.Shared.Actions;
using Content.Server._Crescent.Psionics;
using Robust.Shared.Audio;

namespace Content.Shared._Crescent.Psionics;

public sealed partial class SiphonBloodActionEvent : HardbloodArtsTargetEvent
{
    /// <summary>
    ///    Multiplier to Amplification for how much blood to siphon.
    /// </summary>
    [DataField]
    public float AmplificationMultiplier = 1;

    /// <summary>
    ///    Amount of blood to siphon as a base value.
    /// </summary>
    [DataField]
    public int SiphonAmount = 10;

    /// <summary>
    ///    The sound to play when the power is used.
    /// </summary>
    [DataField]
    public SoundPathSpecifier PowerSound = new SoundPathSpecifier("/Audio/Effects/gib1.ogg");
}
