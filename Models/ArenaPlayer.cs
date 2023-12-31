using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Entities.Constants;
using CounterStrikeSharp.API.Modules.Utils;

namespace CS2Multi1v1.Models;

// A single player in a 1v1 arena, wraps a CCSPlayerController and provides functionality related to 1v1

public class ArenaPlayer
{
    public CCSPlayerController PlayerController;
    private int _weaponPreferences;

    public ArenaPlayer(CCSPlayerController playerController)
    {
        PlayerController = playerController;
        _weaponPreferences = 0; //todo: impl
    }

    public void ResetPlayerWeapons(RoundType roundType)
    {
        string knifeName = EnumUtils.GetEnumMemberAttributeValue(CsItem.Knife) ?? "";

        if (PlayerController?.Pawn?.Value?.IsValid == null || PlayerController.Pawn!.Value!.IsValid == false) return;

        // Remove default (spawn) weapons
        foreach (CHandle<CBasePlayerWeapon> weapon in PlayerController!.Pawn!.Value!.WeaponServices!.MyWeapons)
        {
            if (!weapon.IsValid || !weapon.Value!.IsValid || weapon.Value!.DesignerName == knifeName) continue;
            PlayerController.ExecuteClientCommand("slot3");
            PlayerController.Pawn.Value.RemovePlayerItem(weapon.Value);
            weapon.Value!.Remove();
        }

        // Give round items
        if (roundType.PrimaryWeapon != null)
        {
            CsItem item = (CsItem)roundType.PrimaryWeapon;
            PlayerController.GiveNamedItem(item);
        }
        if (roundType.SecondaryWeapon != null)
        {
            CsItem item = (CsItem)roundType.SecondaryWeapon;
            PlayerController.GiveNamedItem(item);
        }
        if (roundType.Armour)
        {
            PlayerController.GiveNamedItem(CsItem.AssaultSuit);
        }
        if (roundType.Helmet)
        {
            PlayerController.GiveNamedItem(CsItem.KevlarHelmet);
        }
    }

    public void PrintToChat(string text)
    {
        PlayerController.PrintToChat($" {ChatColors.Olive}  CS2Multi1v1 \u2022 {ChatColors.Default}{text}");
    }
}
