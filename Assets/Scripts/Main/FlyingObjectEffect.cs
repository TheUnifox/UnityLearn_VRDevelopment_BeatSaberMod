// Decompiled with JetBrains decompiler
// Type: FlyingObjectEffect
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public abstract class FlyingObjectEffect : MonoBehaviour
{
  [SerializeField]
  private AnimationCurve _moveAnimationCurve = AnimationCurve.Linear(0.0f, 0.0f, 1f, 1f);
  [SerializeField]
  private float _shakeFrequency = 1f;
  [SerializeField]
  private float _shakeStrength = 20f;
  [SerializeField]
  private AnimationCurve _shakeStrengthAnimationCurve = AnimationCurve.Linear(0.0f, 1f, 1f, 0.0f);
  private bool _initialized;
  private Quaternion _shakeRotation;
  private Quaternion _rotation;
  private float _elapsedTime;
  private Vector3 _startPos;
  private Vector3 _targetPos;
  private float _duration;
  private bool _shake;
  private readonly LazyCopyHashSet<IFlyingObjectEffectDidFinishEvent> _didFinishEvent = new LazyCopyHashSet<IFlyingObjectEffectDidFinishEvent>();

  public ILazyCopyHashSet<IFlyingObjectEffectDidFinishEvent> didFinishEvent => (ILazyCopyHashSet<IFlyingObjectEffectDidFinishEvent>) this._didFinishEvent;

  public void InitAndPresent(float duration, Vector3 targetPos, Quaternion rotation, bool shake)
  {
    this._rotation = rotation;
    this.transform.localRotation = rotation;
    this._duration = duration;
    this._targetPos = targetPos;
    this._shake = shake;
    this._elapsedTime = 0.0f;
    this._startPos = this.transform.position;
    this._initialized = true;
    this.enabled = true;
    this.ManualUpdate(0.0f);
  }

  protected void Update()
  {
    if (!this._initialized)
      this.enabled = false;
    else if ((double) this._elapsedTime >= (double) this._duration)
    {
      foreach (IFlyingObjectEffectDidFinishEvent effectDidFinishEvent in this._didFinishEvent.items)
        effectDidFinishEvent.HandleFlyingObjectEffectDidFinish(this);
    }
    else
    {
      float num = this._elapsedTime / this._duration;
      this.ManualUpdate(num);
      this.transform.localPosition = Vector3.Lerp(this._startPos, this._targetPos, this._moveAnimationCurve.Evaluate(num));
      if (this._shake)
      {
        this._shakeRotation.eulerAngles = new Vector3(0.0f, 0.0f, Mathf.Sin(num * 3.14159274f * this._shakeFrequency) * this._shakeStrength * this._shakeStrengthAnimationCurve.Evaluate(num));
        this.transform.localRotation = this._rotation * this._shakeRotation;
      }
      this._elapsedTime += Time.deltaTime;
    }
  }

  protected abstract void ManualUpdate(float t);
}
