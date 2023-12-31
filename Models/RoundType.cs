using CounterStrikeSharp.API.Modules.Entities.Constants;

namespace CS2Multi1v1.Models;

// Weapons and various config for a single round-type

public struct RoundType
{
    public string Name;
    public CsItem? PrimaryWeapon;
    public CsItem? SecondaryWeapon;
    public bool UsePreferredPrimary;
    public bool UsePreferredSecondary;
    public bool Armour;
    public bool Helmet;

    public RoundType(string name, CsItem? primary, CsItem? secondary, bool usePreferredPrimary = false, bool usePreferredSecondary = false, bool armour = true, bool helmet = true)
    {
        Name = name;
        PrimaryWeapon = primary;
        SecondaryWeapon = secondary;
        UsePreferredPrimary = usePreferredPrimary;
        UsePreferredSecondary = usePreferredSecondary;
        Armour = armour;
        Helmet = helmet;
    }

    public static readonly RoundType Rifle = new RoundType("AK47", CsItem.AK47, CsItem.USPS);
    public static readonly RoundType Pistol = new RoundType("Pistol", null, CsItem.USPS);
    public static readonly RoundType Scout = new RoundType("Scout", CsItem.Scout, CsItem.USPS);
    public static readonly RoundType Awp = new RoundType("AWP", CsItem.AWP, CsItem.Deagle);
    public static readonly RoundType Deagle = new RoundType("Deagle", null, CsItem.Deagle);
    public static readonly RoundType Smg = new RoundType("SMG", CsItem.GalilAR, CsItem.P250);
}
