// Decompiled with JetBrains decompiler
// Type: SteamPlatformAdditionalContentModel
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Zenject;

public class SteamPlatformAdditionalContentModel : AdditionalContentModel
{
  [Inject]
  protected SteamLevelProductsModelSO _steamLevelProductsModel;
  protected HashSet<uint> _entitlementsAppIds = new HashSet<uint>();
  protected TaskCompletionSource<bool> _dataIsValidTaskCompletionSource;
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
    CancellationToken token)
  {
    SteamLevelProductsModelSO.LevelProductData levelProductData = this._steamLevelProductsModel.GetLevelProductData(levelId);
    if (levelProductData == null)
      return await Task.FromResult<AdditionalContentModel.OpenProductStoreResult>(AdditionalContentModel.OpenProductStoreResult.Failed);
    this.OpenProductStore(levelProductData.appId);
    return await Task.FromResult<AdditionalContentModel.OpenProductStoreResult>(AdditionalContentModel.OpenProductStoreResult.OK);
  }

  public override async Task<AdditionalContentModel.OpenProductStoreResult> OpenLevelPackProductStoreAsync(
    string levelPackId,
    CancellationToken token)
  {
    SteamLevelProductsModelSO.LevelPackProductData levelPackProductData = this._steamLevelProductsModel.GetLevelPackProductData(levelPackId);
    if (levelPackProductData == null)
      return await Task.FromResult<AdditionalContentModel.OpenProductStoreResult>(AdditionalContentModel.OpenProductStoreResult.Failed);
    this.OpenBundleUrl(levelPackProductData.bundleId);
    return await Task.FromResult<AdditionalContentModel.OpenProductStoreResult>(AdditionalContentModel.OpenProductStoreResult.OK);
  }

  public virtual void OpenProductStore(uint appId) => this.InvalidateData();

  public virtual void OpenBundleUrl(uint bundleId) => this.InvalidateData();

  public override Task<AdditionalContentModel.IsPackBetterBuyThanLevelResult> IsPackBetterBuyThanLevelAsync(
    string levelPackId,
    CancellationToken token)
  {
    return Task.FromResult<AdditionalContentModel.IsPackBetterBuyThanLevelResult>(AdditionalContentModel.IsPackBetterBuyThanLevelResult.PackIsBetter);
  }

  public virtual async Task<AdditionalContentModel.UpdateEntitlementsResult> CheckForNewEntitlementsAsync(
    CancellationToken cancellationToken)
  {
    return await Task.FromResult<AdditionalContentModel.UpdateEntitlementsResult>(AdditionalContentModel.UpdateEntitlementsResult.OK);
  }

  public virtual bool HasLevelEntitlement(string levelId)
  {
    SteamLevelProductsModelSO.LevelProductData levelProductData = this._steamLevelProductsModel.GetLevelProductData(levelId);
    return levelProductData != null && this._entitlementsAppIds.Contains(levelProductData.appId);
  }

  public virtual bool HasLevelPackEntitlement(string levelPackId)
  {
    SteamLevelProductsModelSO.LevelPackProductData levelPackProductData = this._steamLevelProductsModel.GetLevelPackProductData(levelPackId);
    if (levelPackProductData == null)
      return false;
    foreach (SteamLevelProductsModelSO.LevelProductData levelProductData in levelPackProductData.levelProductsData)
    {
      if (!this._entitlementsAppIds.Contains(levelProductData.appId))
        return false;
    }
    return true;
  }
}
