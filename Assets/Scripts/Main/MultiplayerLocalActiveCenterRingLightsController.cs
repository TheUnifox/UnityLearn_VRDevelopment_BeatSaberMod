// Decompiled with JetBrains decompiler
// Type: MultiplayerLocalActiveCenterRingLightsController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class MultiplayerLocalActiveCenterRingLightsController : MonoBehaviour
{
  [SerializeField]
  protected float[] _verticalLinePositions;
  [SerializeField]
  protected Transform[] _horizontalLines;
  [Inject]
  protected readonly MultiplayerCenterResizeController _centerResizeController;
  [Inject]
  protected readonly MultiplayerLayoutProvider _layoutProvider;
  [Inject]
  protected readonly BeatmapObjectSpawnCenter _beatmapObjectSpawnCenter;
  protected bool _edgeDistanceFromCenterCalculated;
  protected bool _spawnCenterDistanceFound;

  public virtual void Start()
  {
    foreach (Component horizontalLine in this._horizontalLines)
      horizontalLine.gameObject.SetActive(false);
    if (this._centerResizeController.isEdgeDistanceFromCenterCalculated)
      this.HandleEdgeDistanceFromCenterWasCalculated(this._centerResizeController.edgeDistanceFromCenter);
    else
      this._centerResizeController.edgeDistanceFromCenterWasCalculatedEvent += new System.Action<float>(this.HandleEdgeDistanceFromCenterWasCalculated);
    if (this._beatmapObjectSpawnCenter.spawnCenterDistanceWasFound)
      this.HandleSpawnCenterDistanceWasFound(this._beatmapObjectSpawnCenter.spawnCenterDistance);
    else
      this._beatmapObjectSpawnCenter.spawnCenterDistanceWasFoundEvent += new System.Action<float>(this.HandleSpawnCenterDistanceWasFound);
  }

  public virtual void OnDestroy()
  {
    if ((UnityEngine.Object) this._centerResizeController != (UnityEngine.Object) null)
      this._centerResizeController.edgeDistanceFromCenterWasCalculatedEvent -= new System.Action<float>(this.HandleEdgeDistanceFromCenterWasCalculated);
    if (!((UnityEngine.Object) this._beatmapObjectSpawnCenter != (UnityEngine.Object) null))
      return;
    this._beatmapObjectSpawnCenter.spawnCenterDistanceWasFoundEvent -= new System.Action<float>(this.HandleSpawnCenterDistanceWasFound);
  }

  public virtual void TryResize()
  {
    if (!this._edgeDistanceFromCenterCalculated || !this._spawnCenterDistanceFound)
      return;
    this.Resize();
  }

  public virtual void Resize()
  {
    float distanceFromCenter = this._centerResizeController.edgeDistanceFromCenter;
    double spawnCenterDistance = (double) this._beatmapObjectSpawnCenter.spawnCenterDistance;
    int playerSpotsCount = this._layoutProvider.activePlayerSpotsCount;
    float num = 360f / (float) playerSpotsCount;
    Vector3 vector3_1 = new Vector3(0.0f, 0.0f, (float) spawnCenterDistance);
    Quaternion quaternion1 = Quaternion.Euler(0.0f, 0.0f, 90f);
    for (int index1 = 1; index1 < playerSpotsCount; ++index1)
    {
      Quaternion quaternion2 = Quaternion.Euler(0.0f, num * (float) index1, 0.0f);
      for (int index2 = 0; index2 < this._verticalLinePositions.Length; ++index2)
      {
        Vector3 vector3_2 = new Vector3(0.0f, this._verticalLinePositions[index2], -distanceFromCenter);
        Vector3 vector3_3 = quaternion2 * vector3_2 + vector3_1;
        int index3 = this._verticalLinePositions.Length * index1 + index2;
        Quaternion quaternion3 = quaternion2 * quaternion1;
        this._horizontalLines[index3].gameObject.SetActive(true);
        this._horizontalLines[index3].position = vector3_3;
        this._horizontalLines[index3].rotation = quaternion3;
      }
    }
  }

  public virtual void HandleSpawnCenterDistanceWasFound(float spawnCenterDistance)
  {
    this._spawnCenterDistanceFound = true;
    this.TryResize();
  }

  public virtual void HandleEdgeDistanceFromCenterWasCalculated(
    float constructEdgeDistanceFromCenter)
  {
    this._edgeDistanceFromCenterCalculated = true;
    this.TryResize();
  }
}
