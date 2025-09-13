using Content.Shared.Abilities.Psionics;
using Content.Shared._Crescent.Psionics;
using Content.Server.Body.Components;
using Content.Server.Body.Systems;
using Content.Server.Popups;
using Content.Shared.Popups;
using Robust.Server.Audio;
using Robust.Shared.Audio;
using Robust.Shared.Player;

namespace Content.Server._Crescent.Psionics;

public sealed class HardbloodArtsSystem : EntitySystem
{

    [Dependency] private readonly SharedPsionicAbilitiesSystem _psionics = default!;
    [Dependency] private readonly BloodstreamSystem _bloodstream = default!;
    [Dependency] private readonly PopupSystem _popup = default!;

    [Dependency] private readonly AudioSystem _sound = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<PsionicComponent, SiphonBloodActionEvent>(OnSiphonBlood);
    }
    private void OnSiphonBlood(Entity<PsionicComponent> ent, ref SiphonBloodActionEvent args)
    {
        var (uid, comp) = ent;
        var user = args.Performer;
        var target = args.Target;

        if (!_psionics.OnAttemptPowerUse(args.Performer, args.PowerName, args.CheckInsulation))
            return;

        if (!TryComp(user, out BloodstreamComponent? userBlood) || !TryComp(target, out BloodstreamComponent? targetBlood))
            return;

        var amp = _psionics.ModifiedAmplification(user, comp);

        var amount = args.SiphonAmount * (args.AmplificationMultiplier * amp);

        if (targetBlood.BloodSolution?.Comp.Solution.Volume < amount)
        {
            _popup.PopupEntity(Loc.GetString("siphon-blood-power-use-no-blood", ("target", target)), user, user, PopupType.Medium);
            return;
        }

        _bloodstream.TryModifyBloodLevel(target, amount);
        _bloodstream.TryModifyBloodLevel(user, -amount);

        _popup.PopupEntity(Loc.GetString("siphon-blood-power-use-user", ("target", target)), user, user, PopupType.Small);
        _popup.PopupEntity(Loc.GetString("siphon-blood-power-use-target", ("user", user)), target, target, PopupType.MediumCaution);

        if (args.PowerSound != null)
            _sound.PlayPvs(args.PowerSound, user);

        DoGlimmerEffects(uid, comp, args);
    }

    private void UseBlood(EntityUid uid, PsionicComponent component, HardbloodArtsTargetEvent args)
    {

    }

    private void DoGlimmerEffects(EntityUid uid, PsionicComponent component, HardbloodArtsTargetEvent args)
    {
        if (!args.DoGlimmerEffects
            || args.MinGlimmer == 0 && args.MaxGlimmer == 0)
            return;

        var minGlimmer = (int)Math.Round(MathF.MinMagnitude(args.MinGlimmer, args.MaxGlimmer)
            * component.CurrentAmplification - component.CurrentDampening);
        var maxGlimmer = (int)Math.Round(MathF.MaxMagnitude(args.MinGlimmer, args.MaxGlimmer)
            * component.CurrentAmplification - component.CurrentDampening);

        _psionics.LogPowerUsed(uid, args.PowerName, minGlimmer, maxGlimmer);
    }
}


