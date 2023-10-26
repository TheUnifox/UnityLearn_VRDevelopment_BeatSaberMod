// Decompiled with JetBrains decompiler
// Type: HMUI.CurvedCanvasSettings
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

using ModestTree;
using UnityEngine;
using UnityEngine.UI;

namespace HMUI
{
  [RequireComponent(typeof (Canvas))]
  [RequireComponent(typeof (CanvasRenderer))]
  public class CurvedCanvasSettings : Graphic
  {
    [SerializeField]
    protected float _radius = 175f;
    [SerializeField]
    protected bool _useFlatInEditMode;
    public const float kMaxElementWidth = 10f;

    public float radius => this._radius;

    public virtual void SetRadius(float value)
    {
      this._radius = value;
      CurvedCanvasSettings.RebuildAndSetup(this.transform);
    }

    protected override void Start()
    {
      base.Start();
      Assert.IsNotNull((object) this.GetComponent<CanvasRenderer>());
      this.raycastTarget = false;
      this.canvas.additionalShaderChannels |= AdditionalCanvasShaderChannels.TexCoord2;
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
      Rect rect = this.rectTransform.rect;
      Vector3 position = this.transform.position;
      vh.Clear();
      vh.AddVert(this.TransformPointFromCanvasTo3D(rect.min), new Color32(), new Vector2());
      vh.AddVert(this.TransformPointFromCanvasTo3D(rect.max), new Color32(), new Vector2());
      vh.AddVert(this.TransformPointFromCanvasTo3D(rect.center), new Color32(), new Vector2());
      vh.AddTriangle(0, 1, 2);
    }

    public virtual Vector3 TransformPointFromCanvasTo3D(Vector2 point) => new Vector3(Mathf.Sin(point.x / this.radius) * this.radius, point.y, Mathf.Cos(point.x / this.radius) * this.radius - this.radius);

    private static void RebuildAndSetup(Transform t)
    {
      Graphic component = t.GetComponent<Graphic>();
      if ((Object) component != (Object) null)
        component.SetAllDirty();
      for (int index = 0; index < t.childCount; ++index)
        CurvedCanvasSettings.RebuildAndSetup(t.GetChild(index));
    }
  }
}
