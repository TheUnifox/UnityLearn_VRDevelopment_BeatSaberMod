// Decompiled with JetBrains decompiler
// Type: ObstacleSaberSparkleEffectManager
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Libraries.HM.HMLib.VR;
using UnityEngine;
using Zenject;

public class ObstacleSaberSparkleEffectManager : MonoBehaviour
{
  [SerializeField]
  protected ObstacleSaberSparkleEffect _obstacleSaberSparkleEffectPrefab;
  [SerializeField]
  protected HapticPresetSO _rumblePreset;
  [Inject]
  protected readonly BeatmapObjectManager _beatmapObjectManager;
  [Inject]
  protected readonly SaberManager _saberManager;
  [Inject]
  protected readonly HapticFeedbackController _hapticFeedbackController;
  [Inject]
  protected readonly ColorManager _colorManager;
  protected Saber[] _sabers;
  protected ObstacleSaberSparkleEffect[] _effects;
  protected Transform[] _effectsTransforms;
  protected bool[] _isSystemActive;
  protected bool[] _wasSystemActive;
  protected Vector3[] _burnMarkPositions;

  public event System.Action<SaberType> sparkleEffectDidStartEvent;

  public event System.Action<SaberType> sparkleEffectDidEndEvent;

  public virtual void Start()
  {
    this._sabers = new Saber[2];
    this._sabers[0] = this._saberManager.leftSaber;
    this._sabers[1] = this._saberManager.rightSaber;
    this._effects = new ObstacleSaberSparkleEffect[2];
    this._effectsTransforms = new Transform[2];
    for (int index = 0; index < 2; ++index)
    {
      this._effects[index] = UnityEngine.Object.Instantiate<ObstacleSaberSparkleEffect>(this._obstacleSaberSparkleEffectPrefab);
      this._effects[index].color = this._colorManager.GetObstacleEffectColor();
      this._effectsTransforms[index] = this._effects[index].transform;
    }
    this._burnMarkPositions = new Vector3[2];
    this._isSystemActive = new bool[2];
    this._wasSystemActive = new bool[2];
  }

  public virtual void OnDisable()
  {
    if ((UnityEngine.Object) this._hapticFeedbackController == (UnityEngine.Object) null)
      return;
    for (int index = 0; index < 2; ++index)
    {
      if (this._isSystemActive[index])
        this._isSystemActive[index] = false;
    }
  }

  public virtual void Update()
  {
    this._wasSystemActive[0] = this._isSystemActive[0];
    this._wasSystemActive[1] = this._isSystemActive[1];
    this._isSystemActive[0] = false;
    this._isSystemActive[1] = false;
    foreach (ObstacleController obstacleController in this._beatmapObjectManager.activeObstacleControllers)
    {
      Bounds bounds = obstacleController.bounds;
      for (int index = 0; index < 2; ++index)
      {
        Vector3 burnMarkPos;
        if (this._sabers[index].isActiveAndEnabled && this.GetBurnMarkPos(bounds, obstacleController.transform, this._sabers[index].saberBladeBottomPos, this._sabers[index].saberBladeTopPos, out burnMarkPos))
        {
          this._isSystemActive[index] = true;
          this._burnMarkPositions[index] = burnMarkPos;
          this._effects[index].SetPositionAndRotation(burnMarkPos, this.GetEffectRotation(burnMarkPos, obstacleController.transform, bounds));
          this._hapticFeedbackController.PlayHapticFeedback(this._sabers[index].saberType.Node(), this._rumblePreset);
          if (!this._wasSystemActive[index])
          {
            this._effects[index].StartEmission();
            System.Action<SaberType> effectDidStartEvent = this.sparkleEffectDidStartEvent;
            if (effectDidStartEvent != null)
              effectDidStartEvent(this._sabers[index].saberType);
          }
        }
      }
    }
    for (int index = 0; index < 2; ++index)
    {
      if (!this._isSystemActive[index] && this._wasSystemActive[index])
      {
        this._effects[index].StopEmission();
        System.Action<SaberType> effectDidEndEvent = this.sparkleEffectDidEndEvent;
        if (effectDidEndEvent != null)
          effectDidEndEvent(this._sabers[index].saberType);
      }
    }
  }

  public virtual Quaternion GetEffectRotation(Vector3 pos, Transform transform, Bounds bounds)
  {
    pos = transform.InverseTransformPoint(pos);
    Vector3 direction = (double) pos.x < (double) bounds.max.x - 0.0099999997764825821 ? ((double) pos.x > (double) bounds.min.x + 0.0099999997764825821 ? ((double) pos.y < (double) bounds.max.y - 0.0099999997764825821 ? ((double) pos.y > (double) bounds.min.y + 0.0099999997764825821 ? new Vector3(180f, 0.0f, 0.0f) : new Vector3(90f, 0.0f, 0.0f)) : new Vector3(-90f, 0.0f, 0.0f)) : new Vector3(0.0f, -90f, 0.0f)) : new Vector3(0.0f, 90f, 0.0f);
    return Quaternion.Euler(transform.TransformDirection(direction));
  }

  public virtual Vector3 BurnMarkPosForSaberType(SaberType saberType) => saberType == this._sabers[0].saberType ? this._burnMarkPositions[0] : this._burnMarkPositions[1];

  public virtual bool GetBurnMarkPos(
    Bounds bounds,
    Transform transform,
    Vector3 bladeBottomPos,
    Vector3 bladeTopPos,
    out Vector3 burnMarkPos)
  {
    bladeBottomPos = transform.InverseTransformPoint(bladeBottomPos);
    bladeTopPos = transform.InverseTransformPoint(bladeTopPos);
    float num = Vector3.Distance(bladeBottomPos, bladeTopPos);
    Vector3 direction = bladeTopPos - bladeBottomPos;
    direction.Normalize();
    float distance;
    if (bounds.IntersectRay(new Ray(bladeBottomPos, direction), out distance) && (double) distance <= (double) num)
    {
      burnMarkPos = transform.TransformPoint(bladeBottomPos + direction * distance);
      return true;
    }
    burnMarkPos = Vector3.zero;
    return false;
  }
}
