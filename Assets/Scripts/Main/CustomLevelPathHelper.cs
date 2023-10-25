// Decompiled with JetBrains decompiler
// Type: CustomLevelPathHelper
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.IO;
using UnityEngine;

public class CustomLevelPathHelper
{
  public const string kStandardLevelInfoFilename = "Info.dat";
  public const string kCustomLevelsDirectoryName = "CustomLevels";
  [DoesNotRequireDomainReloadInit]
  public static readonly string customLevelsDirectoryPath;
  [DoesNotRequireDomainReloadInit]
  public static readonly string baseProjectPath = Application.dataPath;

  static CustomLevelPathHelper() => CustomLevelPathHelper.customLevelsDirectoryPath = Path.Combine(CustomLevelPathHelper.baseProjectPath, "CustomLevels");

  public static string GetDefaultNameForCustomLevel(
    string songName,
    string songAuthorName,
    string levelAuthorName)
  {
    return songAuthorName + " - " + songName + " (" + levelAuthorName + ")";
  }
}
