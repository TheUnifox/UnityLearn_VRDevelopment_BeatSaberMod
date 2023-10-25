// Decompiled with JetBrains decompiler
// Type: SFB.ExtensionFilter
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

namespace SFB
{
  public struct ExtensionFilter
  {
    public readonly string _name;
    public readonly string[] _extensions;

    public ExtensionFilter(string filterName, params string[] filterExtensions)
    {
      this._name = filterName;
      this._extensions = filterExtensions;
    }
  }
}
