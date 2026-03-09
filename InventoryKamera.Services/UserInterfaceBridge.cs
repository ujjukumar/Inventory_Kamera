namespace InventoryKamera;

#nullable enable

/// <summary>
/// Public bridge that allows the WinForms (or any host) layer to register
/// real UI callbacks. The internal <see cref="UserInterface"/> stub in this
/// assembly delegates every call through these delegates.
/// </summary>
public static class UserInterfaceBridge
{
    // Navigation
    public static Action<Bitmap>? SetNavigationImage;

    // Errors
    public static Action<string>? AddError;
    public static Action<string>? UnexpectedError;

    // Gear (weapons / artifacts)
    public static Action<Bitmap>? SetGearPictureBox;
    public static Action<Bitmap, object>? SetGear;

    // Counters
    public static Action? IncrementWeaponCount;
    public static Action? IncrementArtifactCount;
    public static Action? IncrementCharacterCount;
    public static Action<int>? SetWeaponMax;
    public static Action<int>? SetArtifactMax;

    // Character display
    public static Action? ResetCharacterDisplay;
    public static Action<Bitmap, string, string>? SetCharacterNameAndElement;
    public static Action<Bitmap, int, int>? SetCharacterLevel;
    public static Action<int>? SetCharacterConstellation;
    public static Action<Bitmap, string, int>? SetCharacterTalent;
    public static Action<string>? SetMainCharacterName;

    // Materials
    public static Action<Bitmap, Bitmap, string, int>? SetMaterial;
    public static Action<Bitmap, int>? SetMora;
}
