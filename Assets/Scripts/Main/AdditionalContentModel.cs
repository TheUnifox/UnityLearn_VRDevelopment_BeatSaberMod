// Decompiled with JetBrains decompiler
// Type: AdditionalContentModel
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public abstract class AdditionalContentModel : MonoBehaviour
{
  [Inject]
  private AlwaysOwnedContentContainerSO _alwaysOwnedContentContainer;

  public event System.Action didInvalidateDataEvent;

  protected void OnApplicationFocus(bool hasFocus)
  {
    if (!hasFocus)
      return;
    this.InvalidateData();
  }

  protected void InvalidateData()
  {
    this.InvalidateDataInternal();
    System.Action invalidateDataEvent = this.didInvalidateDataEvent;
    if (invalidateDataEvent == null)
      return;
    invalidateDataEvent();
  }

  public async Task<AdditionalContentModel.EntitlementStatus> GetLevelEntitlementStatusAsync(
    string levelId,
    CancellationToken token)
  {
    return this._alwaysOwnedContentContainer.alwaysOwnedBeatmapLevelIds.Contains(levelId) || levelId.StartsWith("custom_level_") ? AdditionalContentModel.EntitlementStatus.Owned : await this.GetLevelEntitlementStatusInternalAsync(levelId, token);
  }

  public async Task<AdditionalContentModel.EntitlementStatus> GetPackEntitlementStatusAsync(
    string levelPackId,
    CancellationToken token)
  {
    return this._alwaysOwnedContentContainer.alwaysOwnedPacksIds.Contains(levelPackId) || levelPackId.StartsWith("custom_levelpack_") ? AdditionalContentModel.EntitlementStatus.Owned : await this.GetPackEntitlementStatusInternalAsync(levelPackId, token);
  }

  protected abstract void InvalidateDataInternal();

  protected abstract Task<AdditionalContentModel.EntitlementStatus> GetLevelEntitlementStatusInternalAsync(
    string levelId,
    CancellationToken token);

  protected abstract Task<AdditionalContentModel.EntitlementStatus> GetPackEntitlementStatusInternalAsync(
    string levelPackId,
    CancellationToken token);

  public abstract Task<AdditionalContentModel.IsPackBetterBuyThanLevelResult> IsPackBetterBuyThanLevelAsync(
    string levelPackId,
    CancellationToken token);

  public abstract Task<AdditionalContentModel.OpenProductStoreResult> OpenLevelProductStoreAsync(
    string levelId,
    CancellationToken token);

  public abstract Task<AdditionalContentModel.OpenProductStoreResult> OpenLevelPackProductStoreAsync(
    string levelPackId,
    CancellationToken token);

  public enum EntitlementStatus
  {
    Failed,
    Owned,
    NotOwned,
  }

  public enum OpenProductStoreResult
  {
    OK,
    Failed,
  }

  public enum UpdateEntitlementsResult
  {
    OK,
    Failed,
  }

  public enum IsPackBetterBuyThanLevelResult
  {
    PackIsBetter,
    LevelIsBetter,
    Failed,
  }
}
