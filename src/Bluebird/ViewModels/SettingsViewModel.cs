using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Bluebird.ViewModels;

public class SettingsViewModel : INotifyPropertyChanged
{
    public static SettingsViewModel SettingsVM = new();
    // General
    private bool forcedarkenabled;
    public bool IsForceDarkEnabled
    {
        get => forcedarkenabled;
        set
        {
            if (value !=  forcedarkenabled)
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

    public SettingsViewModel()
    {
    }

    private void RaisePropertyChanged([CallerMemberName] string name = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    public event PropertyChangedEventHandler PropertyChanged;
}