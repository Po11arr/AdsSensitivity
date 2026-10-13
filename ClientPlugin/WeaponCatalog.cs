using System;
using System.Collections.Generic;
using System.Linq;
using Sandbox.Definitions;
using Sandbox.Game.World;
using VRage.Utils;

namespace ClientPlugin;

internal readonly struct WeaponInfo(string subtypeId, string displayName, bool isBaseGame)
{
    public readonly string SubtypeId = subtypeId;
    public readonly string DisplayName = displayName;
    public readonly bool IsBaseGame = isBaseGame;
}

internal static class WeaponCatalog
{
    /// <summary>
    /// Hand weapons that take ammo (rifles, pistols, launchers, modded guns), read from the loaded
    /// definitions so no SubtypeId is hardcoded. Tools have no magazines and are skipped.
    /// </summary>
    public static List<WeaponInfo> ZoomableWeapons()
    {
        var result = new List<WeaponInfo>();
        try
        {
            var definitions = MyDefinitionManager.Static;
            if (definitions == null)
                return result;

            foreach (var item in definitions.GetPhysicalItemDefinitions())
            {
                if (item is not MyWeaponItemDefinition weaponItem || !item.Public)
                    continue;
                if (!definitions.TryGetWeaponDefinition(weaponItem.WeaponDefinitionId, out var weapon))
                    continue;
                if (weapon.AmmoMagazinesId == null || weapon.AmmoMagazinesId.Length == 0)
                    continue;

                result.Add(new WeaponInfo(item.Id.SubtypeName, item.DisplayNameText ?? item.Id.SubtypeName, item.Context?.IsBaseGame ?? false));
            }
        }
        catch (Exception e)
        {
            MyLog.Default.WriteLine($"{Plugin.Name}: failed to list weapons: {e.Message}");
        }

        return result.OrderBy(w => w.DisplayName, StringComparer.OrdinalIgnoreCase).ToList();
    }

    /// <summary>Magnification for the local character's current weapon.</summary>
    public static float CurrentWeaponZoom(Config config)
    {
        var item = MySession.Static?.LocalCharacter?.CurrentWeapon?.PhysicalItemDefinition;
        if (item == null)
            return config.DefaultZoom;
        return config.GetZoom(item.Id.SubtypeName, item.DisplayNameText, item.Context?.IsBaseGame ?? false);
    }
}
