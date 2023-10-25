// Decompiled with JetBrains decompiler
// Type: TweenPosition
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using System;
using System.Collections;
using UnityEngine;

public class TweenPosition : MonoBehaviour
{
  public bool _unscaledTime;
  public bool _localPosition;
  public float _duration = 1f;
  public AnimationCurve _animationCurve = AnimationCurve.EaseInOut(0.0f, 0.0f, 1f, 1f);
  protected Transform _transform;
  protected Vector3 _targetPos;

  public Vector3 TargetPos
  {
    set
    {
      if (this.TargetPos == value)
        return;
      this._targetPos = value;
      this.AnimateToNewPos(this._targetPos);
    }
    get => this._targetPos;
  }

  public virtual void Awake() => this._transform = this.transform;

  public virtual void AnimateToNewPos(Vector3 pos) => this.StartUniqueCoroutine<Vector3>(new Func<Vector3, IEnumerator>(this.AnimateToNewPosCoroutine), pos);

  public virtual IEnumerator AnimateToNewPosCoroutine(Vector3 pos)
  {
    Vector3 startPos = this._localPosition ? this._transform.localPosition : this._transform.position;
    float elapsedTime = 0.0f;
    while ((double) elapsedTime < (double) this._duration)
    {
      if (this._localPosition)
        this._transform.localPosition = Vector3.Lerp(startPos, this._targetPos, this._animationCurve.Evaluate(elapsedTime / this._duration));
      else
        this._transform.position = Vector3.Lerp(startPos, this._targetPos, this._animationCurve.Evaluate(elapsedTime / this._duration));
      elapsedTime += this._unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
      yield return (object) null;
    }
    if (this._localPosition)
      this._transform.localPosition = this._targetPos;
    else
      this._transform.position = this._targetPos;
  }
}
