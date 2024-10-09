namespace Bluebird.ViewModels;

public class SettingsViewModel : INotifyPropertyChanged
{
    public static SettingsViewModel SettingsVM = new();
    public ObservableCollection<FavoriteItems> FavoritesList { get; set; } = new ObservableCollection<FavoriteItems>();

    // General
    private bool forcedarkenabled;
    public bool IsForceDarkEnabled
    {
        get => forcedarkenabled;
        set
        {
            if (value != forcedarkenabled)
            {
                forcedarkenabled = value;
                RaisePropertyChanged();
            }
        }
    }

    // Personalization
    private muxc.TabViewWidthMode tabwidthmode;
    public muxc.TabViewWidthMode TabWidthMode
    {
        get => tabwidthmode;
        set
        {
            if (value != tabwidthmode)
            {
                tabwidthmode = value;
                RaisePropertyChanged();
            }
        }
    }

    private bool isreadingmodeenabled;
    public bool IsReadingModeEnabled
    {
        get => isreadingmodeenabled;
        set
        {
            if (value != isreadingmodeenabled)
            {
                isreadingmodeenabled = value;
                RaisePropertyChanged();
            }
        }
    }

    private bool istranslateenabled;
    public bool IsTranslateEnabled
    {
        get => istranslateenabled;
        set
        {
            if (value != istranslateenabled)
            {
                istranslateenabled = value;
                RaisePropertyChanged();
            }
        }
    }

    private bool isqrcodegenenabled;
    public bool IsQRCodeGenEnabled
    {
        get => isqrcodegenenabled;
        set
        {
            if (value != isqrcodegenenabled)
            {
                isqrcodegenenabled = value;
                RaisePropertyChanged();
            }
        }
    }

    private VerticalAlignment urlboxpos;
    public VerticalAlignment UrlboxPos
    {
        get => urlboxpos;
        set
        {
            if (value != urlboxpos)
            {
                urlboxpos = value;
                RaisePropertyChanged();
            }
        }
    }

    private bool isnewtabwallpaperdisabled;
    public bool IsNewTabWallpaperDisabled
    {
        get => isnewtabwallpaperdisabled;
        set
        {
            if (value != isnewtabwallpaperdisabled)
            {
                isnewtabwallpaperdisabled = value;
            }
        }
    }

    public SettingsViewModel()
    {
    }

    private void RaisePropertyChanged([CallerMemberName] string name = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    public event PropertyChangedEventHandler PropertyChanged;
}