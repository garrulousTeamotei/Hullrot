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
using Robust.Shared.Enums;
using Robust.Client.Graphics;
using Robust.Client.Player;
using Content.Shared._Arcadis.DeepSleep;

namespace Content.Client._Arcadis.DeepSleep;

public sealed class DeepSleepOverlay : Overlay
{
    [Dependency] private readonly IEntityManager _entityManager = default!;
    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;
    [Dependency] private readonly IPlayerManager _playerManager = default!;
    [Dependency] private readonly IEntitySystemManager _sysMan = default!;
    public string ShaderName = "Sleeping";
    public float SleepProgression = 0.0f;
    public override OverlaySpace Space => OverlaySpace.WorldSpace;
    public override bool RequestScreenTexture => true;
    private readonly ShaderInstance _rainbowShader;

    public DeepSleepOverlay()
    {
        IoCManager.InjectDependencies(this);
        _rainbowShader = _prototypeManager.Index<ShaderPrototype>(ShaderName).InstanceUnique();
    }

    protected override void FrameUpdate(FrameEventArgs args)
    {
        if (!_entityManager.TryGetComponent(_playerManager.LocalEntity, out DeepSleepShaderComponent? deepSleepComp))
            return;

        SleepProgression = deepSleepComp.SleepProgression;
    }
    protected override bool BeforeDraw(in OverlayDrawArgs args)
    {
        if (!_entityManager.TryGetComponent(_playerManager.LocalEntity, out EyeComponent? eyeComp))
            return false;

        if (args.Viewport.Eye != eyeComp.Eye)
            return false;

        if (SleepProgression <= 0.0f)
            return false;

        return true;
    }

    protected override void Draw(in OverlayDrawArgs args)
    {
        if (ScreenTexture == null)
            return;


        var handle = args.WorldHandle;
        _rainbowShader.SetParameter("SCREEN_TEXTURE", ScreenTexture);
        _rainbowShader.SetParameter("effectScale", SleepProgression);
        handle.UseShader(_rainbowShader);
        handle.DrawRect(args.WorldBounds, Color.White);
        handle.UseShader(null);
    }
}
