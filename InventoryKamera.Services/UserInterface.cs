namespace InventoryKamera;

/// <summary>
/// Internal façade used by all Services-layer code.
/// Every method delegates to <see cref="UserInterfaceBridge"/> so the
/// host (WinForms) can register the real implementations at startup.
/// </summary>
internal static class UserInterface
{
    public static void SetNavigation_Image(Bitmap bitmap)
        => UserInterfaceBridge.SetNavigationImage?.Invoke(bitmap);

    public static void AddError(string error)
        => UserInterfaceBridge.AddError?.Invoke(error);

    public static void UnexpectedError(string message)
        => UserInterfaceBridge.UnexpectedError?.Invoke(message);

    public static void SetGearPictureBox(Bitmap bm)
        => UserInterfaceBridge.SetGearPictureBox?.Invoke(bm);

    public static void SetGear(Bitmap bm, object gear)
        => UserInterfaceBridge.SetGear?.Invoke(bm, gear);

    public static void IncrementWeaponCount()
        => UserInterfaceBridge.IncrementWeaponCount?.Invoke();

    public static void IncrementArtifactCount()
        => UserInterfaceBridge.IncrementArtifactCount?.Invoke();

    public static void IncrementCharacterCount()
        => UserInterfaceBridge.IncrementCharacterCount?.Invoke();

    public static void SetWeapon_Max(int max)
        => UserInterfaceBridge.SetWeaponMax?.Invoke(max);

    public static void SetArtifact_Max(int max)
        => UserInterfaceBridge.SetArtifactMax?.Invoke(max);

    public static void ResetCharacterDisplay()
        => UserInterfaceBridge.ResetCharacterDisplay?.Invoke();

    public static void SetCharacter_NameAndElement(Bitmap bm, string name, string element)
        => UserInterfaceBridge.SetCharacterNameAndElement?.Invoke(bm, name, element);

    public static void SetCharacter_Level(Bitmap bm, int level, int maxLevel)
        => UserInterfaceBridge.SetCharacterLevel?.Invoke(bm, level, maxLevel);

    public static void SetCharacter_Constellation(int constellation)
        => UserInterfaceBridge.SetCharacterConstellation?.Invoke(constellation);

    public static void SetCharacter_Talent(Bitmap bm, string level, int index)
        => UserInterfaceBridge.SetCharacterTalent?.Invoke(bm, level, index);

    public static void SetMainCharacterName(string name)
        => UserInterfaceBridge.SetMainCharacterName?.Invoke(name);

    public static void SetMaterial(Bitmap nameplate, Bitmap quantity, string name, int count)
        => UserInterfaceBridge.SetMaterial?.Invoke(nameplate, quantity, name, count);

    public static void SetMora(Bitmap bm, int count)
        => UserInterfaceBridge.SetMora?.Invoke(bm, count);
}
