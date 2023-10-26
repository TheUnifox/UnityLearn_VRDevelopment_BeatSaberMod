// Decompiled with JetBrains decompiler
// Type: HMUI.VerticalScrollIndicator
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

using UnityEngine;

namespace HMUI
{
  public class VerticalScrollIndicator : MonoBehaviour
  {
    [SerializeField]
    protected RectTransform _handle;
    [SerializeField]
    protected AnimationClip _normalAnimationClip;
    [SerializeField]
    protected AnimationClip _disabledAnimationClip;
    [SerializeField]
    protected float _padding;
    protected float _progress;
    protected float _normalizedPageHeight = 1f;

    public float progress
    {
      set
      {
        if ((double) this._progress == (double) value)
          return;
        this._progress = Mathf.Clamp01(value);
        this.RefreshHandle();
      }
      get => this._progress;
    }

    public float normalizedPageHeight
    {
      set
      {
        if ((double) this._normalizedPageHeight == (double) value)
          return;
        this._normalizedPageHeight = Mathf.Clamp01(value);
        this.RefreshHandle();
      }
      get => this._normalizedPageHeight;
    }

    public bool disabled
    {
      set => (value ? this._disabledAnimationClip : this._normalAnimationClip).SampleAnimation(this.gameObject, 0.0f);
    }

    public virtual void OnEnable() => this.RefreshHandle();

    public virtual void RefreshHandle()
    {
      float num = ((RectTransform) this.transform).rect.size.y - 2f * this._padding;
      this._handle.sizeDelta = new Vector2(0.0f, this._normalizedPageHeight * num);
      this._handle.anchoredPosition = new Vector2(0.0f, (float) (-(double) this._progress * (1.0 - (double) this._normalizedPageHeight)) * num - this._padding);
    }
  }
}
