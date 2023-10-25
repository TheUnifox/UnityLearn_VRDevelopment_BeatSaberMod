// Decompiled with JetBrains decompiler
// Type: BeatmapLevelPack
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class BeatmapLevelPack : IBeatmapLevelPack, IAnnotatedBeatmapLevelCollection
{
  protected string _levelPackID;
  protected string _packName;
  protected string _shortPackName;
  protected Sprite _coverImage;
  protected Sprite _smallCoverImage;
  protected IBeatmapLevelCollection _beatmapLevelCollection;

  public string packID => this._levelPackID;

  public string packName => this._packName;

  public string shortPackName => this._shortPackName;

  public string collectionName => this.shortPackName;

  public Sprite coverImage => this._coverImage;

  public Sprite smallCoverImage => this._smallCoverImage;

  public IBeatmapLevelCollection beatmapLevelCollection => this._beatmapLevelCollection;

  public BeatmapLevelPack(
    string levelPackID,
    string packName,
    string shortPackName,
    Sprite coverImage,
    Sprite smallCoverImage,
    IBeatmapLevelCollection levelCollection)
  {
    this._levelPackID = levelPackID;
    this._packName = packName;
    this._shortPackName = shortPackName;
    this._coverImage = coverImage;
    this._smallCoverImage = smallCoverImage;
    this._beatmapLevelCollection = levelCollection;
  }

  public static BeatmapLevelPack CreateBeatmapLevelPackByUsingBeatmapCharacteristicFiltering(
    IBeatmapLevelPack beatmapLevelPack,
    BeatmapCharacteristicSO beatmapCharacteristic)
  {
    return new BeatmapLevelPack(beatmapLevelPack.packID, beatmapLevelPack.packName, beatmapLevelPack.shortPackName, beatmapLevelPack.coverImage, beatmapLevelPack.smallCoverImage, (IBeatmapLevelCollection) BeatmapLevelCollection.CreateBeatmapLevelCollectionByUsingBeatmapCharacteristicFiltering(beatmapLevelPack.beatmapLevelCollection, beatmapCharacteristic));
  }
}
