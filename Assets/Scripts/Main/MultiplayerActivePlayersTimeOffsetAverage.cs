// Decompiled with JetBrains decompiler
// Type: MultiplayerActivePlayersTimeOffsetAverage
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class MultiplayerActivePlayersTimeOffsetAverage : IMultiplayerObservable
{
  [Inject]
  protected readonly IMultiplayerSessionManager _multiplayerSessionManager;
  protected float _lastReturnedOffsetSyncTime;
  protected float _timeOfLastValidReturnedTime;

  public float offsetSyncTime
  {
    get
    {
      float num1 = 0.0f;
      int num2 = 0;
      for (int index = 0; index < this._multiplayerSessionManager.connectedPlayerCount; ++index)
      {
        IConnectedPlayer connectedPlayer = this._multiplayerSessionManager.GetConnectedPlayer(index);
        if (connectedPlayer.IsActive())
        {
          num1 += connectedPlayer.offsetSyncTime;
          ++num2;
        }
      }
      if (num2 <= 0)
        return this._lastReturnedOffsetSyncTime + (Time.timeSinceLevelLoad - this._timeOfLastValidReturnedTime);
      float offsetSyncTime = num1 / (float) num2;
      this._lastReturnedOffsetSyncTime = offsetSyncTime;
      this._timeOfLastValidReturnedTime = Time.timeSinceLevelLoad;
      return offsetSyncTime;
    }
  }

  public bool isFailed
  {
    get
    {
      for (int index = 0; index < this._multiplayerSessionManager.connectedPlayerCount; ++index)
      {
        if (!this._multiplayerSessionManager.GetConnectedPlayer(index).IsFailed())
          return false;
      }
      return true;
    }
  }
}
