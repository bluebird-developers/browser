﻿namespace Bluebird.Core;

public class AppVersionHelper
{
    public static string GetAppVersion()
    {
        Package package = Package.Current;
        PackageId packageId = package.Id;
        PackageVersion version = packageId.Version;

        return string.Format("{0}.{1}.{2}", version.Major, version.Minor, version.Build);
    }
}
