// Decompiled with JetBrains decompiler
// Type: OculusPlatformAdditionalContentModel
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Oculus.Platform;
using Oculus.Platform.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Zenject;

public class OculusPlatformAdditionalContentModel : AdditionalContentModel
{
  [Inject]
  protected OculusLevelProductsModelSO _oculusLevelProductsModel;
  protected HashSet<string> _entitlementsSKU = new HashSet<string>();
  protected SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);
  protected bool _isDataValid;

  protected override void InvalidateDataInternal() => this._isDataValid = false;

  protected override async Task<AdditionalContentModel.EntitlementStatus> GetLevelEntitlementStatusInternalAsync(
    string levelId,
    CancellationToken cancellationToken)
  {
    return !await this.DataIsValidAsync(cancellationToken) ? AdditionalContentModel.EntitlementStatus.Failed : (this.HasLevelEntitlement(levelId) ? AdditionalContentModel.EntitlementStatus.Owned : AdditionalContentModel.EntitlementStatus.NotOwned);
  }

  protected override async Task<AdditionalContentModel.EntitlementStatus> GetPackEntitlementStatusInternalAsync(
    string packId,
    CancellationToken cancellationToken)
  {
    return !await this.DataIsValidAsync(cancellationToken) ? AdditionalContentModel.EntitlementStatus.Failed : (this.HasLevelPackEntitlement(packId) ? AdditionalContentModel.EntitlementStatus.Owned : AdditionalContentModel.EntitlementStatus.NotOwned);
  }

  public virtual async Task<bool> DataIsValidAsync(CancellationToken cancellationToken)
  {
    await this._semaphoreSlim.WaitAsync(cancellationToken);
    try
    {
      if (!this._isDataValid)
        this._isDataValid = await this.CheckForNewEntitlementsAsync(cancellationToken) == AdditionalContentModel.UpdateEntitlementsResult.OK;
    }
    finally
    {
      this._semaphoreSlim.Release();
    }
    return this._isDataValid;
  }

  public override async Task<AdditionalContentModel.OpenProductStoreResult> OpenLevelProductStoreAsync(
    string levelId,
    CancellationToken cancellationToken)
  {
    OculusPlatformAdditionalContentModel additionalContentModel = this;
    string sku = additionalContentModel._oculusLevelProductsModel.GetLevelProductData(levelId).sku;
    // ISSUE: explicit non-virtual call
    Message<Purchase> message = await __nonvirtual (additionalContentModel.LaunchCheckoutFlow(sku));
    if (message.IsError)
    {
      additionalContentModel.InvalidateData();
      cancellationToken.ThrowIfCancellationRequested();
      return AdditionalContentModel.OpenProductStoreResult.Failed;
    }
    if (!string.IsNullOrEmpty(message.Data.Sku))
    {
      additionalContentModel._entitlementsSKU.Add(message.Data.Sku);
      await Task.Delay(2500);
      additionalContentModel.InvalidateData();
      cancellationToken.ThrowIfCancellationRequested();
      return AdditionalContentModel.OpenProductStoreResult.OK;
    }
    additionalContentModel.InvalidateData();
    cancellationToken.ThrowIfCancellationRequested();
    return AdditionalContentModel.OpenProductStoreResult.OK;
  }

  public override async Task<AdditionalContentModel.OpenProductStoreResult> OpenLevelPackProductStoreAsync(
    string levelPackId,
    CancellationToken cancellationToken)
  {
    OculusPlatformAdditionalContentModel additionalContentModel = this;
    string sku = additionalContentModel._oculusLevelProductsModel.GetLevelPackProductData(levelPackId).sku;
    // ISSUE: explicit non-virtual call
    Message<Purchase> message = await __nonvirtual (additionalContentModel.LaunchCheckoutFlow(sku));
    if (message.IsError)
    {
      additionalContentModel.InvalidateData();
      cancellationToken.ThrowIfCancellationRequested();
      return AdditionalContentModel.OpenProductStoreResult.Failed;
    }
    if (!string.IsNullOrEmpty(message.Data.Sku))
    {
      OculusLevelProductsModelSO.LevelPackProductData levelPackProductData = additionalContentModel._oculusLevelProductsModel.GetLevelPackProductData(levelPackId);
      if (levelPackProductData != null)
      {
        foreach (OculusLevelProductsModelSO.LevelProductData levelProductData in levelPackProductData.levelProductsData)
          additionalContentModel._entitlementsSKU.Add(levelProductData.sku);
      }
      await Task.Delay(2500);
      additionalContentModel.InvalidateData();
      cancellationToken.ThrowIfCancellationRequested();
      return AdditionalContentModel.OpenProductStoreResult.OK;
    }
    additionalContentModel.InvalidateData();
    cancellationToken.ThrowIfCancellationRequested();
    return AdditionalContentModel.OpenProductStoreResult.OK;
  }

  public virtual async Task<Message<Purchase>> LaunchCheckoutFlow(string sku)
  {
    TaskCompletionSource<Message<Purchase>> launchCheckoutFlowTaskSource = new TaskCompletionSource<Message<Purchase>>();
    IAP.LaunchCheckoutFlow(sku).OnComplete((Message<Purchase>.Callback) (msg => launchCheckoutFlowTaskSource.TrySetResult(msg)));
    return await launchCheckoutFlowTaskSource.Task;
  }

  public override async Task<AdditionalContentModel.IsPackBetterBuyThanLevelResult> IsPackBetterBuyThanLevelAsync(
    string levelPackId,
    CancellationToken token)
  {
    return await Task.FromResult<AdditionalContentModel.IsPackBetterBuyThanLevelResult>(AdditionalContentModel.IsPackBetterBuyThanLevelResult.PackIsBetter);
  }

  public virtual async Task<AdditionalContentModel.UpdateEntitlementsResult> CheckForNewEntitlementsAsync(
    CancellationToken cancellationToken)
  {
    TaskCompletionSource<AdditionalContentModel.UpdateEntitlementsResult> getViewerPurchasesTaskSource = new TaskCompletionSource<AdditionalContentModel.UpdateEntitlementsResult>();
    AssetFile.GetList().OnComplete((Message<AssetDetailsList>.Callback) (getListMsg =>
    {
      if (cancellationToken.IsCancellationRequested)
        getViewerPurchasesTaskSource.TrySetCanceled(cancellationToken);
      else if (!getListMsg.IsError)
      {
        bool flag = true;
        foreach (AssetDetails assetDetails in (DeserializableList<AssetDetails>) getListMsg.Data)
        {
          string levelSku = this._oculusLevelProductsModel.GetLevelSku(Path.GetFileName(assetDetails.Filepath));
          if (assetDetails.IapStatus == "free" || assetDetails.IapStatus == "entitled")
            this._entitlementsSKU.Add(levelSku);
          else
            flag = false;
        }
        if (flag || !UnityEngine.Application.isMobilePlatform)
          getViewerPurchasesTaskSource.TrySetResult(AdditionalContentModel.UpdateEntitlementsResult.OK);
        else
          IAP.GetViewerPurchases().OnComplete((Message<PurchaseList>.Callback) (getPurchasesMsg =>
          {
            if (cancellationToken.IsCancellationRequested)
            {
              getViewerPurchasesTaskSource.TrySetCanceled(cancellationToken);
            }
            else
            {
              if (!getPurchasesMsg.IsError)
              {
                foreach (Purchase purchase in (DeserializableList<Purchase>) getPurchasesMsg.Data)
                  this._entitlementsSKU.Add(purchase.Sku);
              }
              getViewerPurchasesTaskSource.TrySetResult(AdditionalContentModel.UpdateEntitlementsResult.OK);
            }
          }));
      }
      else
        getViewerPurchasesTaskSource.TrySetResult(AdditionalContentModel.UpdateEntitlementsResult.Failed);
    }));
    return await getViewerPurchasesTaskSource.Task;
  }

  public virtual bool HasLevelEntitlement(string levelId) => this._entitlementsSKU.Contains(this._oculusLevelProductsModel.GetLevelProductData(levelId).sku);

  public virtual bool HasLevelPackEntitlement(string levelPackId)
  {
    OculusLevelProductsModelSO.LevelPackProductData levelPackProductData = this._oculusLevelProductsModel.GetLevelPackProductData(levelPackId);
    if (levelPackProductData == null)
      return false;
    foreach (OculusLevelProductsModelSO.LevelProductData levelProductData in levelPackProductData.levelProductsData)
    {
      if (!this._entitlementsSKU.Contains(levelProductData.sku))
        return false;
    }
    return true;
  }
}
