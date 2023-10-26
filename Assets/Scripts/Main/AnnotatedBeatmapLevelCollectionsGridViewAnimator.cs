// Decompiled with JetBrains decompiler
// Type: AnnotatedBeatmapLevelCollectionsGridViewAnimator
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Runtime.CompilerServices;
using Tweening;
using UnityEngine;
using Zenject;

public class AnnotatedBeatmapLevelCollectionsGridViewAnimator : MonoBehaviour
{
  [SerializeField]
  protected RectTransform _viewportTransform;
  [SerializeField]
  protected RectTransform _contentTransform;
  [Space]
  [SerializeField]
  protected float _transitionDuration = 0.4f;
  [SerializeField]
  protected EaseType _easeType = EaseType.InOutCubic;
  [Inject]
  protected readonly TimeTweeningManager _tweeningManager;
  protected float _rowHeight;
  protected int _rowCount;
  protected int _selectedRow;
  protected Vector2Tween _viewportSizeTween;
  protected Vector2Tween _contentPositionTween;

    public virtual void Init(float rowHeight, int rowCount)
    {
        this._rowHeight = rowHeight;
        this._rowCount = rowCount;
        this._selectedRow = 0;
        Vector2 sizeDelta = this._contentTransform.sizeDelta;
        sizeDelta.y = this._rowHeight * (float)this._rowCount;
        this._contentTransform.sizeDelta = sizeDelta;
        Vector2 anchoredPosition = this._contentTransform.anchoredPosition;
        anchoredPosition.y = this.GetContentYOffset();
        this._contentTransform.anchoredPosition = anchoredPosition;
        Vector2 sizeDelta2 = this._viewportTransform.sizeDelta;
        sizeDelta2.y = this._rowHeight;
        this._viewportTransform.sizeDelta = sizeDelta2;
    }

    public virtual void OnDestroy() => this.DespawnAllActiveTweens();

    public virtual void ScrollToRowIdxInstant(int selectedRow)
    {
        this._selectedRow = selectedRow;
        Vector2 anchoredPosition = this._contentTransform.anchoredPosition;
        anchoredPosition.y = this.GetContentYOffset();
        this._contentTransform.anchoredPosition = anchoredPosition;
    }

    public virtual void AnimateOpen(bool animated)
  {
    this.DespawnAllActiveTweens();
    Vector2 sizeDelta = this._viewportTransform.sizeDelta;
    Vector2 anchoredPosition = this._contentTransform.anchoredPosition;
    int num = (double) this._rowCount * 0.5 <= (double) this._selectedRow ? 1 : -1;
    float y = ((float) this._rowCount + Mathf.Abs((float) ((double) (this._rowCount - 1) * 0.5 - (double) this._selectedRow + 0.5)) * 2f + (float) num) * this._rowHeight;
    Vector2 p2_1 = new Vector2(sizeDelta.x, y);
    Vector2 p2_2 = new Vector2(anchoredPosition.x, this.GetContentYOffset());
    if (!animated)
    {
      this._viewportTransform.sizeDelta = p2_1;
      this._contentTransform.anchoredPosition = p2_2;
    }
    else
    {
      this._viewportSizeTween = Vector2Tween.Pool.Spawn(sizeDelta, p2_1, (System.Action<Vector2>) (size => this._viewportTransform.sizeDelta = size), this._transitionDuration, this._easeType, 0.0f);
      this._viewportSizeTween.onCompleted = (System.Action) (() =>
      {
        Vector2Tween.Pool.Despawn(this._viewportSizeTween);
        this._viewportSizeTween = (Vector2Tween) null;
      });
      this._contentPositionTween = Vector2Tween.Pool.Spawn(anchoredPosition, p2_2, (System.Action<Vector2>) (pos => this._contentTransform.anchoredPosition = pos), this._transitionDuration, this._easeType, 0.0f);
      this._contentPositionTween.onCompleted = (System.Action) (() =>
      {
        Vector2Tween.Pool.Despawn(this._contentPositionTween);
        this._contentPositionTween = (Vector2Tween) null;
      });
      this._tweeningManager.RestartTween((Tween) this._viewportSizeTween, (object) this);
      this._tweeningManager.RestartTween((Tween) this._contentPositionTween, (object) this);
    }
  }

