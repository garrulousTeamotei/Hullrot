using Content.Client._Arcadis.DeepSleep;
using Content.Shared._Arcadis.DeepSleep;
using Content.Shared.Drugs;
using Robust.Client.Graphics;
using Robust.Client.Player;
using Robust.Shared.Player;

namespace Content.Client._Arcadis.DeepSleep;

/// <summary>
///     System to handle drug related overlays.
/// </summary>
public sealed class OverlaySystem : EntitySystem
{
    [Dependency] private readonly IPlayerManager _player = default!;
    [Dependency] private readonly IOverlayManager _overlayMan = default!;

    private DeepSleepOverlay _overlay = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<DeepSleepShaderComponent, ComponentInit>(OnInit);
        SubscribeLocalEvent<DeepSleepShaderComponent, ComponentShutdown>(OnShutdown);

        SubscribeLocalEvent<DeepSleepShaderComponent, LocalPlayerAttachedEvent>(OnPlayerAttached);
        SubscribeLocalEvent<DeepSleepShaderComponent, LocalPlayerDetachedEvent>(OnPlayerDetached);

        _overlay = new();
    }

    private void OnPlayerAttached(EntityUid uid, DeepSleepShaderComponent component, LocalPlayerAttachedEvent args)
    {
        _overlayMan.AddOverlay(_overlay);
    }

    private void OnPlayerDetached(EntityUid uid, DeepSleepShaderComponent component, LocalPlayerDetachedEvent args)
    {
        _overlay.SleepProgression = 0;
        _overlayMan.RemoveOverlay(_overlay);
    }

    private void OnInit(EntityUid uid, DeepSleepShaderComponent component, ComponentInit args)
    {
        if (_player.LocalEntity == uid)
            _overlayMan.AddOverlay(_overlay);
    }

    private void OnShutdown(EntityUid uid, DeepSleepShaderComponent component, ComponentShutdown args)
    {
        if (_player.LocalEntity == uid)
        {
            _overlay.SleepProgression = 0.0f;
            _overlayMan.RemoveOverlay(_overlay);
        }
    }
}
