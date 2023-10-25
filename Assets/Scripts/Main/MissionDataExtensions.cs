// Decompiled with JetBrains decompiler
// Type: MissionDataExtensions
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Polyglot;

public static class MissionDataExtensions
{
  public static string Name(
    this MissionObjective.ReferenceValueComparisonType comparisonType)
  {
    if (comparisonType == MissionObjective.ReferenceValueComparisonType.Max)
      return Localization.Get("MAX_MISSION_OBJECTIVE_TYPE");
    return comparisonType == MissionObjective.ReferenceValueComparisonType.Min ? Localization.Get("MIN_MISSION_OBJECTIVE_TYPE") : "";
  }
}