  public virtual void AnimateClose(int selectedRow, bool animated)
  {
    this._selectedRow = selectedRow;
    this.DespawnAllActiveTweens();
    Vector2 sizeDelta = this._viewportTransform.sizeDelta;
    Vector2 anchoredPosition = this._contentTransform.anchoredPosition;
    Vector2 p2_1 = new Vector2(sizeDelta.x, this._rowHeight);
    Vector2 p2_2 = new Vector2(anchoredPosition.x, this.GetContentYOffset());
    if (!animated)
    {
      this._viewportTransform.sizeDelta = p2_1;
      this._contentTransform.anchoredPosition = p2_2;
    }
    else
    {
      this._viewportSizeTween = Vector2Tween.Pool.Spawn(sizeDelta, p2_1, (System.Action<Vector2>) (size => this._viewportTransform.sizeDelta = size), this._transitionDuration, this._easeType, 0.0f);
      this._viewportSizeTween.onCompleted = (System.Action) (() =>
      {
        Vector2Tween.Pool.Despawn(this._viewportSizeTween);
        this._viewportSizeTween = (Vector2Tween) null;
      });
      this._contentPositionTween = Vector2Tween.Pool.Spawn(anchoredPosition, p2_2, (System.Action<Vector2>) (pos => this._contentTransform.anchoredPosition = pos), this._transitionDuration, this._easeType, 0.0f);
      this._contentPositionTween.onCompleted = (System.Action) (() =>
      {
        Vector2Tween.Pool.Despawn(this._contentPositionTween);
        this._contentPositionTween = (Vector2Tween) null;
      });
      this._tweeningManager.RestartTween((Tween) this._viewportSizeTween, (object) this);
      this._tweeningManager.RestartTween((Tween) this._contentPositionTween, (object) this);
    }
  }

  public virtual void DespawnAllActiveTweens()
  {
    if (this._viewportSizeTween != null)
    {
      Vector2Tween.Pool.Despawn(this._viewportSizeTween);
      this._viewportSizeTween = (Vector2Tween) null;
    }
    if (this._contentPositionTween == null)
      return;
    Vector2Tween.Pool.Despawn(this._contentPositionTween);
    this._contentPositionTween = (Vector2Tween) null;
  }

  public virtual float GetContentYOffset() => ((float) this._selectedRow - (float) (this._rowCount - 1) * 0.5f) * this._rowHeight;

  [CompilerGenerated]
  public virtual void m_CAnimateOpenm_Eb__13_0(Vector2 size) => this._viewportTransform.sizeDelta = size;

  [CompilerGenerated]
  public virtual void m_CAnimateOpenm_Eb__13_1()
  {
    Vector2Tween.Pool.Despawn(this._viewportSizeTween);
    this._viewportSizeTween = (Vector2Tween) null;
  }

  [CompilerGenerated]
  public virtual void m_CAnimateOpenm_Eb__13_2(Vector2 pos) => this._contentTransform.anchoredPosition = pos;

  [CompilerGenerated]
  public virtual void m_CAnimateOpenm_Eb__13_3()
  {
    Vector2Tween.Pool.Despawn(this._contentPositionTween);
    this._contentPositionTween = (Vector2Tween) null;
  }

  [CompilerGenerated]
  public virtual void m_CAnimateClosem_Eb__14_0(Vector2 size) => this._viewportTransform.sizeDelta = size;

  [CompilerGenerated]
  public virtual void m_CAnimateClosem_Eb__14_1()
  {
    Vector2Tween.Pool.Despawn(this._viewportSizeTween);
    this._viewportSizeTween = (Vector2Tween) null;
  }

  [CompilerGenerated]
  public virtual void m_CAnimateClosem_Eb__14_2(Vector2 pos) => this._contentTransform.anchoredPosition = pos;

  [CompilerGenerated]
  public virtual void m_CAnimateClosem_Eb__14_3()
  {
    Vector2Tween.Pool.Despawn(this._contentPositionTween);
    this._contentPositionTween = (Vector2Tween) null;
  }
}
