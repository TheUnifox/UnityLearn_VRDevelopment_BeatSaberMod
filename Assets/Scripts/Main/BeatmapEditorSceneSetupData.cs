// Decompiled with JetBrains decompiler
// Type: BeatmapEditorSceneSetupData
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using UnityEngine;

[Serializable]
public class BeatmapEditorSceneSetupData : SceneSetupData
{
  [SerializeField]
  protected string _levelDirPath;
  [SerializeField]
  protected string _levelAssetPath;

  public string levelDirPath => this._levelDirPath;

  public string levelAssetPath => this._levelAssetPath;

  public BeatmapEditorSceneSetupData(string levelDirPath, string levelAssetPath)
  {
    this._levelDirPath = levelDirPath;
    this._levelAssetPath = levelAssetPath;
  }
}
