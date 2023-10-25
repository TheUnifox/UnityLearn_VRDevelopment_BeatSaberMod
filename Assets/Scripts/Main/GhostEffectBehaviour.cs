// Decompiled with JetBrains decompiler
// Type: GhostEffectBehaviour
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

[Serializable]
public class GhostEffectBehaviour : PlayableBehaviour
{
  public AnimationCurve alphaCurve = AnimationCurve.EaseInOut(0.0f, 0.0f, 1f, 1f);
  public AnimationCurve sizeCurve = AnimationCurve.EaseInOut(0.0f, 0.0f, 1f, 1f);
  public AnimationCurve distanceCurve = AnimationCurve.EaseInOut(0.0f, 0.0f, 1f, 1f);
  public Vector3 _distanceMultiplier = 10f * Vector3.one;
  [Space]
  [Tooltip("Make sure Array Reference is on the GhostEffect Object (this uses its parent for local space calculations")]
  public bool _useStartTransform;
  [Tooltip("Make sure Array Reference is on the GhostEffect Object (this uses its parent for local space calculations")]
  public bool _useEndTransform;
  [Space]
  [DrawIf("_useStartTransform", false, DrawIfAttribute.DisablingType.DontDraw)]
  public Vector3 _startLocalPosition;
  [DrawIf("_useStartTransform", true, DrawIfAttribute.DisablingType.DontDraw)]
  [NullAllowed]
  public Transform _startTransform;
  [DrawIf("_useEndTransform", false, DrawIfAttribute.DisablingType.DontDraw)]
  public Vector3 _endLocalPosition;
  [DrawIf("_useEndTransform", true, DrawIfAttribute.DisablingType.DontDraw)]
  [NullAllowed]
  public Transform _endTransform;
  public EaseType _positionEasing;
  [Space]
  public GhostEffectBehaviour.EndBehavior _endBehavior;
  public float progress;
  [HideInInspector]
  [NullAllowed]
  public TextMeshPro[] textMeshPros;
  [HideInInspector]
  [NullAllowed]
  public CanvasGroup[] _canvasGroups;
  [HideInInspector]
  public GhostEffectBehaviour.GhostEffectType _ghostEffectType;
  [HideInInspector]
  [NullAllowed]
  public Transform _ghostEffectTransform;
  protected Vector3 _direction;
  protected bool _finished;

  public override void OnBehaviourPlay(Playable playable, FrameData info)
  {
    this._direction = Vector3.Normalize(this._startLocalPosition - this._endLocalPosition);
    this._finished = false;
    this.EnableObjects(true);
    if (this._useStartTransform)
      this._startLocalPosition = this._ghostEffectTransform.parent.InverseTransformPoint(this._startTransform.position);
    if (!this._useEndTransform)
      return;
    this._endLocalPosition = this._ghostEffectTransform.parent.InverseTransformPoint(this._endTransform.position);
  }

  public override void ProcessFrame(Playable playable, FrameData info, object playerData)
  {
    GhostEffectBehaviour behaviour = ((ScriptPlayable<GhostEffectBehaviour>) playable).GetBehaviour();
    switch (this._ghostEffectType)
    {
      case GhostEffectBehaviour.GhostEffectType.TextMeshPro:
        if (this.textMeshPros.Length < 1)
          return;
        break;
      case GhostEffectBehaviour.GhostEffectType.Canvas:
        if (this._canvasGroups.Length < 1)
          return;
        break;
    }
    if ((double) behaviour.progress >= 1.0)
    {
      this._finished = true;
      this.EnableObjects(false);
    }
    else
    {
      if (this._finished)
      {
        this._finished = false;
        this.EnableObjects(true);
      }
      this._ghostEffectTransform.localPosition = this._startLocalPosition + (this._endLocalPosition - this._startLocalPosition) * Interpolation.Interpolate(behaviour.progress, this._positionEasing);
      float num1 = this.alphaCurve.Evaluate(behaviour.progress);
      float num2 = this.sizeCurve.Evaluate(behaviour.progress);
      Vector3 b = this.distanceCurve.Evaluate(behaviour.progress) * this._distanceMultiplier;
      if (this._ghostEffectType == GhostEffectBehaviour.GhostEffectType.TextMeshPro)
      {
        for (int index = 0; index < this.textMeshPros.Length; ++index)
        {
          this.textMeshPros[index].alpha = num1;
          this.textMeshPros[index].transform.localScale = Vector3.one * num2;
          this.textMeshPros[index].transform.localPosition = Vector3.Scale(this._direction, b) * (float) index;
        }
      }
      else
      {
        for (int index = 0; index < this._canvasGroups.Length; ++index)
        {
          this._canvasGroups[index].alpha = num1;
          this._canvasGroups[index].transform.localScale = Vector3.one * num2;
          this._canvasGroups[index].transform.localPosition = Vector3.Scale(this._direction, b) * (float) index;
        }
      }
    }
  }

  public virtual void EnableObjects(bool on)
  {
    if (this._endBehavior == GhostEffectBehaviour.EndBehavior.Nothing && !on)
      return;
    if (this._ghostEffectType == GhostEffectBehaviour.GhostEffectType.TextMeshPro)
    {
      for (int index = 0; index < this.textMeshPros.Length; ++index)
      {
        if (index != 0 || this._endBehavior != GhostEffectBehaviour.EndBehavior.DisableCopies || on)
          this.textMeshPros[index].gameObject.SetActive(on);
      }
    }
    else
    {
      for (int index = 0; index < this._canvasGroups.Length; ++index)
      {
        if (index != 0 || this._endBehavior != GhostEffectBehaviour.EndBehavior.DisableCopies || on)
          this._canvasGroups[index].gameObject.SetActive(on);
      }
    }
  }

  public enum EndBehavior
  {
    DisableAll,
    DisableCopies,
    Nothing,
  }

  public enum GhostEffectType
  {
    TextMeshPro,
    Canvas,
  }
}
