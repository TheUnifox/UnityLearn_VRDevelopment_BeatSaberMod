// Decompiled with JetBrains decompiler
// Type: BeatmapLevelCollectionSO
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using UnityEngine;

public class BeatmapLevelCollectionSO : PersistentScriptableObject, IBeatmapLevelCollection
{
  [SerializeField]
  protected BeatmapLevelSO[] _beatmapLevels;

  public IReadOnlyList<IPreviewBeatmapLevel> beatmapLevels => (IReadOnlyList<IPreviewBeatmapLevel>) this._beatmapLevels;
}
