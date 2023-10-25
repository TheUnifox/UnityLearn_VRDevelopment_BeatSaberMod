// Decompiled with JetBrains decompiler
// Type: BeatmapLevelPackCollectionSO
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using UnityEngine;

public class BeatmapLevelPackCollectionSO : PersistentScriptableObject, IBeatmapLevelPackCollection
{
  [SerializeField]
  protected BeatmapLevelPackSO[] _beatmapLevelPacks;
  [SerializeField]
  protected PreviewBeatmapLevelPackSO[] _previewBeatmapLevelPack;
  protected IBeatmapLevelPack[] _allBeatmapLevelPacks;

  public PreviewBeatmapLevelPackSO[] previewBeatmapLevelPack
  {
    get => this._previewBeatmapLevelPack;
    set => this._previewBeatmapLevelPack = value;
  }

  public IBeatmapLevelPack[] beatmapLevelPacks
  {
    get
    {
      if (this._allBeatmapLevelPacks == null)
        this.LoadAllBeatmapLevelPacks();
      return this._allBeatmapLevelPacks;
    }
    set => this._allBeatmapLevelPacks = value;
  }

  public virtual void LoadAllBeatmapLevelPacks()
  {
    if (this._allBeatmapLevelPacks != null)
      return;
    List<IBeatmapLevelPack> beatmapLevelPackList = new List<IBeatmapLevelPack>();
    if (this._beatmapLevelPacks != null)
    {
      foreach (BeatmapLevelPackSO beatmapLevelPack in this._beatmapLevelPacks)
        beatmapLevelPackList.Add((IBeatmapLevelPack) beatmapLevelPack);
    }
    if (this._previewBeatmapLevelPack != null)
    {
      foreach (PreviewBeatmapLevelPackSO beatmapLevelPackSo in this._previewBeatmapLevelPack)
        beatmapLevelPackList.Add((IBeatmapLevelPack) beatmapLevelPackSo);
    }
    this._allBeatmapLevelPacks = beatmapLevelPackList.ToArray();
  }
}
