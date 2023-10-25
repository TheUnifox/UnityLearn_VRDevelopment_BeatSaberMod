// Decompiled with JetBrains decompiler
// Type: CampaignProgressModel
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CampaignProgressModel : MonoBehaviour
{
  [Inject]
  protected PlayerDataModel _playerDataModel;
  protected HashSet<string> _missionIds;
  protected string _finalMissionId;
  protected bool _numberOfClearedMissionsDirty = true;
  protected int _numberOfClearedMissions;

  public int numberOfClearedMissions
  {
    get
    {
      if (this._numberOfClearedMissionsDirty)
      {
        this.UpdateNumberOfClearedMissions();
        this._numberOfClearedMissionsDirty = false;
      }
      return this._numberOfClearedMissions;
    }
  }

  public virtual void Awake() => this._missionIds = new HashSet<string>();

  public virtual bool IsMissionRegistered(string missionId) => this._missionIds.Contains(missionId);

  public virtual void RegisterMissionId(string missionId)
  {
    this._missionIds.Add(missionId);
    this._numberOfClearedMissionsDirty = true;
  }

  public virtual bool IsMissionCleared(string missionId) => this._playerDataModel.playerData.GetPlayerMissionStatsData(missionId).cleared;

  public virtual bool IsMissionFinal(string missionId) => this._finalMissionId == missionId;

  public virtual void SetFinalMissionId(string missionId) => this._finalMissionId = missionId;

  public virtual bool WillFinishGameAfterThisMission(string missionId) => this.IsMissionFinal(missionId) && !this.IsMissionCleared(missionId);

  public virtual void SetMissionCleared(string missionId) => this.__SetMissionCleared(missionId, true);

  public virtual void __SetMissionCleared(string missionId, bool cleared)
  {
    this._playerDataModel.playerData.GetPlayerMissionStatsData(missionId).cleared = cleared;
    this._numberOfClearedMissionsDirty = true;
  }

  public virtual int UpdateNumberOfClearedMissions()
  {
    this._numberOfClearedMissions = 0;
    foreach (string missionId in this._missionIds)
    {
      if (this._playerDataModel.playerData.GetPlayerMissionStatsData(missionId).cleared)
        ++this._numberOfClearedMissions;
    }
    return this._numberOfClearedMissions;
  }
}
