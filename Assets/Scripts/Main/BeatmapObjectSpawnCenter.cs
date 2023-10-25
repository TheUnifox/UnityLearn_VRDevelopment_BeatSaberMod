// Decompiled with JetBrains decompiler
// Type: BeatmapObjectSpawnCenter
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using UnityEngine;

public class BeatmapObjectSpawnCenter : MonoBehaviour
{
  [SerializeField]
  protected BeatmapObjectSpawnCenter.PlayerCountToDistance[] _distances;
  [SerializeField]
  protected float _defaultDistnace = 40f;
  protected bool _spawnCenterDistanceWasFound;
  protected float _spawnCenterDistance;

  public float spawnCenterDistance => this._spawnCenterDistance;

  public bool spawnCenterDistanceWasFound => this._spawnCenterDistanceWasFound;

  public event System.Action<float> spawnCenterDistanceWasFoundEvent;

  public virtual float CalculateSpawnCenterPosition(int numberOfPlayers)
  {
    foreach (BeatmapObjectSpawnCenter.PlayerCountToDistance distance in this._distances)
    {
      if (distance.playerCount == numberOfPlayers)
      {
        this.ReportAndSaveSpawnCenterDistance(distance.distance);
        return distance.distance;
      }
    }
    Debug.LogWarning((object) string.Format("Didn't find distance for {0} number of players, using default", (object) numberOfPlayers));
    this.ReportAndSaveSpawnCenterDistance(this._defaultDistnace);
    return this._defaultDistnace;
  }

  public virtual void ReportAndSaveSpawnCenterDistance(float distance)
  {
    this._spawnCenterDistance = distance;
    this._spawnCenterDistanceWasFound = true;
    System.Action<float> distanceWasFoundEvent = this.spawnCenterDistanceWasFoundEvent;
    if (distanceWasFoundEvent == null)
      return;
    distanceWasFoundEvent(distance);
  }

  [Serializable]
  public class PlayerCountToDistance
  {
    [SerializeField]
    protected int _playerCount;
    [SerializeField]
    protected float _distance;

    public int playerCount => this._playerCount;

    public float distance => this._distance;
  }
}
