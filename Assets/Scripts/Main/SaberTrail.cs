// Decompiled with JetBrains decompiler
// Type: SaberTrail
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using UnityEngine;

public class SaberTrail : MonoBehaviour
{
  [SerializeField]
  protected SaberTrailRenderer _trailRendererPrefab;
  [Header("Trail Settings")]
  [Tooltip("Age of most distant segment of trail")]
  [SerializeField]
  protected float _trailDuration = 0.4f;
  [Tooltip("Saber position snapshots taken per second")]
  [SerializeField]
  protected int _samplingFrequency = 50;
  [Tooltip("Segments count in final trail mesh")]
  [SerializeField]
  protected int _granularity = 60;
  [Range(0.0f, 1f)]
  [Tooltip("Duration of most distant segment of trail section that gradients from white to movementData color")]
  [SerializeField]
  protected float _whiteSectionMaxDuration = 0.1f;
  [SerializeField]
  protected bool _colorOverwrite;
  [SerializeField]
  [DrawIf("_colorOverwrite", true, DrawIfAttribute.DisablingType.DontDraw)]
  protected Color _forcedColor = Color.cyan;
  protected const int kIgnoredFramesCount = 4;
  protected const int kSnapshotCapacityMargin = 3;
  protected const int kScaleCheckFramesInterval = 10;
  protected Color _color;
  protected IBladeMovementData _movementData;
  protected float _lastTrailElementTime;
  protected SaberTrailRenderer _trailRenderer;
  protected TrailElementCollection _trailElementCollection;
  protected float _sampleStep;
  protected int _framesPassed;
  protected float _lastZScale;
  protected int _framesToScaleCheck;
  protected bool _inited;

  public virtual void Awake() => this._trailRenderer = UnityEngine.Object.Instantiate<SaberTrailRenderer>(this._trailRendererPrefab, Vector3.zero, Quaternion.identity);

  public virtual void Setup(Color color, IBladeMovementData movementData)
  {
    this._color = this._colorOverwrite ? this._forcedColor : color;
    this._movementData = movementData;
  }

  public virtual void Init()
  {
    this._sampleStep = 1f / (float) this._samplingFrequency;
    BladeMovementDataElement lastAddedData = this._movementData.lastAddedData;
    Vector3 bottomPos = lastAddedData.bottomPos;
    Vector3 topPos = lastAddedData.topPos;
    this._lastTrailElementTime = lastAddedData.time;
    this._trailElementCollection = new TrailElementCollection(Mathf.CeilToInt((float) this._samplingFrequency * this._trailDuration) + 3, bottomPos, topPos, this._lastTrailElementTime);
    float trailWidth = this.GetTrailWidth(lastAddedData);
    this._whiteSectionMaxDuration = Math.Min(this._whiteSectionMaxDuration, this._trailDuration);
    this._lastZScale = this.transform.lossyScale.z;
    this._trailRenderer.Init(trailWidth, this._trailDuration, this._granularity, this._whiteSectionMaxDuration);
    this._inited = true;
  }

  public virtual void ResetTrailData()
  {
    BladeMovementDataElement lastAddedData = this._movementData.lastAddedData;
    Vector3 bottomPos = lastAddedData.bottomPos;
    Vector3 topPos = lastAddedData.topPos;
    this._lastTrailElementTime = lastAddedData.time;
    this._trailElementCollection.InitSnapshots(bottomPos, topPos, this._lastTrailElementTime);
  }

  public virtual void LateUpdate()
  {
    if (this._framesPassed <= 4)
    {
      if (this._framesPassed == 4)
        this.Init();
      ++this._framesPassed;
    }
    else
    {
      BladeMovementDataElement lastAddedData = this._movementData.lastAddedData;
      BladeMovementDataElement prevAddedData = this._movementData.prevAddedData;
      --this._framesToScaleCheck;
      if (this._framesToScaleCheck <= 0)
      {
        this._framesToScaleCheck = 10;
        if (!Mathf.Approximately(this.transform.lossyScale.z, this._lastZScale))
        {
          this._lastZScale = this.transform.lossyScale.z;
          this._trailRenderer.SetTrailWidth(this.GetTrailWidth(lastAddedData));
        }
      }
      int num = (int) Mathf.Floor((lastAddedData.time - this._lastTrailElementTime) / this._sampleStep);
      for (int index = 0; index < num; ++index)
      {
        this._lastTrailElementTime += this._sampleStep;
        float t = (float) (((double) lastAddedData.time - (double) this._lastTrailElementTime) / ((double) lastAddedData.time - (double) prevAddedData.time));
        this._trailElementCollection.SetHeadData(Vector3.LerpUnclamped(lastAddedData.bottomPos, prevAddedData.bottomPos, t), Vector3.LerpUnclamped(lastAddedData.topPos, prevAddedData.topPos, t), this._lastTrailElementTime);
        this._trailElementCollection.MoveTailToHead();
      }
      this._trailElementCollection.SetHeadData(lastAddedData.bottomPos, lastAddedData.topPos, lastAddedData.time);
      this._trailElementCollection.UpdateDistances();
      this._trailRenderer.UpdateMesh(this._trailElementCollection, this._color);
    }
  }

  public virtual void OnEnable()
  {
    if (this._inited)
    {
      this.ResetTrailData();
      this._trailRenderer.UpdateMesh(this._trailElementCollection, this._color);
    }
    if (!(bool) (UnityEngine.Object) this._trailRenderer)
      return;
    this._trailRenderer.enabled = true;
  }

  public virtual void OnDisable()
  {
    if (!(bool) (UnityEngine.Object) this._trailRenderer)
      return;
    this._trailRenderer.enabled = false;
  }

  public virtual void OnDestroy()
  {
    if (!((UnityEngine.Object) this._trailRenderer != (UnityEngine.Object) null))
      return;
    UnityEngine.Object.Destroy((UnityEngine.Object) this._trailRenderer.gameObject);
  }

  public virtual float GetTrailWidth(BladeMovementDataElement lastAddedData) => (lastAddedData.bottomPos - lastAddedData.topPos).magnitude;

  public virtual void OnDrawGizmosSelected()
  {
    if (this._movementData == null)
      return;
    BladeMovementDataElement lastAddedData = this._movementData.lastAddedData;
    Vector3 bottomPos = lastAddedData.bottomPos;
    Vector3 topPos = lastAddedData.topPos;
    float magnitude = (bottomPos - topPos).magnitude;
    if ((double) magnitude < (double) Mathf.Epsilon)
      return;
    Gizmos.color = Color.red;
    Gizmos.DrawSphere(bottomPos, magnitude * 0.04f);
    Gizmos.color = Color.blue;
    Gizmos.DrawSphere(topPos, magnitude * 0.04f);
  }
}
