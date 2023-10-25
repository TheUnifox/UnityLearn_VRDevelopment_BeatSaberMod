// Decompiled with JetBrains decompiler
// Type: RotateBySpawnRotation
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class RotateBySpawnRotation : MonoBehaviour
{
  [SerializeField]
  protected float _aheadTime;
  [SerializeField]
  protected float _smooth;
  [Inject]
  protected readonly BeatmapCallbacksController _beatmapCallbacksController;
  protected BeatmapDataCallbackWrapper _beatmapDataCallbackWrapper;
  protected float _currentRotation;
  protected float _prevRotation;
  protected float _targetRotation;

  public virtual void Start()
  {
    this.transform.rotation = Quaternion.identity;
    this._beatmapDataCallbackWrapper = this._beatmapCallbacksController.AddBeatmapCallback<SpawnRotationBeatmapEventData>(this._aheadTime, new BeatmapDataCallback<SpawnRotationBeatmapEventData>(this.HandleSpawnRotationBeatmapEvent));
    if ((double) this._smooth != 0.0)
      return;
    this.enabled = false;
  }

  public virtual void OnDestroy()
  {
    if (this._beatmapCallbacksController == null)
      return;
    this._beatmapCallbacksController.RemoveBeatmapCallback(this._beatmapDataCallbackWrapper);
  }

  public virtual void HandleSpawnRotationBeatmapEvent(SpawnRotationBeatmapEventData beatmapEventData)
  {
    this._targetRotation = beatmapEventData.rotation;
    if ((double) this._smooth != 0.0)
      return;
    this.transform.rotation = Quaternion.Euler(0.0f, this._targetRotation, 0.0f);
  }

  public virtual void FixedUpdate()
  {
    this._prevRotation = this._currentRotation;
    this._currentRotation = Mathf.Lerp(this._currentRotation, this._targetRotation, TimeHelper.fixedDeltaTime * this._smooth);
  }

  public virtual void LateUpdate() => this.transform.rotation = Quaternion.Euler(0.0f, Mathf.LerpUnclamped(this._prevRotation, this._currentRotation, TimeHelper.interpolationFactor), 0.0f);
}
