// Decompiled with JetBrains decompiler
// Type: CustomTweenBehaviour
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;

[Serializable]
public class CustomTweenBehaviour : PlayableBehaviour
{
  [HideInInspector]
  [NullAllowed]
  public Transform[] _transforms;
  public bool startLocationCurrentPosition;
  [DrawIf("startLocationCurrentPosition", false, DrawIfAttribute.DisablingType.DontDraw)]
  public Vector3 startLocalPos;
  public Vector3 endLocalPos;
  public EaseType easeType;
  public bool endXRelativeToSelfRotation;
  [HideInInspector]
  public float elementDelay;
  [Space]
  public bool _lockX;
  public bool _lockY;
  public bool _lockZ;
  public bool _randomizeOrder;
  public float _randomizedMinDurationMultiplier = 1f;
  protected float _duration = 1f;
  protected float _perItemDuration = 1f;
  protected Vector3[] _originalLocalPos;
  protected bool _initialized;
  protected int[] _randomizedOrder;
  protected float[] _randomizedDuration;

  public override void OnGraphStart(Playable playable)
  {
    this._duration = (float) playable.GetDuration<Playable>() * 0.98f;
    this._perItemDuration = this._transforms.Length < 2 ? this._duration : this._duration - (float) (this._transforms.Length - 1) * this.elementDelay;
    this._originalLocalPos = new Vector3[this._transforms.Length];
    for (int index = 0; index < this._transforms.Length; ++index)
      this._originalLocalPos[index] = this._transforms[index].localPosition;
    this._initialized = true;
  }

  public override void ProcessFrame(Playable playable, FrameData info, object playerData)
  {
    ((ScriptPlayable<CustomTweenBehaviour>) playable).GetBehaviour();
    float time = (float) playable.GetTime<Playable>();
    if (this.startLocationCurrentPosition && !this._initialized)
    {
      this._originalLocalPos = new Vector3[this._transforms.Length];
      for (int index = 0; index < this._transforms.Length; ++index)
        this._originalLocalPos[index] = this._transforms[index].localPosition;
      this._initialized = true;
    }
    for (int index = 0; index < this._transforms.Length; ++index)
    {
      Transform transform = this._transforms[index];
      Vector3 vector3_1 = this.startLocalPos;
      if (this.startLocationCurrentPosition)
        vector3_1 = this._originalLocalPos[index];
      Vector3 vector3_2 = this.endLocalPos;
      if (this.endXRelativeToSelfRotation)
        vector3_2 = new Vector3((transform.localRotation * vector3_2).x, vector3_2.y, vector3_2.z);
      Vector3 vector3_3;
      if ((double) this.elementDelay <= 0.0)
      {
        vector3_3 = vector3_1 + (vector3_2 - vector3_1) * Interpolation.Interpolate(Mathf.Clamp01(time / this._duration), this.easeType);
      }
      else
      {
        float num1 = this.elementDelay * (this._randomizeOrder ? (float) this._randomizedOrder[index] : (float) index);
        float num2 = (double) this._randomizedMinDurationMultiplier < 1.0 ? this._randomizedDuration[index] : this._perItemDuration;
        vector3_3 = vector3_1 + (vector3_2 - vector3_1) * Interpolation.Interpolate(Mathf.Clamp01((time - num1) / num2), this.easeType);
      }
      if (this._lockX)
        vector3_3.x = transform.localPosition.x;
      if (this._lockY)
        vector3_3.y = transform.localPosition.y;
      if (this._lockZ)
        vector3_3.z = transform.localPosition.z;
      transform.localPosition = vector3_3;
    }
  }

  public override void OnPlayableDestroy(Playable playable)
  {
    if (!this._initialized)
      return;
    for (int index = 0; index < this._transforms.Length; ++index)
    {
      if ((UnityEngine.Object) this._transforms[index] != (UnityEngine.Object) null)
        this._transforms[index].localPosition = this._originalLocalPos[index];
    }
  }

  public override void OnBehaviourPlay(Playable playable, FrameData info)
  {
    if (this._randomizeOrder)
      this._randomizedOrder = Enumerable.Range(0, this._transforms.Length).OrderBy<int, float>((Func<int, float>) (i => UnityEngine.Random.value)).ToArray<int>();
    this._randomizedDuration = new float[this._transforms.Length];
    if ((double) this._randomizedMinDurationMultiplier >= 1.0)
      return;
    for (int index = 0; index < this._transforms.Length; ++index)
      this._randomizedDuration[index] = UnityEngine.Random.Range(this._randomizedMinDurationMultiplier, 1f) * this._perItemDuration;
  }
}
