using System;

// Token: 0x02000039 RID: 57
public abstract class BeatmapSaveDataHelpers
{
    // Token: 0x06000124 RID: 292 RVA: 0x00004B44 File Offset: 0x00002D44
    public static Version GetVersion(string data)
    {
        string text = data.Substring(0, 50);
        int num = text.IndexOf("\"_version\":\"", StringComparison.Ordinal);
        int num2;
        if (num == -1)
        {
            num2 = text.IndexOf("\"version\":\"", StringComparison.Ordinal);
            num2 += "\"version\":\"".Length;
        }
        else
        {
            num2 = num + "\"_version\":\"".Length;
        }
        int num3 = text.IndexOf("\"", num2, StringComparison.Ordinal);
        return new Version(text.Substring(num2, num3 - num2));
    }

    // Token: 0x040000FE RID: 254
    [DoesNotRequireDomainReloadInit]
    private static readonly Version version2_6_0 = new Version("2.6.0");

    // Token: 0x040000FF RID: 255
    private const string kLegacyVersionSearchString = "\"_version\":\"";

    // Token: 0x04000100 RID: 256
    private const string kVersionSearchString = "\"version\":\"";
}
