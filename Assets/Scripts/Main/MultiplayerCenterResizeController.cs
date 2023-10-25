// Decompiled with JetBrains decompiler
// Type: MultiplayerCenterResizeController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Runtime.CompilerServices;
using UnityEngine;
using Zenject;

public class MultiplayerCenterResizeController : MonoBehaviour
{
  [SerializeField]
  protected float _platformWidth;
  [Inject]
  protected readonly MultiplayerLayoutProvider _layoutProvider;
  [CompilerGenerated]
  protected bool m_CisEdgeDistanceFromCenterCalculated;
  [CompilerGenerated]
  protected float m_CedgeDistanceFromCenter;

  public event System.Action<float> edgeDistanceFromCenterWasCalculatedEvent;

  public bool isEdgeDistanceFromCenterCalculated
  {
    get => this.m_CisEdgeDistanceFromCenterCalculated;
    private set => this.m_CisEdgeDistanceFromCenterCalculated = value;
  }

  public float edgeDistanceFromCenter
  {
    get => this.m_CedgeDistanceFromCenter;
    private set => this.m_CedgeDistanceFromCenter = value;
  }

  public virtual void Start()
  {
    if (this._layoutProvider.layout != MultiplayerPlayerLayout.NotDetermined)
      this.HandlePlayersLayoutWasCalculated(this._layoutProvider.layout, this._layoutProvider.activePlayerSpotsCount);
    else
      this._layoutProvider.playersLayoutWasCalculatedEvent += new System.Action<MultiplayerPlayerLayout, int>(this.HandlePlayersLayoutWasCalculated);
  }

  public virtual void OnDestroy()
  {
    if (this._layoutProvider == null)
      return;
    this._layoutProvider.playersLayoutWasCalculatedEvent -= new System.Action<MultiplayerPlayerLayout, int>(this.HandlePlayersLayoutWasCalculated);
  }

  public virtual void HandlePlayersLayoutWasCalculated(
    MultiplayerPlayerLayout layout,
    int numberOfPlayers)
  {
    if (layout != MultiplayerPlayerLayout.Circle)
      return;
    if (numberOfPlayers == 1)
      numberOfPlayers = 3;
    this.edgeDistanceFromCenter = this._platformWidth / (2f * Mathf.Tan(3.14159274f / (float) numberOfPlayers));
    this.isEdgeDistanceFromCenterCalculated = true;
    System.Action<float> wasCalculatedEvent = this.edgeDistanceFromCenterWasCalculatedEvent;
    if (wasCalculatedEvent == null)
      return;
    wasCalculatedEvent(this.edgeDistanceFromCenter);
  }
}
