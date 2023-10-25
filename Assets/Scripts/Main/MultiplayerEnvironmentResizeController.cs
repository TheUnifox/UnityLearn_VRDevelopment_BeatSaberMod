// Decompiled with JetBrains decompiler
// Type: MultiplayerEnvironmentResizeController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

public class MultiplayerEnvironmentResizeController : MonoBehaviour
{
  [SerializeField]
  protected Transform _platformEnd;
  [SerializeField]
  protected MultiplayerEnvironmentResizeController.ResizeData[] _resizeData;
  [Inject]
  protected readonly MultiplayerCenterResizeController _centerResizeController;
  [Inject]
  protected readonly BeatmapObjectSpawnCenter _beatmapObjectSpawnCenter;
  protected bool _isResizingFinished;
  protected bool _edgeDistanceFromCenterFound;
  protected bool _spawnCenterDistanceFound;

  public bool isResizingFinished => this._isResizingFinished;

  public event System.Action resizingDidFinishEvent;

  public virtual void Start()
  {
    if (!this._centerResizeController.isEdgeDistanceFromCenterCalculated)
      this._centerResizeController.edgeDistanceFromCenterWasCalculatedEvent += new System.Action<float>(this.HandleEdgeDistanceFromCenterWasCalculated);
    else
      this.HandleEdgeDistanceFromCenterWasCalculated(this._centerResizeController.edgeDistanceFromCenter);
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

  public virtual void HandleEdgeDistanceFromCenterWasCalculated(float edgeDistanceFromCenter)
  {
    this._edgeDistanceFromCenterFound = true;
    this.TryResize();
  }

  public virtual void HandleSpawnCenterDistanceWasFound(float distance)
  {
    this._spawnCenterDistanceFound = true;
    this.TryResize();
  }

  public virtual void TryResize()
  {
    if (!this._edgeDistanceFromCenterFound || !this._spawnCenterDistanceFound)
      return;
    this.Resize();
  }

  public virtual void Resize()
  {
    Assert.IsTrue(this._beatmapObjectSpawnCenter.spawnCenterDistanceWasFound);
    Assert.IsTrue(this._centerResizeController.isEdgeDistanceFromCenterCalculated);
    float num1 = this._beatmapObjectSpawnCenter.spawnCenterDistance - this._centerResizeController.edgeDistanceFromCenter;
    float num2 = num1 - this._platformEnd.localPosition.z;
    foreach (MultiplayerEnvironmentResizeController.ResizeData resizeData in this._resizeData)
    {
      bool flag1 = resizeData.resizeType.HasFlag((Enum) MultiplayerEnvironmentResizeController.ResizeType.Position);
      bool flag2 = resizeData.resizeType.HasFlag((Enum) MultiplayerEnvironmentResizeController.ResizeType.Length);
      foreach (TubeBloomPrePassLight light in resizeData.lights)
      {
        if (flag1)
        {
          Transform transform = light.transform;
          Vector3 localPosition = transform.localPosition;
          transform.localPosition = new Vector3(localPosition.x, localPosition.y, num1 + resizeData.offset);
        }
        if (flag2)
          light.length = num2 + resizeData.offset;
      }
      foreach (Transform otherTransform in resizeData.otherTransforms)
      {
        if (flag1)
        {
          Vector3 localPosition = otherTransform.localPosition;
          otherTransform.localPosition = new Vector3(localPosition.x, localPosition.y, num1 + resizeData.offset);
        }
      }
    }
    this._isResizingFinished = true;
    System.Action resizingDidFinishEvent = this.resizingDidFinishEvent;
    if (resizingDidFinishEvent == null)
      return;
    resizingDidFinishEvent();
  }

  [Flags]
  public enum ResizeType
  {
    None = 0,
    Position = 1,
    Length = 2,
  }

  [Serializable]
  public class ResizeData
  {
    [SerializeField]
    protected MultiplayerEnvironmentResizeController.ResizeType _resizeType;
    [SerializeField]
    protected float _offset;
    [SerializeField]
    [NullAllowed]
    protected TubeBloomPrePassLight[] _lights;
    [SerializeField]
    [NullAllowed]
    protected Transform[] _otherTransforms;

    public MultiplayerEnvironmentResizeController.ResizeType resizeType => this._resizeType;

    public float offset => this._offset;

    public TubeBloomPrePassLight[] lights => this._lights;

    public Transform[] otherTransforms => this._otherTransforms;
  }
}
