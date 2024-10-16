using Windows.Security.Credentials;

namespace Bluebird.Core;

public static class PasswordHelper
{
    private const string RESOURCE_NAME = "BluebirdAppLock";
    public static string GetCredential()
    {
        PasswordVault vault = new();
        try
        {
            var credential = vault.FindAllByResource(RESOURCE_NAME).FirstOrDefault();
            if (credential != null)
            {
                // Retrieves the actual userName and password.
                string userName = credential.UserName;
                string password = vault.Retrieve(RESOURCE_NAME, userName).Password;
                return password;
            }
            else
                return string.Empty;
            
        }
        catch (Exception)
        {
            // If no credentials have been stored with the given RESOURCE_NAME, an exception
            // is thrown.
            return string.Empty;
        }
    }

    public static void SaveCredential(string userName, string password)
    {
        PasswordVault vault = new();
        PasswordCredential credential = new(RESOURCE_NAME, userName, password);

        // Permanently stores credential in the password vault.
        vault.Add(credential);
    }

    public static void RemoveCredential(string userName)
    {
        PasswordVault vault = new();
        try
        {
            // Removes the credential from the password vault.
            vault.Remove(vault.Retrieve(RESOURCE_NAME, userName));
        }
        catch (Exception)
        {
            // If no credentials have been stored with the given RESOURCE_NAME, an exception
            // is thrown.
        }
    }
}
