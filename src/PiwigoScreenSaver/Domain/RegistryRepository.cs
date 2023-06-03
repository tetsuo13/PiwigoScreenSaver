using Microsoft.Win32;
using System;
using System.Security.Cryptography;
using System.Text;

namespace PiwigoScreenSaver.Domain;

/// <summary>
/// Values stored in registry are encrypted using Windows Data Protection
/// (DPAPI) unique to the current user in attempt to mitigate leaking
/// login credentials.
/// </summary>
public class RegistryRepository : ISettingsRepository
{
    private const string HkcuPath = @"Software\PiwigoScreenSaver";

    public string GetValue(string name)
    {
        using var key = Registry.CurrentUser.OpenSubKey(HkcuPath);

        if (key?.GetValue(name) is string value)
        {
            return Decrypt(value);
        }

        return null;
    }

    public void SetValue(string name, string value)
    {
        var encryptedValue = Encrypt(value);
        using var key = Registry.CurrentUser.OpenSubKey(HkcuPath, true);

        if (key != null)
        {
            key.SetValue(name, encryptedValue);
        }
        else
        {
            using var newKey = Registry.CurrentUser.CreateSubKey(HkcuPath);
            newKey.SetValue(name, encryptedValue, RegistryValueKind.String);
        }
    }

    private static string Encrypt(string value) =>
        Convert.ToBase64String(ProtectedData.Protect(Encoding.Unicode.GetBytes(value),
            null, DataProtectionScope.CurrentUser));

    private static string Decrypt(string value) =>
        Encoding.Unicode.GetString(ProtectedData.Unprotect(Convert.FromBase64String(value),
            null, DataProtectionScope.CurrentUser));
}
