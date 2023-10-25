// Decompiled with JetBrains decompiler
// Type: BeatmapLevelsPromoDataSO
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BeatmapLevelsPromoDataSO : ScriptableObject
{
  [Space]
  [SerializeField]
  protected List<PreviewBeatmapLevelPackSO> _promotedBeatmapLevelPacks;
  [SerializeField]
  protected List<PreviewBeatmapLevelPackSO> _updatedBeatmapLevelPacks;
  [Space]
  [SerializeField]
  protected List<PreviewBeatmapLevelSO> _promotedBeatmapLevels;
  [SerializeField]
  protected List<PreviewBeatmapLevelSO> _updatedBeatmapLevels;
  protected HashSet<string> _promotedBeatmapLevelPacksSet;
  protected HashSet<string> _updatedBeatmapLevelPacksSet;
  protected HashSet<string> _promotedBeatmapLevelsSet;
  protected HashSet<string> _updatedBeatmapLevelsSet;

  public virtual bool IsBeatmapLevelPackPromoted(IBeatmapLevelPack beatmapLevelPack)
  {
    if (this._promotedBeatmapLevelPacksSet == null)
      this._promotedBeatmapLevelPacksSet = new HashSet<string>(this._promotedBeatmapLevelPacks.Select<PreviewBeatmapLevelPackSO, string>((Func<PreviewBeatmapLevelPackSO, string>) (pack => pack.packID)));
    return this._promotedBeatmapLevelPacksSet.Contains(beatmapLevelPack.packID);
  }

  public virtual bool IsBeatmapLevelPackUpdated(IBeatmapLevelPack beatmapLevelPack)
  {
    if (this._updatedBeatmapLevelPacksSet == null)
      this._updatedBeatmapLevelPacksSet = new HashSet<string>(this._updatedBeatmapLevelPacks.Select<PreviewBeatmapLevelPackSO, string>((Func<PreviewBeatmapLevelPackSO, string>) (pack => pack.packID)));
    return this._updatedBeatmapLevelPacksSet.Contains(beatmapLevelPack.packID);
  }

  public virtual bool IsBeatmapLevelPromoted(IPreviewBeatmapLevel beatmapLevel)
  {
    if (this._promotedBeatmapLevelsSet == null)
      this._promotedBeatmapLevelsSet = new HashSet<string>(this._promotedBeatmapLevels.Select<PreviewBeatmapLevelSO, string>((Func<PreviewBeatmapLevelSO, string>) (level => level.levelID)));
    return this._promotedBeatmapLevelsSet.Contains(beatmapLevel.levelID);
  }

  public virtual bool IsBeatmapLevelUpdated(IPreviewBeatmapLevel beatmapLevel)
  {
    if (this._updatedBeatmapLevelsSet == null)
      this._updatedBeatmapLevelsSet = new HashSet<string>(this._updatedBeatmapLevels.Select<PreviewBeatmapLevelSO, string>((Func<PreviewBeatmapLevelSO, string>) (level => level.levelID)));
    return this._updatedBeatmapLevelsSet.Contains(beatmapLevel.levelID);
  }
}
