// Decompiled with JetBrains decompiler
// Type: DlcPromoPanelDataSO
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using UnityEngine;

public class DlcPromoPanelDataSO : ScriptableObject
{
  [SerializeField]
  protected int _defaultMusicPackPromoIndex;
  [SerializeField]
  protected DlcPromoPanelDataSO.MusicPackPromoInfo[] _musicPackPromoInfos;
  [SerializeField]
  protected int _cutOffTest = 2;
  [SerializeField]
  protected int _minNumberOfNotOwnedPacks = 2;

  public int cutOffTest => this._cutOffTest;

  public int minNumberOfNotOwnedPacks => this._minNumberOfNotOwnedPacks;

  public DlcPromoPanelDataSO.MusicPackPromoInfo[] musicPackPromoInfos => this._musicPackPromoInfos;

  public DlcPromoPanelDataSO.MusicPackPromoInfo defaultMusicPackPromo => this.musicPackPromoInfos[this._defaultMusicPackPromoIndex];

  [Serializable]
  public class MusicPackPromoInfo
  {
    [SerializeField]
    protected PreviewBeatmapLevelPackSO _beatmapLevelPack;
    [SerializeField]
    [NullAllowed]
    protected BeatmapLevelSO _beatmapLevel;
    [SerializeField]
    protected Sprite _bannerImage;
    [SerializeField]
    protected string _bannerPromoText;

    public IBeatmapLevelPack previewBeatmapLevelPack => (IBeatmapLevelPack) this._beatmapLevelPack;

    public IPreviewBeatmapLevel previewBeatmapLevel => (IPreviewBeatmapLevel) this._beatmapLevel;

    public Sprite bannerImage => this._bannerImage;

    public string bannerPromoText => this._bannerPromoText;
  }
}
