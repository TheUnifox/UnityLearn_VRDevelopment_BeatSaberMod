// Decompiled with JetBrains decompiler
// Type: LevelPackDetailViewController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class LevelPackDetailViewController : ViewController
{
  [SerializeField]
  protected GameObject _detailWrapper;
  [SerializeField]
  protected ImageView _packImage;
  [SerializeField]
  protected Button _buyButton;
  [SerializeField]
  protected GameObject _buyContainer;
  [SerializeField]
  protected LoadingControl _loadingControl;
  [SerializeField]
  protected GameObject _requireInternetContainer;
  [Space]
  [SerializeField]
  protected KawaseBlurRendererSO _kawaseBlurRenderer;
  [Inject]
  protected AdditionalContentModel _additionalContentModel;
  [Inject]
  protected DlcPromoPanelModel _dlcPromoPanelModel;
  [Inject]
  protected IAnalyticsModel _analyticsModel;
  protected EventBinder _eventBinder = new EventBinder();
  protected CancellationTokenSource _cancellationTokenSource;
  protected IBeatmapLevelPack _pack;
  protected Sprite _blurredPackArtwork;

  public virtual void SetData(IBeatmapLevelPack pack)
  {
    this._pack = pack;
    if ((UnityEngine.Object) this._blurredPackArtwork != (UnityEngine.Object) null)
    {
      UnityEngine.Object.Destroy((UnityEngine.Object) this._blurredPackArtwork);
      this._blurredPackArtwork = (Sprite) null;
    }
    int downsample = 2;
    Texture2D texture = this._kawaseBlurRenderer.Blur((Texture) pack.coverImage.texture, KawaseBlurRendererSO.KernelSize.Kernel7, downsample);
    this._blurredPackArtwork = Sprite.Create(texture, new Rect(0.0f, 0.0f, (float) texture.width, (float) texture.height), new Vector2(0.5f, 0.5f), (float) (1024 >> downsample), 0U, SpriteMeshType.FullRect, new Vector4(0.0f, 0.0f, 0.0f, 0.0f), false);
    this._packImage.sprite = pack.coverImage;
    this.RefreshAvailabilityAsync();
    this._analyticsModel.LogImpression("Level Pack Detail", new Dictionary<string, string>()
    {
      ["item_id"] = this._pack.packID
    });
  }

  protected override void DidActivate(
    bool firstActivation,
    bool addedToHierarchy,
    bool screenSystemEnabling)
  {
    if (firstActivation)
    {
      this.buttonBinder.AddBinding(this._buyButton, new System.Action(this.BuyPackButtonWasPressed));
      System.Action handleDidPressRefreshButton = (System.Action) (() => this.RefreshAvailabilityAsync());
      this._eventBinder.Bind((System.Action) (() => this._loadingControl.didPressRefreshButtonEvent += handleDidPressRefreshButton), (System.Action) (() => this._loadingControl.didPressRefreshButtonEvent -= handleDidPressRefreshButton));
    }
    this._additionalContentModel.didInvalidateDataEvent += new System.Action(this.HandleAdditionalContentModelDidInvalidateData);
    this.RefreshAvailabilityAsync();
  }

  protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
  {
    this._additionalContentModel.didInvalidateDataEvent -= new System.Action(this.HandleAdditionalContentModelDidInvalidateData);
    this._cancellationTokenSource?.Cancel();
  }

  protected override void OnDestroy()
  {
    this._additionalContentModel.didInvalidateDataEvent -= new System.Action(this.HandleAdditionalContentModelDidInvalidateData);
    this._eventBinder.ClearAllBindings();
    if ((UnityEngine.Object) this._blurredPackArtwork != (UnityEngine.Object) null)
    {
      UnityEngine.Object.Destroy((UnityEngine.Object) this._blurredPackArtwork);
      this._blurredPackArtwork = (Sprite) null;
    }
    base.OnDestroy();
  }

  public virtual async void RefreshAvailabilityAsync()
  {
    LevelPackDetailViewController detailViewController = this;
    if (!detailViewController.isActiveAndEnabled)
      return;
    try
    {
      // ISSUE: explicit non-virtual call
      __nonvirtual (detailViewController.ShowContent(LevelPackDetailViewController.ContentType.Loading));
      detailViewController._cancellationTokenSource?.Cancel();
      detailViewController._cancellationTokenSource = new CancellationTokenSource();
      detailViewController._requireInternetContainer.SetActive(false);
      switch (await detailViewController._additionalContentModel.GetPackEntitlementStatusAsync(detailViewController._pack.packID, detailViewController._cancellationTokenSource.Token))
      {
        case AdditionalContentModel.EntitlementStatus.Owned:
          // ISSUE: explicit non-virtual call
          __nonvirtual (detailViewController.ShowContent(LevelPackDetailViewController.ContentType.Owned));
          break;
        case AdditionalContentModel.EntitlementStatus.NotOwned:
          // ISSUE: explicit non-virtual call
          __nonvirtual (detailViewController.ShowContent(LevelPackDetailViewController.ContentType.Buy));
          break;
        default:
          // ISSUE: explicit non-virtual call
          __nonvirtual (detailViewController.ShowContent(LevelPackDetailViewController.ContentType.Error, "Error loading data."));
          break;
      }
    }
    catch (OperationCanceledException ex)
    {
    }
  }

  public virtual void BuyPackButtonWasPressed()
  {
    this._dlcPromoPanelModel.BuyPackButtonWasPressed(this._pack, "Music Pack", "Buy - " + this._pack.packID);
    this.OpenLevelPackProductStoreAsync();
  }

  public virtual async void OpenLevelPackProductStoreAsync()
  {
    try
    {
      this.ShowContent(LevelPackDetailViewController.ContentType.Loading);
      this._cancellationTokenSource?.Cancel();
      this._cancellationTokenSource = new CancellationTokenSource();
      int num = (int) await this._additionalContentModel.OpenLevelPackProductStoreAsync(this._pack.packID, this._cancellationTokenSource.Token);
      this.RefreshAvailabilityAsync();
    }
    catch (OperationCanceledException ex)
    {
    }
  }

  public virtual void ShowContent(
    LevelPackDetailViewController.ContentType contentType,
    string errorText = "")
  {
    this._detailWrapper.SetActive(contentType == LevelPackDetailViewController.ContentType.Owned || contentType == LevelPackDetailViewController.ContentType.Buy);
    this._buyContainer.gameObject.SetActive(contentType == LevelPackDetailViewController.ContentType.Buy);
    if (contentType == LevelPackDetailViewController.ContentType.Buy)
    {
      this._dlcPromoPanelModel.BuyPackButtonWasShown(this._pack, "Music Pack", "Buy " + this._pack.packName);
      this._packImage.sprite = this._blurredPackArtwork;
      this._packImage.color = new Color(1f, 1f, 1f, 0.7f);
    }
    else
    {
      this._packImage.sprite = this._pack.coverImage;
      this._packImage.color = Color.white;
    }
    if (contentType != LevelPackDetailViewController.ContentType.Loading)
    {
      if (contentType == LevelPackDetailViewController.ContentType.Error)
        this._loadingControl.ShowText(errorText, true);
      else
        this._loadingControl.Hide();
    }
    else
      this._loadingControl.ShowLoading();
  }

  public virtual void HandleAdditionalContentModelDidInvalidateData() => this.RefreshAvailabilityAsync();

  [CompilerGenerated]
  public virtual void m_CDidActivatem_Eb__16_0() => this.RefreshAvailabilityAsync();

  public enum ContentType
  {
    Loading,
    Owned,
    Buy,
    Error,
  }
}
