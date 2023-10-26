// Decompiled with JetBrains decompiler
// Type: HMUI.PanelAnimationSO
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

using System;
using UnityEngine;

namespace HMUI
{
  public class PanelAnimationSO : ScriptableObject
  {
    [SerializeField]
    protected float _duration = 0.3f;
    [SerializeField]
    protected AnimationCurve _scaleXAnimationCurve = AnimationCurve.EaseInOut(0.0f, 0.0f, 0.0f, 0.0f);
    [SerializeField]
    protected AnimationCurve _scaleYAnimationCurve = AnimationCurve.EaseInOut(0.0f, 0.0f, 0.0f, 0.0f);
    [SerializeField]
    protected AnimationCurve _alphaAnimationCurve = AnimationCurve.EaseInOut(0.0f, 0.0f, 0.0f, 0.0f);
    [SerializeField]
    protected AnimationCurve _parentAlphaAnimationCurve = AnimationCurve.EaseInOut(0.0f, 0.0f, 0.0f, 0.0f);

    public virtual void ExecuteAnimation(GameObject go) => this.ExecuteAnimation(go, (CanvasGroup) null, false, (Action) null);

    public virtual void ExecuteAnimation(GameObject go, Action finishedCallback) => this.ExecuteAnimation(go, (CanvasGroup) null, false, finishedCallback);

    public virtual void ExecuteAnimation(
      GameObject go,
      CanvasGroup parentCanvasGroup,
      Action finishedCallback)
    {
      this.ExecuteAnimation(go, parentCanvasGroup, false, finishedCallback);
    }

    public virtual void ExecuteAnimation(
      GameObject go,
      CanvasGroup parentCanvasGroup,
      bool instant,
      Action finishedCallback)
    {
      CanvasGroup orAddComponent = EssentialHelpers.GetOrAddComponent<CanvasGroup>(go);
      EssentialHelpers.GetOrAddComponent<PanelAnimation>(go).StartAnimation(orAddComponent, parentCanvasGroup, instant ? 0.0f : this._duration, this._scaleXAnimationCurve, this._scaleYAnimationCurve, this._alphaAnimationCurve, this._parentAlphaAnimationCurve, finishedCallback);
    }
  }
}
