using Content.Shared.Containers.ItemSlots;
using Content.Shared.Coordinates;
using Robust.Shared.Audio;
using Content.Shared.Audio;
using Robust.Shared.Network;
using Robust.Shared.Containers;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Audio.Systems;
using Content.Shared.Popups;
using Content.Shared.Examine;
using Content.Shared.Interaction;
using Robust.Shared.Timing;

namespace Content.Shared._Arcadis.DeepSleep;

public sealed class DeepSleepSystem : EntitySystem
{
    [Dependency] private readonly IPrototypeManager _protoMan = default!;
    public override void Initialize()
    {
        base.Initialize();

        // SubscribeLocalEvent<DeepSleepSleepingComponent, ExaminedEvent>(OnDeepSleepAction);
        // SubscribeLocalEvent()
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        foreach (var comp in EntityManager.EntityQuery<DeepSleepSleepingComponent>())
        {
            // so sue me for using comp.Owner, whaddaya WANT me to do here???
            if (!TryComp<DeepSleepShaderComponent>(comp.Owner, out var shadercomp))
                EnsureComp<DeepSleepShaderComponent>(comp.Owner, out shadercomp);

            shadercomp.SleepProgression += comp.SleepProgressionSpeed;
            if (shadercomp.SleepProgression <= 0)
            {
                RemComp<DeepSleepShaderComponent>(comp.Owner);
                RemComp<DeepSleepSleepingComponent>(comp.Owner);
            }
        }

        // Tyler is going to kill me for this system if it is ever upstreamed. Count my days.
    }
}
