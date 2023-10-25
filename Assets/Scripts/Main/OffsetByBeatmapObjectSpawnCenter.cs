// Decompiled with JetBrains decompiler
// Type: OffsetByBeatmapObjectSpawnCenter
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class OffsetByBeatmapObjectSpawnCenter : MonoBehaviour
{
  [Inject]
  protected readonly BeatmapObjectSpawnCenter _spawnCenter;

  public virtual void Start()
  {
    if (this._spawnCenter.spawnCenterDistanceWasFound)
      this.HandleSpawnCenterDistanceWasFound(this._spawnCenter.spawnCenterDistance);
    else
      this._spawnCenter.spawnCenterDistanceWasFoundEvent += new System.Action<float>(this.HandleSpawnCenterDistanceWasFound);
  }

  public virtual void HandleSpawnCenterDistanceWasFound(float distance)
  {
    this.transform.position += new Vector3(0.0f, 0.0f, distance);
    this.enabled = false;
  }
}
