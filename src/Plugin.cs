using CounterStrikeSharp.API.Core;
using FixVectorLeak.src.Structs;

namespace FixVectorLeak.src;

public class Plugin : BasePlugin
{
    public override string ModuleName => "FixVectorLeak";
    public override string ModuleVersion => "1.0.0";

    private readonly QAngle_t _zeroAng = new();

    public unsafe override void Load(bool hotReload)
    {
        AddCommand("css_superjump", string.Empty, (p, i) =>
        {
            if (p?.PlayerPawn.Value is null) return;

            var vel = p.PlayerPawn.Value.AbsVelocity.ToVector_t() with
            {
                Z = 350
            };

            p.PlayerPawn.Value.Teleport(velocity:vel);
        });

        AddCommand("css_movefwd", string.Empty, (p, i) =>
        {
            if (p?.PlayerPawn.Value is null) return;

            var (fwd, _, _) = p.PlayerPawn.Value.AbsRotation!.AngleVectors();

            fwd.Scale(25);

            p.PlayerPawn.Value.Teleport(p.PlayerPawn.Value.AbsOrigin!.ToVector_t() + fwd);
        });

        AddCommand("css_moveright", string.Empty, (p, i) =>
        {
            if (p?.PlayerPawn.Value is null) return;

            var (_, right, _) = p.PlayerPawn.Value.AbsRotation!.AngleVectors();

            right.Scale(25);

            p.PlayerPawn.Value.Teleport(p.PlayerPawn.Value.AbsOrigin!.ToVector_t() + right);
        });

        AddCommand("css_moveleft", string.Empty, (p, i) =>
        {
            if (p?.PlayerPawn.Value is null) return;

            var (_, left, _) = p.PlayerPawn.Value.AbsRotation!.AngleVectors();

            left.Scale(25);

            p.PlayerPawn.Value.Teleport(p.PlayerPawn.Value.AbsOrigin!.ToVector_t() - left);
        });

        AddCommand("css_zeroang", string.Empty, (p, i) =>
        {
            if (p?.PlayerPawn.Value is null) return;

            p.PlayerPawn.Value.Teleport(angles: _zeroAng);
        });

        RegisterEventHandler<EventWeaponFire>(OnWeaponFire);
    }

    private HookResult OnWeaponFire(EventWeaponFire @event, GameEventInfo info)
    {
        var player = @event.Userid;

        if ( player?.PlayerPawn.Value is null ) return HookResult.Continue;

        player.PlayerPawn.Value.EyeAngles.AngleVectors(out var fwd, out _, out _); // or: var (fwd, right, up) = player.PlayerPawn.Value.EyeAngles.AngleVectors();

        fwd.Scale(-200);

        var velocity = player.PlayerPawn.Value.AbsVelocity.ToVector_t() with
        {
            Z = 250
        } + fwd;

        player.PlayerPawn.Value.Teleport(velocity:velocity);

        return HookResult.Continue;
    }
}