using System.Runtime.InteropServices;

namespace InventoryKamera;

internal static class UserInterface
{
    public static void SetNavigation_Image(Bitmap bitmap)
    {
        // No-op for now, or maybe fire an event
    }

    public static void AddLog(string message)
    {
        // No-op, should use ILogger instead
    }

    public static void SetBitmap(Bitmap bitmap)
    {
         // No-op
    }

    public static void ResetCharacterDisplay()
    {
        // No-op
    }

    public static void IncrementCharacterCount()
    {
        // No-op
    }

    public static void AddError(string error)
    {
        // No-op
    }

    public static void SetCharacter_NameAndElement(Bitmap bm, string name, string element)
    {
        // No-op
    }

    public static void SetCharacter_Level(Bitmap bm, int level, int maxLevel)
    {
        // No-op
    }

    public static void SetCharacter_Constellation(int constellation)
    {
        // No-op
    }

    public static void SetGearPictureBox(Bitmap bm)
    { 
        // No-op 
    }

    public static void SetGear(Bitmap bm, object gear)
    {
        // No-op
    }

    public static void IncrementWeaponCount()
    {
        // No-op
    }

    public static void IncrementArtifactCount()
    {
        // No-op
    }

    public static void UnexpectedError(string message) 
    {
        // No-op
    }

    public static void SetCharacter_Talent(Bitmap bm, string level, int index)
    {
        // No-op
    }

    public static void SetWeapon_Max(int max)
    {
        // No-op
    }

    public static void SetArtifact_Max(int max)
    {
        // No-op
    }

    public static void SetMainCharacterName(string name)
    {
        // No-op
    }

    public static void SetMaterial(Bitmap nameplate, Bitmap quantity, string name, int count)
    {
        // No-op
    }

    public static void SetMora(Bitmap bm, int count)
    {
        // No-op
    }

    // Add other methods as discovered from errors
}
