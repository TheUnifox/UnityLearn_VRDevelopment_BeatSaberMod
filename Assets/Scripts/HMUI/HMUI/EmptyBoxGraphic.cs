// Decompiled with JetBrains decompiler
// Type: HMUI.EmptyBoxGraphic
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

using UnityEngine;
using UnityEngine.UI;

namespace HMUI
{
  public class EmptyBoxGraphic : Graphic
  {
    [SerializeField]
    protected float _depth = 1f;

    protected override void OnPopulateMesh(VertexHelper vh)
    {
      Rect pixelAdjustedRect = this.GetPixelAdjustedRect();
      Vector4 vector4 = new Vector4(pixelAdjustedRect.x, pixelAdjustedRect.y, pixelAdjustedRect.x + pixelAdjustedRect.width, pixelAdjustedRect.y + pixelAdjustedRect.height);
      Color32 color = (Color32) this.color;
      Vector2 zero = Vector2.zero;
      vh.Clear();
      vh.AddVert(new Vector3(vector4.x, vector4.y, this._depth), color, zero);
      vh.AddVert(new Vector3(vector4.x, vector4.w, this._depth), color, zero);
      vh.AddVert(new Vector3(vector4.z, vector4.w, this._depth), color, zero);
      vh.AddVert(new Vector3(vector4.z, vector4.y, this._depth), color, zero);
      vh.AddVert(new Vector3(vector4.x, vector4.y, -this._depth), color, zero);
      vh.AddVert(new Vector3(vector4.x, vector4.w, -this._depth), color, zero);
      vh.AddVert(new Vector3(vector4.z, vector4.w, -this._depth), color, zero);
      vh.AddVert(new Vector3(vector4.z, vector4.y, -this._depth), color, zero);
      vh.AddTriangle(0, 1, 1);
      vh.AddTriangle(2, 3, 3);
      vh.AddTriangle(4, 5, 5);
      vh.AddTriangle(6, 7, 7);
    }

    public virtual void OnDrawGizmosSelected()
    {
      Rect pixelAdjustedRect = this.GetPixelAdjustedRect();
      Vector4 vector4 = new Vector4(pixelAdjustedRect.x, pixelAdjustedRect.y, pixelAdjustedRect.x + pixelAdjustedRect.width, pixelAdjustedRect.y + pixelAdjustedRect.height);
      Gizmos.matrix = this.transform.localToWorldMatrix;
      Gizmos.color = Color.red;
      Gizmos.DrawLine(new Vector3(vector4.x, vector4.y, this._depth), new Vector3(vector4.x, vector4.w, this._depth));
      Gizmos.DrawLine(new Vector3(vector4.z, vector4.w, this._depth), new Vector3(vector4.z, vector4.y, this._depth));
      Gizmos.DrawLine(new Vector3(vector4.x, vector4.y, -this._depth), new Vector3(vector4.x, vector4.w, -this._depth));
      Gizmos.DrawLine(new Vector3(vector4.z, vector4.w, -this._depth), new Vector3(vector4.z, vector4.y, -this._depth));
    }
  }
}
