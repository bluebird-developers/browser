using System.Collections;
using System.Collections.Generic;
using System;

namespace Bluebird.Modules.QRCodeGen;

public class QRCodeData : IDisposable
{
    public List<BitArray> ModuleMatrix { get; set; }

    public QRCodeData(int version)
    {
        this.Version = version;
        var size = ModulesPerSideFromVersion(version);
        this.ModuleMatrix = new List<BitArray>();
        for (var i = 0; i < size; i++)
            this.ModuleMatrix.Add(new BitArray(size));
    }

#if NETFRAMEWORK || NETSTANDARD2_0 || NET5_0
    public void SaveRawData(string filePath, Compression compressMode)
    {
        File.WriteAllBytes(filePath, GetRawData(compressMode));
    }
#endif

    public int Version { get; private set; }

    private static int ModulesPerSideFromVersion(int version)
    {
        return 21 + (version - 1) * 4;
    }

    public void Dispose()
    {
        this.ModuleMatrix = null;
        this.Version = 0;

    }
}
