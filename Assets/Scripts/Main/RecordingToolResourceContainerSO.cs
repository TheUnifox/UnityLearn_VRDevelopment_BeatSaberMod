// Decompiled with JetBrains decompiler
// Type: RecordingToolResourceContainerSO
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using UnityEngine;

public class RecordingToolResourceContainerSO : PersistentScriptableObject
{
  [SerializeField]
  protected BeatmapLevelPackCollectionSO _dlcLevelPackCollection;
  [SerializeField]
  protected BeatmapLevelPackCollectionSO _ostAndExtrasPackCollection;
  [SerializeField]
  protected BeatmapCharacteristicCollectionSO _beatmapCharacteristicCollection;
  [SerializeField]
  protected EnvironmentsListSO _environmentsList;
  protected List<IBeatmapLevelPack> _beatmapLevelPacks;

  public List<IBeatmapLevelPack> beatmapLevelPacks => this._beatmapLevelPacks;

  public BeatmapCharacteristicCollectionSO beatmapCharacteristicCollection => this._beatmapCharacteristicCollection;

  public EnvironmentsListSO environmentsList => this._environmentsList;

  protected override void OnEnable()
  {
    base.OnEnable();
    this._beatmapLevelPacks = new List<IBeatmapLevelPack>((IEnumerable<IBeatmapLevelPack>) this._ostAndExtrasPackCollection.beatmapLevelPacks);
    this._beatmapLevelPacks.AddRange((IEnumerable<IBeatmapLevelPack>) this._dlcLevelPackCollection.beatmapLevelPacks);
  }
}
