// Decompiled with JetBrains decompiler
// Type: BeatmapLevelPackSO
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class BeatmapLevelPackSO : 
  PersistentScriptableObject,
  IBeatmapLevelPack,
  IAnnotatedBeatmapLevelCollection
{
  [SerializeField]
  protected string _packID;
  [SerializeField]
  protected string _packName;
  [SerializeField]
  protected string _shortPackName;
  [SerializeField]
  protected Sprite _coverImage;
  [SerializeField]
  protected Sprite _smallCoverImage;
  [Space]
  [SerializeField]
  protected BeatmapLevelCollectionSO _beatmapLevelCollection;

  public string packID => this._packID;

  public string packName => this._packName;

  public string shortPackName => this._shortPackName;

  public string collectionName => this.shortPackName;

  public Sprite coverImage => this._coverImage;

  public Sprite smallCoverImage => this._smallCoverImage;

  public IBeatmapLevelCollection beatmapLevelCollection => (IBeatmapLevelCollection) this._beatmapLevelCollection;
}
