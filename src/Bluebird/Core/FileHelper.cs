using System.IO;
using Windows.Storage.Pickers;
using Windows.Storage.Provider;

namespace Bluebird.Core;

public class FileHelper
{
    public static async Task SaveBytesAsFileAsync(string fileName, byte[] buffer)
    {
        // Create a file picker
        FileSavePicker savePicker = new Windows.Storage.Pickers.FileSavePicker();

        // Set options for your file picker
        savePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
        // Dropdown of file types the user can save the file as
        savePicker.FileTypeChoices.Add("PDF", new List<string>() { ".pdf" });
        // Default file name if the user does not type one in or select a file to replace
        savePicker.SuggestedFileName = fileName;

        // Open the picker for the user to pick a file
        StorageFile file = await savePicker.PickSaveFileAsync();
        if (file != null)
        {
            // Prevent updates to the remote version of the file until we finish making changes and call CompleteUpdatesAsync.
            CachedFileManager.DeferUpdates(file);

            // write to file
            // Another way to write a string to the file is to use this instead:
            await FileIO.WriteBytesAsync(file, buffer);

            // Let Windows know that we're finished changing the file so the other app can update the remote version of the file.
            // Completing updates may require Windows to ask for user input.
            FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(file);
            if (status == FileUpdateStatus.Complete)
            {
                //SaveFileOutputTextBlock.Text = "File " + file.Name + " was saved.";
            }
            else if (status == FileUpdateStatus.CompleteAndRenamed)
            {
                //SaveFileOutputTextBlock.Text = "File " + file.Name + " was renamed and saved.";
            }
            else
            {
                //SaveFileOutputTextBlock.Text = "File " + file.Name + " couldn't be saved.";
            }
        }
        else
        {
            //SaveFileOutputTextBlock.Text = "Operation cancelled.";
        }

    }
    public static async Task DeleteLocalFile(string fileName)
    {
        var file = await ApplicationData.Current.LocalFolder.TryGetItemAsync(fileName);
        if (file != null)
            await file.DeleteAsync();
    }
}