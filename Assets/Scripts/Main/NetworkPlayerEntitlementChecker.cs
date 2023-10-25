// Decompiled with JetBrains decompiler
// Type: NetworkPlayerEntitlementChecker
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class NetworkPlayerEntitlementChecker : MonoBehaviour
{
  [Inject]
  protected readonly IMenuRpcManager _rpcManager;
  [Inject]
  protected readonly AdditionalContentModel _additionalContentModel;

  public virtual void Start() => this._rpcManager.getIsEntitledToLevelEvent += new System.Action<string, string>(this.HandleGetIsEntitledToLevel);

  public virtual void OnDestroy()
  {
    if (this._rpcManager != null)
      this._rpcManager.getIsEntitledToLevelEvent -= new System.Action<string, string>(this.HandleGetIsEntitledToLevel);
    if (!((UnityEngine.Object) this._additionalContentModel != (UnityEngine.Object) null))
      return;
    this._additionalContentModel.didInvalidateDataEvent -= new System.Action(this.HandleDataInvalidated);
  }

  public virtual void HandleDataInvalidated() => this._rpcManager.InvalidateLevelEntitlementStatuses();

  public virtual async void HandleGetIsEntitledToLevel(string userId, string levelId)
  {
    EntitlementsStatus entitlementStatus = await this.GetEntitlementStatus(levelId);
    this._rpcManager.SetIsEntitledToLevel(levelId, entitlementStatus);
  }

  public virtual async Task<EntitlementsStatus> GetEntitlementStatus(string levelId)
  {
    CancellationToken token = new CancellationToken();
    EntitlementsStatus entitlementStatus;
    switch (await this._additionalContentModel.GetLevelEntitlementStatusAsync(levelId, token))
    {
      case AdditionalContentModel.EntitlementStatus.Owned:
        entitlementStatus = EntitlementsStatus.Ok;
        break;
      case AdditionalContentModel.EntitlementStatus.NotOwned:
        entitlementStatus = EntitlementsStatus.NotOwned;
        break;
      default:
        entitlementStatus = EntitlementsStatus.Unknown;
        break;
    }
    return entitlementStatus;
  }
}
