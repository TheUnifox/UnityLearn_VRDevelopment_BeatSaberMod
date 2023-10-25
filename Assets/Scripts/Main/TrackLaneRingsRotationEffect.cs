// Decompiled with JetBrains decompiler
// Type: TrackLaneRingsRotationEffect
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using UnityEngine;

public class TrackLaneRingsRotationEffect : MonoBehaviour
{
  [SerializeField]
  protected TrackLaneRingsManager _trackLaneRingsManager;
  [Header("Startup buildup")]
  [SerializeField]
  protected float _startupRotationAngle;
  [SerializeField]
  protected float _startupRotationStep = 10f;
  [SerializeField]
  protected int _startupRotationPropagationSpeed = 1;
  [SerializeField]
  protected float _startupRotationFlexySpeed = 0.5f;
  protected List<TrackLaneRingsRotationEffect.RingRotationEffect> _activeRingRotationEffects;
  protected List<TrackLaneRingsRotationEffect.RingRotationEffect> _ringRotationEffectsPool;
  protected List<int> ringRotationEffectsToDelete = new List<int>();

  public virtual void Awake()
  {
    this._activeRingRotationEffects = new List<TrackLaneRingsRotationEffect.RingRotationEffect>(20);
    this._ringRotationEffectsPool = new List<TrackLaneRingsRotationEffect.RingRotationEffect>(20);
    for (int index = 0; index < this._ringRotationEffectsPool.Capacity; ++index)
      this._ringRotationEffectsPool.Add(new TrackLaneRingsRotationEffect.RingRotationEffect());
  }

  public virtual void Start() => this.AddRingRotationEffect(this._startupRotationAngle, this._startupRotationStep, this._startupRotationPropagationSpeed, this._startupRotationFlexySpeed);

  public virtual void FixedUpdate()
  {
    TrackLaneRing[] rings = this._trackLaneRingsManager.Rings;
    for (int index = this._activeRingRotationEffects.Count - 1; index >= 0; --index)
    {
      TrackLaneRingsRotationEffect.RingRotationEffect ringRotationEffect = this._activeRingRotationEffects[index];
      for (int progressPos = ringRotationEffect.progressPos; progressPos < ringRotationEffect.progressPos + ringRotationEffect.rotationPropagationSpeed && progressPos < rings.Length; ++progressPos)
        rings[progressPos].SetDestRotation(ringRotationEffect.rotationAngle + (float) progressPos * ringRotationEffect.rotationStep, ringRotationEffect.rotationFlexySpeed);
      ringRotationEffect.progressPos += ringRotationEffect.rotationPropagationSpeed;
      if (ringRotationEffect.progressPos >= rings.Length)
      {
        this.RecycleRingRotationEffect(this._activeRingRotationEffects[index]);
        this._activeRingRotationEffects.RemoveAt(index);
      }
    }
  }

  public virtual void AddRingRotationEffect(
    float angle,
    float step,
    int propagationSpeed,
    float flexySpeed)
  {
    TrackLaneRingsRotationEffect.RingRotationEffect ringRotationEffect = this.SpawnRingRotationEffect();
    ringRotationEffect.progressPos = 0;
    ringRotationEffect.rotationAngle = angle;
    ringRotationEffect.rotationStep = step;
    ringRotationEffect.rotationPropagationSpeed = propagationSpeed;
    ringRotationEffect.rotationFlexySpeed = flexySpeed;
    this._activeRingRotationEffects.Add(ringRotationEffect);
  }

  public virtual float GetFirstRingRotationAngle() => this._trackLaneRingsManager.Rings[0].GetRotation();

  public virtual float GetFirstRingDestinationRotationAngle() => this._trackLaneRingsManager.Rings[0].GetDestinationRotation();

  public virtual TrackLaneRingsRotationEffect.RingRotationEffect SpawnRingRotationEffect()
  {
    TrackLaneRingsRotationEffect.RingRotationEffect ringRotationEffect;
    if (this._ringRotationEffectsPool.Count > 0)
    {
      ringRotationEffect = this._ringRotationEffectsPool[0];
      this._ringRotationEffectsPool.RemoveAt(0);
    }
    else
      ringRotationEffect = new TrackLaneRingsRotationEffect.RingRotationEffect();
    return ringRotationEffect;
  }

  public virtual void RecycleRingRotationEffect(
    TrackLaneRingsRotationEffect.RingRotationEffect ringRotationEffect)
  {
    this._ringRotationEffectsPool.Add(ringRotationEffect);
  }

  public class RingRotationEffect
  {
    public float rotationAngle;
    public float rotationStep;
    public float rotationFlexySpeed;
    public int rotationPropagationSpeed;
    public int progressPos;
  }
}
