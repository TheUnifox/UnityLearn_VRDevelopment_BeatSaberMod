// Decompiled with JetBrains decompiler
// Type: CustomBeatmapLevelPack
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Runtime.CompilerServices;
using UnityEngine;

public class CustomBeatmapLevelPack : IBeatmapLevelPack, IAnnotatedBeatmapLevelCollection
{
  [CompilerGenerated]
  protected readonly string m_CpackID;
  [CompilerGenerated]
  protected readonly string m_CpackName;
  [CompilerGenerated]
  protected readonly string m_CshortPackName;
  [CompilerGenerated]
  protected readonly Sprite m_CcoverImage;
  [CompilerGenerated]
  protected readonly Sprite m_CsmallCoverImage;
  [CompilerGenerated]
  protected readonly IBeatmapLevelCollection m_CbeatmapLevelCollection;

  public string packID => this.m_CpackID;

  public string packName => this.m_CpackName;

  public string shortPackName => this.m_CshortPackName;

  public string collectionName => this.shortPackName;

  public Sprite coverImage => this.m_CcoverImage;

  public Sprite smallCoverImage => this.m_CsmallCoverImage;

  public IBeatmapLevelCollection beatmapLevelCollection => this.m_CbeatmapLevelCollection;

  public bool isPackAlwaysOwned => true;

  public CustomBeatmapLevelPack(
    string packID,
    string packName,
    string shortPackName,
    Sprite coverImage,
    Sprite smallCoverImage,
    CustomBeatmapLevelCollection beatmapLevelCollection)
  {
    this.m_CpackID = packID;
    this.m_CpackName = packName;
    this.m_CshortPackName = shortPackName;
    this.m_CcoverImage = coverImage;
    this.m_CsmallCoverImage = smallCoverImage;
    this.m_CbeatmapLevelCollection = (IBeatmapLevelCollection) beatmapLevelCollection;
  }
}
