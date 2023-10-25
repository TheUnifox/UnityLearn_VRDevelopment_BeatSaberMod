// Decompiled with JetBrains decompiler
// Type: StringExtensions
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

public static class StringExtensions
{
  public static string Truncate(this string s, int length, bool appendEllipsis = false)
  {
    if (string.IsNullOrEmpty(s) || s.Length <= length)
      return s;
    return !appendEllipsis ? s.Substring(0, length) : s.Substring(0, length - 3) + "...";
  }
}
