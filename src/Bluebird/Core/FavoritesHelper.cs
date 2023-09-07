namespace Bluebird.Core;

public class FavoritesHelper
{
    public static async void CreateFirstFavorite(string title, string url)
    {
        // Generate json
        string json = "[{\"title\":\"" + title + "\"," + "\"url\":\"" + url + "\"}]";
        // create json file
        var file = await localFolder.CreateFileAsync("Favorites.json", CreationCollisionOption.ReplaceExisting);
        // write json to json file
        await FileIO.WriteTextAsync(file, json);
    }

    public static async void AddFavorite(string title, string url)
    {
        var fileData = await localFolder.TryGetItemAsync("Favorites.json");
        if (fileData == null) CreateFirstFavorite(title, url);
        else
        {
            // get json file content
            string json = await FileIO.ReadTextAsync((IStorageFile)fileData);
            // new historyitem
            FavoriteItems newFavoriteItem = new()
            {
                Title = title,
                Url = url
            };
            // Convert json to list
            List<FavoriteItems> FavoriteList = JsonConvert.DeserializeObject<List<FavoriteItems>>(json);
            // Add new historyitem
            FavoriteList.Insert(0, newFavoriteItem);
            // Convert list to json
            string newJson = JsonConvert.SerializeObject(FavoriteList);
            // Write json to json file
            await FileIO.WriteTextAsync((IStorageFile)fileData, newJson);
        }
    }

    public static async Task<List<FavoriteItems>> GetFavoritesListAsync()
    {
        var fileData = await localFolder.TryGetItemAsync("Favorites.json");
        if (fileData == null) return null;
        else
        {
            string filecontent = await FileIO.ReadTextAsync((IStorageFile)fileData);
            return JsonConvert.DeserializeObject<List<FavoriteItems>>(filecontent);
        }
    }
}
