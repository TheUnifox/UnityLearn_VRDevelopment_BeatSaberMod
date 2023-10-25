// Decompiled with JetBrains decompiler
// Type: TestPlatformAdditionalContentModel
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class TestPlatformAdditionalContentModel : AdditionalContentModel
{
  [SerializeField]
  protected TestPlatformAdditionalContentModel.Entitlement[] _levelsEntitlements;
  [SerializeField]
  protected TestPlatformAdditionalContentModel.Entitlement[] _levelPacksEntitlements;
  [SerializeField]
  protected bool _packBetterBuyThanLevel = true;

  protected override void InvalidateDataInternal()
  {
  }

  protected override async Task<AdditionalContentModel.EntitlementStatus> GetLevelEntitlementStatusInternalAsync(
    string levelId,
    CancellationToken token)
  {
    await Task.Yield();
    token.ThrowIfCancellationRequested();
    foreach (TestPlatformAdditionalContentModel.Entitlement levelsEntitlement in this._levelsEntitlements)
    {
      if (levelsEntitlement.id == levelId)
        return levelsEntitlement.status;
    }
    return AdditionalContentModel.EntitlementStatus.NotOwned;
  }

  protected override async Task<AdditionalContentModel.EntitlementStatus> GetPackEntitlementStatusInternalAsync(
    string levelPackId,
    CancellationToken token)
  {
    await Task.Yield();
    token.ThrowIfCancellationRequested();
    foreach (TestPlatformAdditionalContentModel.Entitlement packsEntitlement in this._levelPacksEntitlements)
    {
      if (packsEntitlement.id == levelPackId)
        return packsEntitlement.status;
    }
    return AdditionalContentModel.EntitlementStatus.NotOwned;
  }

  public override async Task<AdditionalContentModel.IsPackBetterBuyThanLevelResult> IsPackBetterBuyThanLevelAsync(
    string levelPackId,
    CancellationToken token)
  {
    await Task.Yield();
    token.ThrowIfCancellationRequested();
    return this._packBetterBuyThanLevel ? AdditionalContentModel.IsPackBetterBuyThanLevelResult.PackIsBetter : AdditionalContentModel.IsPackBetterBuyThanLevelResult.LevelIsBetter;
  }

  public override async Task<AdditionalContentModel.OpenProductStoreResult> OpenLevelProductStoreAsync(
    string levelId,
    CancellationToken token)
  {
    TestPlatformAdditionalContentModel additionalContentModel = this;
    await Task.Yield();
    token.ThrowIfCancellationRequested();
    Debug.Log((object) ("Opening test product store for levelId " + levelId));
    // ISSUE: explicit non-virtual call
    __nonvirtual (additionalContentModel.BuyLevel(levelId));
    additionalContentModel.InvalidateData();
    return AdditionalContentModel.OpenProductStoreResult.OK;
  }

  public virtual void BuyLevel(string levelId)
  {
    TestPlatformAdditionalContentModel.Entitlement entitlement = (TestPlatformAdditionalContentModel.Entitlement) null;
    foreach (TestPlatformAdditionalContentModel.Entitlement levelsEntitlement in this._levelsEntitlements)
    {
      if (levelsEntitlement.id == levelId)
      {
        entitlement = levelsEntitlement;
        break;
      }
    }
    if (entitlement == null)
    {
      entitlement = new TestPlatformAdditionalContentModel.Entitlement();
      entitlement.id = levelId;
      this._levelsEntitlements = new List<TestPlatformAdditionalContentModel.Entitlement>((IEnumerable<TestPlatformAdditionalContentModel.Entitlement>) this._levelsEntitlements)
      {
        entitlement
      }.ToArray();
    }
    entitlement.status = AdditionalContentModel.EntitlementStatus.Owned;
  }

  public override async Task<AdditionalContentModel.OpenProductStoreResult> OpenLevelPackProductStoreAsync(
    string levelPackId,
    CancellationToken token)
  {
    await Task.Yield();
    token.ThrowIfCancellationRequested();
    Debug.Log((object) ("Opening test product store for levelPackId " + levelPackId));
    return AdditionalContentModel.OpenProductStoreResult.OK;
  }

  [Serializable]
  public class Entitlement
  {
    public string id;
    public AdditionalContentModel.EntitlementStatus status;
  }
}
