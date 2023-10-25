// Decompiled with JetBrains decompiler
// Type: PS4PlatformAdditionalContentModel
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Threading;
using System.Threading.Tasks;

public class PS4PlatformAdditionalContentModel : AdditionalContentModel
{
  protected override void InvalidateDataInternal()
  {
  }

  protected override Task<AdditionalContentModel.EntitlementStatus> GetLevelEntitlementStatusInternalAsync(
    string levelId,
    CancellationToken token)
  {
    return Task.FromResult<AdditionalContentModel.EntitlementStatus>(AdditionalContentModel.EntitlementStatus.Failed);
  }

  protected override Task<AdditionalContentModel.EntitlementStatus> GetPackEntitlementStatusInternalAsync(
    string levelPackId,
    CancellationToken token)
  {
    return Task.FromResult<AdditionalContentModel.EntitlementStatus>(AdditionalContentModel.EntitlementStatus.Failed);
  }

  public override Task<AdditionalContentModel.IsPackBetterBuyThanLevelResult> IsPackBetterBuyThanLevelAsync(
    string levelPackId,
    CancellationToken token)
  {
    return Task.FromResult<AdditionalContentModel.IsPackBetterBuyThanLevelResult>(AdditionalContentModel.IsPackBetterBuyThanLevelResult.Failed);
  }

  public override Task<AdditionalContentModel.OpenProductStoreResult> OpenLevelProductStoreAsync(
    string levelId,
    CancellationToken token)
  {
    return Task.FromResult<AdditionalContentModel.OpenProductStoreResult>(AdditionalContentModel.OpenProductStoreResult.Failed);
  }

  public override Task<AdditionalContentModel.OpenProductStoreResult> OpenLevelPackProductStoreAsync(
    string levelPackId,
    CancellationToken token)
  {
    return Task.FromResult<AdditionalContentModel.OpenProductStoreResult>(AdditionalContentModel.OpenProductStoreResult.Failed);
  }
}
