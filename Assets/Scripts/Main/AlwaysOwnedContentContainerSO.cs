// Decompiled with JetBrains decompiler
// Type: AlwaysOwnedContentContainerSO
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using UnityEngine;

public class AlwaysOwnedContentContainerSO : PersistentScriptableObject
{
  [SerializeField]
  protected AlwaysOwnedContentSO _alwaysOwnedContent;
  protected HashSet<string> _alwaysOwnedBeatmapLevelIds;
  protected HashSet<string> _alwaysOwnedPacksIds;

  public HashSet<string> alwaysOwnedBeatmapLevelIds
  {
    get
    {
      if (this._alwaysOwnedBeatmapLevelIds == null)
        this.InitAlwaysOwnedItems();
      return this._alwaysOwnedBeatmapLevelIds;
    }
  }

  public HashSet<string> alwaysOwnedPacksIds
  {
    get
    {
      if (this._alwaysOwnedPacksIds == null)
        this.InitAlwaysOwnedItems();
      return this._alwaysOwnedPacksIds;
    }
  }

  protected override void OnEnable()
  {
    base.OnEnable();
    this.InitAlwaysOwnedItems();
  }

  public virtual void InitAlwaysOwnedItems()
  {
    this._alwaysOwnedBeatmapLevelIds = new HashSet<string>();
    this._alwaysOwnedPacksIds = new HashSet<string>();
    foreach (BeatmapLevelPackSO alwaysOwnedPack in this._alwaysOwnedContent.alwaysOwnedPacks)
    {
      this._alwaysOwnedPacksIds.Add(alwaysOwnedPack.packID);
      foreach (IPreviewBeatmapLevel beatmapLevel in (IEnumerable<IPreviewBeatmapLevel>) alwaysOwnedPack.beatmapLevelCollection.beatmapLevels)
        this._alwaysOwnedBeatmapLevelIds.Add(beatmapLevel.levelID);
    }
    foreach (BeatmapLevelSO ownedBeatmapLevel in this._alwaysOwnedContent.alwaysOwnedBeatmapLevels)
      this._alwaysOwnedBeatmapLevelIds.Add(ownedBeatmapLevel.levelID);
  }
}
