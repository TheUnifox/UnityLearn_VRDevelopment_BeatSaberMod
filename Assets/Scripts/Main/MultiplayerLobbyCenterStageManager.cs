// Decompiled with JetBrains decompiler
// Type: MultiplayerLobbyCenterStageManager
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class MultiplayerLobbyCenterStageManager : MonoBehaviour
{
  [SerializeField]
  protected Transform _centerObjectTransform;
  [Space]
  [SerializeField]
  protected CenterStageScreenController _centerStageScreenController;
  [Inject]
  protected readonly ILobbyStateDataModel _lobbyStateDataModel;
  protected float _innerCircleRadius = 1f;
  protected float _minOuterCircleRadius = 4.4f;

  public virtual void Init(float innerCircleRadius, float minOuterCircleRadius)
  {
    this._minOuterCircleRadius = minOuterCircleRadius;
    this._innerCircleRadius = innerCircleRadius;
  }

  public virtual void ActivateCenterStageManager()
  {
    this.RecalculateCenterPosition();
    this._centerStageScreenController.Show();
  }

  public virtual void DeactivateCenterStageManager()
  {
    this.RecalculateCenterPosition();
    this._centerStageScreenController.Hide();
  }

  public virtual void RecalculateCenterPosition() => this._centerObjectTransform.localPosition = new Vector3(0.0f, 0.0f, Mathf.Max(MultiplayerPlayerPlacement.GetOuterCircleRadius(MultiplayerPlayerPlacement.GetAngleBetweenPlayersWithEvenAdjustment(this._lobbyStateDataModel.configuration.maxPlayerCount, MultiplayerPlayerLayout.Circle), this._innerCircleRadius), this._minOuterCircleRadius));
}
