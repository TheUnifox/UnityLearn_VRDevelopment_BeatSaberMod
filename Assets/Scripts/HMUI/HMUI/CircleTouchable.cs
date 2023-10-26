// Decompiled with JetBrains decompiler
// Type: HMUI.CircleTouchable
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

using UnityEngine;

namespace HMUI
{
  public class CircleTouchable : Touchable
  {
    [SerializeField]
    protected float _minRadius = 10f;
    [SerializeField]
    protected float _maxRadius = 15f;
    protected RectTransform _containerRect;

    protected override void OnEnable()
    {
      base.OnEnable();
      this.UpdateCachedReferences();
    }

    public virtual void UpdateCachedReferences() => this._containerRect = this.transform as RectTransform;

    public override bool Raycast(Vector2 sp, Camera eventCamera)
    {
      Vector2 localPoint;
      if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(this._containerRect, sp, eventCamera, out localPoint))
        return false;
      float sqrMagnitude = localPoint.sqrMagnitude;
      return (double) sqrMagnitude <= (double) this._maxRadius * (double) this._maxRadius && (double) sqrMagnitude >= (double) this._minRadius * (double) this._minRadius;
    }

    public virtual void OnDrawGizmosSelected()
    {
      Gizmos.matrix = this.transform.localToWorldMatrix;
      this.DrawGizmoCircle((Vector3) this._containerRect.rect.center, this._minRadius, 32);
      this.DrawGizmoCircle((Vector3) this._containerRect.rect.center, this._maxRadius, 32);
      Gizmos.matrix = Matrix4x4.identity;
    }

    public virtual void DrawGizmoCircle(Vector3 center, float radius, int steps)
    {
      Vector3 from = new Vector3(radius, 0.0f, 0.0f) + center;
      for (int index = 1; index <= steps; ++index)
      {
        float f = 6.28318548f * (float) index / (float) steps;
        Vector3 to = new Vector3(Mathf.Cos(f) * radius, Mathf.Sin(f) * radius, 0.0f);
        to += center;
        Gizmos.DrawLine(from, to);
        from = to;
      }
    }
  }
}
