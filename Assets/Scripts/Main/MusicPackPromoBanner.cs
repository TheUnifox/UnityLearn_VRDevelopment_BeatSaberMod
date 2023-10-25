// Decompiled with JetBrains decompiler
// Type: MusicPackPromoBanner
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Polyglot;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MusicPackPromoBanner : MonoBehaviour
{
  [LocalizationKey]
  protected const string kPlayNow = "PROMO_BANNER_PLAY_NOW_LABEL";
  [LocalizationKey]
  protected const string kGetNow = "PROMO_GET_NOW_LABEL";
  [SerializeField]
  protected GameObject _promoBannerGo;
  [SerializeField]
  protected TextMeshProUGUI _promoText;
  [SerializeField]
  protected Image _backgroundImage;
  protected IBeatmapLevelPack _currentPromoMusicPack;
  protected IPreviewBeatmapLevel _currentPromoBeatmapLevel;
  protected string _text;

  public IPreviewBeatmapLevel currentPromoBeatmapLevel => this._currentPromoBeatmapLevel;

  public IBeatmapLevelPack currentPromoMusicPack => this._currentPromoMusicPack;

  public string promoButtonText => this._text;

  public virtual void Setup(
    DlcPromoPanelDataSO.MusicPackPromoInfo musicPackPromoData,
    bool probablyOwned)
  {
    this._currentPromoMusicPack = musicPackPromoData.previewBeatmapLevelPack;
    this._currentPromoBeatmapLevel = musicPackPromoData.previewBeatmapLevel;
    this._promoBannerGo.SetActive(!string.IsNullOrEmpty(musicPackPromoData.bannerPromoText));
    this._promoText.text = musicPackPromoData.bannerPromoText;
    this._backgroundImage.sprite = musicPackPromoData.bannerImage;
    this._text = probablyOwned ? Localization.Get("PROMO_BANNER_PLAY_NOW_LABEL") : Localization.Get("PROMO_GET_NOW_LABEL");
  }
}
