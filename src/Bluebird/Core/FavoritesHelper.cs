namespace Bluebird.Core;

[JsonSerializable(typeof(ObservableCollection<FavoriteItems>))]
public partial class MyJsonSerializerContext : JsonSerializerContext
{
}

public class FavoriteItems
{
    public string Title { get; set; }
    public string Url { get; set; }
}

public class FavoritesHelper
{

    public static async void LoadFavoritesOnStartup()
    {
        SettingsViewModel.SettingsVM.FavoritesList = await GetFavoritesListAsync();
    }

    public static async void CreateFirstFavorite(string title, string url)
    {
        // Generate json
        string json = "[{\"Title\":\"" + title + "\"," + "\"Url\":\"" + url + "\"}]";
        // create json file
        var file = await localFolder.CreateFileAsync("Favorites.json", CreationCollisionOption.ReplaceExisting);
        // write json to json file
        await FileIO.WriteTextAsync(file, json);
        // new historyitem
        FavoriteItems newFavoriteItem = new()
        {
            Title = title,
            Url = url
        };
        // add item to favorites list
        SettingsViewModel.SettingsVM.FavoritesList.Insert(0, newFavoriteItem);
    }

    public static async void AddFavorite(string title, string url)
    {
        var fileData = await localFolder.TryGetItemAsync("Favorites.json");
        if (fileData == null) CreateFirstFavorite(title, url);
        else
        {
            // new historyitem
            FavoriteItems newFavoriteItem = new()
            {
                Title = title,
                Url = url
            };
            // add item to favorites list
            SettingsViewModel.SettingsVM.FavoritesList.Insert(0, newFavoriteItem);
            SaveListChangesToDisk();
        }
    }

    public static void RemoveFavorite(FavoriteItems item)
    {
        SettingsViewModel.SettingsVM.FavoritesList.Remove(item);
        SaveListChangesToDisk();
    }

    public static async Task<ObservableCollection<FavoriteItems>> GetFavoritesListAsync()
    {
        var fileData = await localFolder.TryGetItemAsync("Favorites.json");
        if (fileData == null)
        {
            ObservableCollection<FavoriteItems> placeholderItems = new();
            return placeholderItems;
        }
        else
        {
            string filecontent = await FileIO.ReadTextAsync((IStorageFile)fileData);
            ObservableCollection<FavoriteItems> Items = JsonSerializer.Deserialize(filecontent, MyJsonSerializerContext.Default.ObservableCollectionFavoriteItems);
            return Items;
        }
    }

    private static async void SaveListChangesToDisk()
    {
        var fileData = await localFolder.GetFileAsync("Favorites.json");
        if (SettingsViewModel.SettingsVM.FavoritesList.Count < 1)
        {
            await fileData.DeleteAsync();
        }
        else
        {
            // Convert list to json
            string newJson = JsonSerializer.Serialize(SettingsViewModel.SettingsVM.FavoritesList, MyJsonSerializerContext.Default.ObservableCollectionFavoriteItems);
            // Write json to json file
            await FileIO.WriteTextAsync((IStorageFile)fileData, newJson);
        }
    }
}
