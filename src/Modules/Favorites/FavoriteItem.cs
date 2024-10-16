namespace Bluebird.Modules.Favorites;

[JsonSerializable(typeof(ObservableCollection<FavoriteItem>))]
public partial class MyJsonSerializerContext : JsonSerializerContext
{
}

public class FavoriteItem
{
    public string Title { get; set; }
    public string Url { get; set; }
}
