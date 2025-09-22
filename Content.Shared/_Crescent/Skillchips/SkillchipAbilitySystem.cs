using Content.Shared.Emag.Systems;

namespace Content.Shared._Crescent.Skillchips;

public abstract partial class SkillchipAbilitySystem : EntitySystem
{
    [Dependency] private readonly EmagSystem _emag = default!;
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<SkillchipImplantHolderComponent, AirlockHackEvent>(OnAirlockHack);
    }

    private void OnAirlockHack(Entity<SkillchipImplantHolderComponent> ent, ref AirlockHackEvent args)
    {
        _emag.DoEmagEffect(ent.Owner, args.Target);
    }
}
