// Decompiled with JetBrains decompiler
// Type: VersionStringHelper
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

public abstract class VersionStringHelper
{
  public static int GetMajorVersionNumber(string versionString)
  {
    if (string.IsNullOrEmpty(versionString))
      return 0;
    int result;
    int.TryParse(versionString.Split('.')[0], out result);
    return result;
  }
}
