<p align="center">
  <img src="images/HeroImage.jpg" alt="Bluebird screenshot" />
</p>

<p align="center">
  <a href="https://www.microsoft.com/store/productId/9PNXW61T4T0V" target="_blank">
    <img src="images/msstorebadge.png" /></a>
  <a href="https://bluebird-developers.github.io/releases/bluebird.appinstaller" target="_blank">
    <img src="images/ghreleasesbadge.png" /></a>
</p>

Bluebird is a modern webbrowser which enhances your browsing experience. It has been in active development for over two years, and is being constantly updated with bug fixes, performance improvements and new features.

## Headline features:
- Responsive UI and quick startup time
- Integrates nicely with other native Windows apps 
- Split tabs
- Reading mode
- Translate websites
- Password lock
- Generate QRCode for website
- Export page to pdf (version 5.1 and newer)
- Export page text content as txt (version 7.5 and newer)
- New tab background images
- Focus mode

Check the app out to see them in action! 

## Installing

You can get Bluebird from the main sources:

### Microsoft Store (recommended)

This is the recommended source for most users, you can get Bluebird easily [here](https://www.microsoft.com/store/productId/9PNXW61T4T0V)

### Winget

If you prefer installing your apps from the command line, you can do so via winget

```batch
winget install Bluebird
```

### GitHub releases (advanced)

> [!WARNING]
> You might not recive updates if you install Bluebird this way, thus this method is only recommended for advanced users

If you like to get the msix for yourself and install without winget or Microsoft Store, you can get the package [here](https://github.com/bluebird-developers/browser/releases)

You have to get the dependencies for your architecture (either X86, X64 or ARM64) as well as the main package. Then you can install them via PowerShell or App installer

```powershell
Add-AppxPackage ".\Microsoft.UI.Xaml.2.8_8.2501.31001.0_arm64__8wekyb3d8bbwe.Appx"
Add-AppxPackage ".\Microsoft.VCLibs.140.00.UWPDesktop_14.0.33728.0_arm64__8wekyb3d8bbwe.Appx"
Add-AppxPackage ".\Bluebird-8.2.0.0.msixbundle"
```
Be sure to replace them with the names of your packages according to your system and the latest versions
## Contributing

### Contributing source code
You would like to contribute to this project?

Awesome! Please first create an issue, so we can talk about the changes you like to implement

### Support the project by donating
You would like to support this project because you love it, but do not have the time or expertise to contribute source code?

A donation would be highly appreciated, and it helps me out a lot. Thanks :)

[PayPal](https://www.paypal.com/paypalme/julianhasreiter)

## Building from source

### 1. Prerequisites
- Visual Studio 2022, version 17.13 or newer
- Windows 11 SDK (10.0.26100.0)
- .NET 9 SDK
- Windows Application Workload + Universal Windows Platform Tools

### 2. Clone the repository
```batch
git clone https://github.com/bluebird-developers/browser.git
```

### 3. Build the app
In the newly cloned folder, navigate into the src folder and open Bluebird.sln (or Bluebird.slnx if you prefer that)
On the top, select your configuration and platform and click on the play icon.
Now VS should start building the app and will start it shortly.

## License
GPL v3.0
