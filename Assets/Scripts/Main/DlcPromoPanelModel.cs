// Decompiled with JetBrains decompiler
// Type: DlcPromoPanelModel
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Zenject;

public class DlcPromoPanelModel
{
  [Inject]
  protected readonly AdditionalContentModel _additionalContentModel;
  [Inject]
  protected readonly IAnalyticsModel _analyticsModel;
  [Inject]
  protected readonly DlcPromoPanelDataSO _dlcPromoPanelData;
  [Inject]
  protected readonly PlayerDataModel _playerDataModel;
  protected DlcPromoPanelDataSO.MusicPackPromoInfo[] _notOwnedMusicPackPromoInfos;
  protected bool _updatingNotOwnedPacks;
  protected bool _initialized;
  protected Random _random;

  public virtual void InitAfterPlatformWasInitialized(bool force)
  {
    if (!force && this._initialized)
    {
      this.UpdateNotOwnedPacksAsync();
    }
    else
    {
      this._initialized = true;
      this._additionalContentModel.didInvalidateDataEvent -= new System.Action(this.HandleAdditionalContentModelDidInvalidateData);
      this._additionalContentModel.didInvalidateDataEvent += new System.Action(this.HandleAdditionalContentModelDidInvalidateData);
      this.UpdateNotOwnedPacksAsync();
    }
  }

  public virtual DlcPromoPanelDataSO.MusicPackPromoInfo GetPackDataForMainMenuPromoBanner(
    out bool owned)
  {
    if (this._random == null)
      this._random = new Random(DateTime.Now.Millisecond);
    string packId = this._dlcPromoPanelData.defaultMusicPackPromo.previewBeatmapLevelPack.packID;
    if (this._playerDataModel.playerData.currentDlcPromoId != packId)
      this._playerDataModel.playerData.SetNewDlcPromo(packId);
    DlcPromoPanelDataSO.MusicPackPromoInfo mainMenuPromoBanner = this._playerDataModel.playerData.currentDlcPromoDisplayCount >= this._dlcPromoPanelData.cutOffTest ? (this._notOwnedMusicPackPromoInfos == null || this._notOwnedMusicPackPromoInfos.Length < this._dlcPromoPanelData.minNumberOfNotOwnedPacks ? this._dlcPromoPanelData.musicPackPromoInfos[this._random.Next(0, this._dlcPromoPanelData.musicPackPromoInfos.Length)] : this._notOwnedMusicPackPromoInfos[this._random.Next(0, this._notOwnedMusicPackPromoInfos.Length)]) : this._dlcPromoPanelData.defaultMusicPackPromo;
    owned = this._notOwnedMusicPackPromoInfos == null || !((IEnumerable<DlcPromoPanelDataSO.MusicPackPromoInfo>) this._notOwnedMusicPackPromoInfos).Contains<DlcPromoPanelDataSO.MusicPackPromoInfo>(mainMenuPromoBanner);
    return mainMenuPromoBanner;
  }

  public virtual void MainMenuDlcPromoBannerWasShown(IBeatmapLevelPack promoPack, string customText)
  {
    this._analyticsModel.LogImpression("DLC Promo Banner", this.GetExperimentEventData(promoPack.packID, "Main Menu", customText));
    this._playerDataModel.playerData.IncreaseCurrentDlcPromoDisplayCount();
  }

  public virtual void MainMenuDlcPromoBannerWasPressed(
    IBeatmapLevelPack promoPack,
    string customText)
  {
    this._analyticsModel.LogClick("DLC Promo Banner", this.GetExperimentEventData(promoPack.packID, "Main Menu", customText));
  }

  public virtual void BuyLevelButtonWasPressed(
    IPreviewBeatmapLevel level,
    string page,
    string customText)
  {
    this._analyticsModel.LogClick("Buy Level Button", this.GetExperimentEventData(level.levelID, page, customText));
  }

  public virtual void BuyLevelButtonWasShown(
    IPreviewBeatmapLevel level,
    string page,
    string customText)
  {
    this._analyticsModel.LogImpression("Buy Level Button", this.GetExperimentEventData(level.levelID, page, customText));
  }

  public virtual void BuyPackButtonWasPressed(
    IBeatmapLevelPack pack,
    string page,
    string customText)
  {
    this._analyticsModel.LogClick("Buy Pack Button", this.GetExperimentEventData(pack.packID, page, customText));
  }

  public virtual void BuyPackButtonWasShown(IBeatmapLevelPack pack, string page, string customText) => this._analyticsModel.LogImpression("Buy Pack Button", this.GetExperimentEventData(pack.packID, page, customText));

  public virtual Dictionary<string, string> GetExperimentEventData(
    string itemId,
    string page,
    string customText)
  {
    return new Dictionary<string, string>()
    {
      ["item_id"] = itemId,
      [nameof (page)] = page,
      ["custom_text"] = customText
    };
  }

  public virtual async void UpdateNotOwnedPacksAsync()
  {
    if (this._updatingNotOwnedPacks)
      return;
    this._updatingNotOwnedPacks = true;
    List<DlcPromoPanelDataSO.MusicPackPromoInfo> newNotOwnedMusicPackPromoInfos = new List<DlcPromoPanelDataSO.MusicPackPromoInfo>();
    DlcPromoPanelDataSO.MusicPackPromoInfo[] musicPackPromoInfoArray = this._dlcPromoPanelData.musicPackPromoInfos;
    for (int index = 0; index < musicPackPromoInfoArray.Length; ++index)
    {
      DlcPromoPanelDataSO.MusicPackPromoInfo packData = musicPackPromoInfoArray[index];
      CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
      if (await this._additionalContentModel.GetPackEntitlementStatusAsync(packData.previewBeatmapLevelPack.packID, cancellationTokenSource.Token) == AdditionalContentModel.EntitlementStatus.NotOwned)
        newNotOwnedMusicPackPromoInfos.Add(packData);
      packData = (DlcPromoPanelDataSO.MusicPackPromoInfo) null;
    }
    musicPackPromoInfoArray = (DlcPromoPanelDataSO.MusicPackPromoInfo[]) null;
    this._notOwnedMusicPackPromoInfos = newNotOwnedMusicPackPromoInfos.ToArray();
    this._updatingNotOwnedPacks = false;
  }

  public virtual void HandleAdditionalContentModelDidInvalidateData() => this.UpdateNotOwnedPacksAsync();
}
